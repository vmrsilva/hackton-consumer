using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Domain.Video.UseCases;
using Hackton.Shared.Dto.Video;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
