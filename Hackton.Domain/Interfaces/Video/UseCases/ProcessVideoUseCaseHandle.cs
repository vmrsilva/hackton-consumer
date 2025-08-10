using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Shared.Dto.Video;
using Hackton.Shared.FileServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace Hackton.Domain.Interfaces.Video.UseCases
{
    public class ProcessVideoUseCaseHandle : IUseCaseCommandHandler<VideoMessageDto>
    {
        private readonly IFileService _fileService;

        public ProcessVideoUseCaseHandle(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task Handle(VideoMessageDto command, CancellationToken cancellation = default)
        {
            // Baixa o vídeo para stream
            var fileStream = await _fileService.DownloadVideoAsync("video.mp4");

            // Salva vídeo em arquivo temporário
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".mp4");
            using (var file = File.Create(tempFile))
            {
                await fileStream.CopyToAsync(file, cancellation);
            }

            // Cria pasta temporária para frames
            string framesFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(framesFolder);

            // Padrão simples numérico para frames
            string outputPattern = Path.Combine(framesFolder, "frame_%04d.png");

            // Comando FFmpeg para extrair 1 frame por segundo, sobrescrevendo (-y)
            string parameters = $"-y -i \"{tempFile}\" -vf fps=1 \"{outputPattern}\"";

            var conversion = FFmpeg.Conversions.New()
                .AddParameter(parameters, ParameterPosition.PreInput);

            await conversion.Start();

            var resultados = new List<(TimeSpan, string)>();

            var barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }
                }
            };

            // Lista arquivos gerados
            var files = Directory.GetFiles(framesFolder, "frame_*.png");

            // fps=1 → cada frame equivale a 1 segundo
            for (int i = 0; i < files.Length; i++)
            {
                string framePath = files[i];
                TimeSpan timestamp = TimeSpan.FromSeconds(i);

                using var bitmap = new Bitmap(framePath);
                var result = barcodeReader.Decode(bitmap);
                if (result != null)
                {
                    resultados.Add((timestamp, result.Text));
                }
            }

            // Limpa arquivos temporários
            Directory.Delete(framesFolder, true);
            File.Delete(tempFile);

            // Aqui você pode salvar ou enviar os resultados
            foreach (var (time, text) in resultados)
            {
                Console.WriteLine($"QR Code detectado em {time}: {text}");
            }
        }
    }
}
