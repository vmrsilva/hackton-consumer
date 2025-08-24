using Hackton.Domain.Enums;
using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Domain.Interfaces.Video.UseCases;
using Hackton.Domain.Interfaces.VideoResult;
using Hackton.Domain.Video.Entity;
using Hackton.Domain.Video.Exceptions;
using Hackton.Domain.VideoResult.Entity;
using Hackton.Shared.Dto.Video;
using Hackton.Shared.FileServices;
using Hackton.Shared.ImageProcessor;
using Hackton.Shared.VideoSplit;

namespace Hackton.Domain.Video.UseCases
{
    public class ProcessVideoUseCaseHandle : IProcessVideoUseCaseHandle
    {
        private readonly IFileService _fileService;
        private readonly IImagesProcessor _imagesProcessor;
        private readonly IVideoRepository _videoRepository;
        private readonly IVideoResultRepository _videoResultRepository;
        private readonly IVideoSplitService _videoSplitService;
        public ProcessVideoUseCaseHandle(IFileService fileService,
                                         IImagesProcessor imagesProcessor,
                                         IVideoRepository videoRepository,
                                         IVideoResultRepository videoResultRepository,
                                         IVideoSplitService videoSplitService)
        {
            _fileService = fileService;
            _imagesProcessor = imagesProcessor;
            _videoRepository = videoRepository;
            _videoResultRepository = videoResultRepository;
            _videoSplitService = videoSplitService;
        }

        public async Task Handle(VideoMessageDto command, CancellationToken cancellation = default)
        {
            var videoDb = await _videoRepository.GetByIdAsync(command.VideoId).ConfigureAwait(false);

            if (videoDb is null)
                throw new VideoNotFoundException();

            await UpdateVideoStatusAsync(videoDb, VideoStatusEnum.Processando).ConfigureAwait(false);

            var fileStream = await _fileService.DownloadVideoAsync(command.FileName);

            var extensionFile = command.FileName.Split('.')[1];

            var splitResult = await _videoSplitService.ProcessVideoSPlit(fileStream, extensionFile, cancellation).ConfigureAwait(false);

            var files = splitResult.Files;
            var resultados = new List<(TimeSpan, string)>();

            for (int i = 0; i < files.Length; i++)
            {
                string framePath = files[i];
                TimeSpan timestamp = TimeSpan.FromSeconds(i);

                var result = _imagesProcessor.ProcessSingleImage(framePath);
                if (!string.IsNullOrEmpty(result))
                {
                    resultados.Add((timestamp, result));
                }
            }

            if (!string.IsNullOrEmpty(splitResult.FramesFolder) && Directory.Exists(splitResult.FramesFolder))
                Directory.Delete(splitResult.FramesFolder, true);

            if (!string.IsNullOrWhiteSpace(splitResult.TempFilePath) && File.Exists(splitResult.TempFilePath))
                File.Delete(splitResult.TempFilePath);

            await UpdateVideoStatusAsync(videoDb, VideoStatusEnum.Concluido).ConfigureAwait(false);

            var resulte = resultados.Select(r => new ResultItem
            {
                Time = r.Item1,
                Description = r.Item2
            }).ToList();
            var videoResult = new VideoResultEntity(videoDb.Id, resulte);

            await _videoResultRepository.Create(videoResult).ConfigureAwait(false);

            foreach (var (time, text) in resultados)
            {
                Console.WriteLine($"QR Code detectado em {time}: {text}");
            }
        }

        private async Task UpdateVideoStatusAsync(VideoEntity videoDb, VideoStatusEnum status)
        {
            videoDb.ChangeStatus(status);
            await _videoRepository.UpdateAsync(videoDb).ConfigureAwait(false);
        }
    }
}
