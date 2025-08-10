using Hackton.Shared.FileServices;
using Hackton.Shared.FileServices.Settings;
using Hackton.Shared.Messaging.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureBlobOptions>(configuration.GetSection("AzureBlob"));
            services.AddScoped<IFileService, FileService>();
            return services;
        }


    }
}
