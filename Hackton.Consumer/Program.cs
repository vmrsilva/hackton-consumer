//using Hackton.Consumer;
//using Hackton.Shared;

//var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();

//builder.Services.AddShared(builder.Configuration);

//var host = builder.Build();
//host.Run();

using Hackton.Consumer;
using Hackton.Domain;
using Hackton.Shared;
using Hackton.Shared.Dto.Video;
using MassTransit;
using Hackton.Infrastructure;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var fila = configuration.GetSection("MassTransit")["QueueVideos"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Server"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["User"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Password"] ?? string.Empty;

        //services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddShared(configuration);
        services.AddDomain(configuration);
        services.AddInfrastructure(configuration);
        //services.AddScoped<IContactRepository, ContactRepository>();
        //services.AddScoped<IContactService, ContactService>();
        //services.AddDbContext<TechChallangeContext>(options => options.UseSqlServer(configuration.GetConnectionString("Database")));

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
