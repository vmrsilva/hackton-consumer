using Hackton.Shared.FileServices;
using Hackton.Shared.FileServices.Settings;
using Hackton.Shared.ImageProcessor;
using Hackton.Shared.VideoSplit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackton.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureBlobOptions>(configuration.GetSection("AzureBlob"));
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IImagesProcessor, ImagesProcessor>();
            services.AddScoped<IVideoSplitService, VideoSplitService>();
            return services;
        }


    }
}
