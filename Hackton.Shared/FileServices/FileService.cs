using Azure.Storage.Blobs;
using Hackton.Shared.FileServices.Settings;
using Microsoft.Extensions.Options;

namespace Hackton.Shared.FileServices
{
    public class FileService : IFileService
    {
        private readonly AzureBlobOptions _options;
        public FileService(IOptions<AzureBlobOptions> options)
        {
            _options = options.Value;
        }

        public async Task<Stream> DownloadVideoAsync(string blobName)
        {
            if (string.IsNullOrEmpty(blobName))
                throw new ArgumentException("Nome do blob inválido", nameof(blobName));

            var blobServiceClient = new BlobServiceClient(_options.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_options.VideoContainerName);

            var blobClient = containerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
                throw new FileNotFoundException($"Arquivo '{blobName}' não encontrado.");

            var downloadResponse = await blobClient.DownloadAsync();

            return downloadResponse.Value.Content;
        }
    }
}
