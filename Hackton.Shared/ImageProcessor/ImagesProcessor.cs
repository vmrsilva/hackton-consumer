using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Collections.Concurrent;
using ImageSharpImage = SixLabors.ImageSharp.Image;
using L8 = SixLabors.ImageSharp.PixelFormats.L8;
using Rgba32 = SixLabors.ImageSharp.PixelFormats.Rgba32;

namespace Hackton.Shared.ImageProcessor
{
    public class ImagesProcessor : IImagesProcessor, IDisposable
    {
        public ConcurrentDictionary<string, string> ProcessBatch(string[] imagePaths)
        {
            var results = new ConcurrentDictionary<string, string>();

            Parallel.ForEach(imagePaths, imagePath =>
            {
                results[imagePath] = ProcessSingleImage(imagePath);
            });

            return results;
        }

        public string ProcessSingleImage(string imagePath)
        {
            try
            {
                if (!_streamPool.TryTake(out var memoryStream))
                {
                    memoryStream = new MemoryStream();
                }

                try
                {
                    using (var image = ImageSharpImage.Load<Rgba32>(imagePath))
                    {
                        image.Mutate(x => x.Grayscale());

                        memoryStream.SetLength(0);
                        image.SaveAsBmp(memoryStream);
                        memoryStream.Position = 0;

                        using (var l8Image = ImageSharpImage.Load<L8>(memoryStream))
                        {
                            var result = _reader.Decode(l8Image);
                            return result?.Text ?? string.Empty;
                        }
                    }
                }
                finally
                {
                    _streamPool.Add(memoryStream);
                }
            }
            catch (Exception ex)
            {
                return $"Erro: {ex.Message}";
            }
        }

        private static readonly ZXing.ImageSharp.BarcodeReader<L8> _reader = new ZXing.ImageSharp.BarcodeReader<L8>
        {
            Options = new ZXing.Common.DecodingOptions
            {
                PossibleFormats = new[] { ZXing.BarcodeFormat.QR_CODE },
                TryHarder = true
            }
        };

        private readonly ConcurrentBag<MemoryStream> _streamPool = new ConcurrentBag<MemoryStream>();

        public void Dispose()
        {
            foreach (var stream in _streamPool)
            {
                stream?.Dispose();
            }
            _streamPool.Clear();
        }
    }
}
