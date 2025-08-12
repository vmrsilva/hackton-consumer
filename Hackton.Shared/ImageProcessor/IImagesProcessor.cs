using System.Collections.Concurrent;

namespace Hackton.Shared.ImageProcessor
{
    public interface IImagesProcessor
    {
        ConcurrentDictionary<string, string> ProcessBatch(string[] imagePaths);
        string ProcessSingleImage(string imagePath);
    }
}
