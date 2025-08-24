using Hackton.Shared.Dto.VideoSplit;
using Xabe.FFmpeg;

namespace Hackton.Shared.VideoSplit
{
    public class VideoSplitService : IVideoSplitService
    {
        public async Task<VideoSplitResultDto> ProcessVideoSPlit(Stream fileStream, string extensionFile, CancellationToken cancellation = default)
        {
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "." + extensionFile);

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

            return new VideoSplitResultDto
            {
                Files = files,
                TempFilePath = tempFile,
                FramesFolder = framesFolder
            };
        }
    }
}
