using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Shared.Dto.Video;
using Hackton.Shared.FileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var id = command.VideoId;

            var fileStream = await _fileService.DownloadVideoAsync(command.FileUrl);




            //string tempFile = Path.GetTempFileName() + ".mp4";
            //using ( fileStream )
            //{
            //    await videoStream.CopyToAsync(fileStream);
            //}

            //var resultados = new List<(TimeSpan, string)>();
            //var barcodeReader = new BarcodeReaderGeneric
            //{
            //    AutoRotate = true,
            //    Options = new DecodingOptions
            //    {
            //        TryHarder = true,
            //        PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }
            //    }
            //};

            //// Extrai frames de 1 em 1 segundo (ajuste conforme necessário)
            //await FFMpegArguments
            //    .FromFileInput(tempFile)
            //    .OutputToPipe(new RawVideoPipeSink(frame =>
            //    {
            //        using (var bitmap = new Bitmap(frame.Image))
            //        {
            //            var result = barcodeReader.Decode(bitmap);
            //            if (result != null)
            //            {
            //                resultados.Add((frame.Timestamp, result.Text));
            //            }
            //        }
            //    }), options => options
            //        .WithVideoFilters(filterOptions => filterOptions.Fps(1))
            //        .WithFrameOutputCount(null))
            //    .ProcessAsynchronously();

            //File.Delete(tempFile);
            //var xreturn = resultados;














            throw new NotImplementedException();
        }
    }
}
