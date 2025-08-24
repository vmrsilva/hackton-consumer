using Hackton.Consumer;
using Hackton.Domain;
using Hackton.Infrastructure;
using Hackton.Shared;
using Hackton.Shared.Dto.Video;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var fila = configuration.GetSection("MassTransit")["QueueVideos"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Server"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["User"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Password"] ?? string.Empty;

        services.AddShared(configuration);
        services.AddDomain(configuration);
        services.AddInfrastructure(configuration);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<VideoProcessingConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });

                cfg.Message<VideoMessageDto>(m =>
                {
                    m.SetEntityName("video-posted-exchange");
                });

                cfg.ReceiveEndpoint(fila, e =>
                {
                    e.ConfigureConsumer<VideoProcessingConsumer>(context);
                });
            });

        });
    })
    .Build();

host.Run();
