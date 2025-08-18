namespace Hackton.Shared.FileServices
{
    public interface IFileService
    {
        Task<Stream> DownloadVideoAsync(string blobName);
    }
}
