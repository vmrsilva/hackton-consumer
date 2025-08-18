namespace Hackton.Shared.FileServices.Settings
{
    public record AzureBlobOptions
    {
        public string ConnectionString { get; set; }
        public string VideoContainerName { get; set; }
    }
}
