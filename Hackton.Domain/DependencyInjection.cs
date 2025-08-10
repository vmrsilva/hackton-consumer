using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Domain.Interfaces.Video.UseCases;
using Hackton.Shared.Dto.Video;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUseCaseCommandHandler<VideoMessageDto>, ProcessVideoUseCaseHandle>();
            return services;
        }
    }
}
