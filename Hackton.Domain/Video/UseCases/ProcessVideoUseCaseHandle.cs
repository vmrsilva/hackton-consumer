using Hackton.Domain.Interfaces.Video.UseCases;
using Hackton.Shared.Dto.Video;
using Hackton.Shared.FileServices;
using Hackton.Shared.ImageProcessor;
using Xabe.FFmpeg;

namespace Hackton.Domain.Video.UseCases
{
    public class ProcessVideoUseCaseHandle : IProcessVideoUseCaseHandle
    {
        private readonly IFileService _fileService;
        private readonly IImagesProcessor _imagesProcessor;
        public ProcessVideoUseCaseHandle(IFileService fileService, IImagesProcessor imagesProcessor)
        {
            _fileService = fileService;
            _imagesProcessor = imagesProcessor;
        }

        public async Task Handle(VideoMessageDto command, CancellationToken cancellation = default)
        {
            var fileStream = await _fileService.DownloadVideoAsync("video.mp4");

            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".mp4");
            using (var file = File.Create(tempFile))
            {
                await fileStream.CopyToAsync(file, cancellation);
            }

            string framesFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(framesFolder);

            string outputPattern = Path.Combine(framesFolder, "frame_%04d.png");

            string parameters = $"-y -i \"{tempFile}\" -vf fps=1 \"{outputPattern}\"";

            var conversion = FFmpeg.Conversions.New()
                .AddParameter(parameters, ParameterPosition.PreInput);

            await conversion.Start();

            var resultados = new List<(TimeSpan, string)>();


            var files = Directory.GetFiles(framesFolder, "frame_*.png");


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

            Directory.Delete(framesFolder, true);
            File.Delete(tempFile);

            foreach (var (time, text) in resultados)
            {
                Console.WriteLine($"QR Code detectado em {time}: {text}");
            }
        }
    }
}
