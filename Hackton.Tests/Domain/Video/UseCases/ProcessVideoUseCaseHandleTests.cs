using Bogus;
using Hackton.Domain.Enums;
using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Domain.Interfaces.Video.UseCases;
using Hackton.Domain.Interfaces.VideoResult;
using Hackton.Domain.Video.Entity;
using Hackton.Domain.Video.Exceptions;
using Hackton.Domain.Video.UseCases;
using Hackton.Domain.VideoResult.Entity;
using Hackton.Shared.Dto.Video;
using Hackton.Shared.Dto.VideoSplit;
using Hackton.Shared.FileServices;
using Hackton.Shared.ImageProcessor;
using Hackton.Shared.VideoSplit;
using Moq;
using Xunit;

namespace Hackton.Tests.Domain.Video.UseCases
{
    public class ProcessVideoUseCaseHandleTests
    {
        private readonly IProcessVideoUseCaseHandle _processVideoUseCaseHandle;
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IImagesProcessor> _imagesProcessor;
        private readonly Mock<IVideoRepository> _videoRepository;
        private readonly Mock<IVideoResultRepository> _videoResultRepository;
        private readonly Mock<IVideoSplitService> _videoSplitService;
        public ProcessVideoUseCaseHandleTests()
        {
            _fileService = new Mock<IFileService>();
            _imagesProcessor = new Mock<IImagesProcessor>();
            _videoRepository = new Mock<IVideoRepository>();
            _videoResultRepository = new Mock<IVideoResultRepository>();
            _videoSplitService = new Mock<IVideoSplitService>();

            _processVideoUseCaseHandle = new ProcessVideoUseCaseHandle(_fileService.Object,
                                                                       _imagesProcessor.Object,
                                                                       _videoRepository.Object,
                                                                       _videoResultRepository.Object,
                                                                       _videoSplitService.Object);
        }

        [Fact(DisplayName = "Should Return VideoNotFoundException When Video Does Not Exist")]
        public async Task ShouldReturnVideoNotFoundExceptionWhenVideoDoesNotExist()
        {
            var mockVideoMessageDto = new Faker<VideoMessageDto>()
                 .RuleFor(x => x.VideoId, f => f.Random.Guid())
                 .RuleFor(x => x.FileName, f => f.System.FileName())
                 .Generate();

            _videoRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((VideoEntity)null);

            await Assert.ThrowsAsync<VideoNotFoundException>(() => _processVideoUseCaseHandle.Handle(mockVideoMessageDto));

            _videoRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _videoRepository.Verify(x => x.UpdateAsync(It.IsAny<VideoEntity>()), Times.Never);
            _fileService.Verify(x => x.DownloadVideoAsync(It.IsAny<string>()), Times.Never);
            _videoSplitService.Verify(x => x.ProcessVideoSPlit(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _imagesProcessor.Verify(x => x.ProcessSingleImage(It.IsAny<string>()), Times.Never);
            _videoResultRepository.Verify(x => x.Create(It.IsAny<VideoResultEntity>()), Times.Never);
        }

        [Fact(DisplayName = "Should Return Video Result")]
        public async Task ShouldReturnVideoResult()
        {
            var mockVideoMessageDto = new Faker<VideoMessageDto>()
                 .RuleFor(x => x.VideoId, f => f.Random.Guid())
                 .RuleFor(x => x.FileName, f => f.System.FileName())
                 .Generate();

            var videoMock = new Faker<VideoEntity>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => f.Lorem.Word())
                .RuleFor(x => x.Description, f => f.Lorem.Sentence(2))
                .RuleFor(x => x.Status, VideoStatusEnum.NaFila)
                .Generate();

            _videoRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(videoMock);
            _fileService.Setup(x => x.DownloadVideoAsync(It.IsAny<string>())).ReturnsAsync(new MemoryStream());
            var framesFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(framesFolder);

            var tempFilePath = Path.Combine(framesFolder, "tempVideo.mp4");
            File.WriteAllText(tempFilePath, string.Empty);

            var videoSplitResultDtoMock = new VideoSplitResultDto
            {
                Files = new[]
                     {
            Path.Combine(framesFolder, "frame_0001.png"),
            Path.Combine(framesFolder, "frame_0002.png")
                    },
                TempFilePath = tempFilePath,
                FramesFolder = framesFolder
            };
            _videoSplitService.Setup(x => x.ProcessVideoSPlit(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(videoSplitResultDtoMock);

            await _processVideoUseCaseHandle.Handle(mockVideoMessageDto);

            _videoRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _videoRepository.Verify(x => x.UpdateAsync(It.IsAny<VideoEntity>()), Times.Exactly(2));
            _fileService.Verify(x => x.DownloadVideoAsync(It.IsAny<string>()), Times.Once);
            _videoSplitService.Verify(x => x.ProcessVideoSPlit(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _imagesProcessor.Verify(x => x.ProcessSingleImage(It.IsAny<string>()), Times.Exactly(videoSplitResultDtoMock.Files.Count()));
            _videoResultRepository.Verify(x => x.Create(It.IsAny<VideoResultEntity>()), Times.Once);
        }


    }
}
