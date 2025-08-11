using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Shared.Dto.Video;
using MassTransit;

namespace Hackton.Consumer
{
    public class VideoProcessingConsumer : IConsumer<VideoMessageDto>
    {
        private readonly ILogger<VideoProcessingConsumer> _logger;
        private readonly IUseCaseCommandHandler<VideoMessageDto> _useCaseCommandHandler;


        public VideoProcessingConsumer(ILogger<VideoProcessingConsumer> logger, IUseCaseCommandHandler<VideoMessageDto> useCaseCommandHandler)
        {
            _logger = logger;
            _useCaseCommandHandler = useCaseCommandHandler;
        }

        public async Task Consume(ConsumeContext<VideoMessageDto> context)
        {
            var videoMessage = context.Message;

            try
            {
                _logger.LogInformation("Processing video {VideoId}", videoMessage.VideoId);
                await _useCaseCommandHandler.Handle(videoMessage, context.CancellationToken).ConfigureAwait(false);
                await Task.Delay(5000);
            }
            catch (Exception ex)
            {

                _logger.LogError("Error processing video {VideoId}", ex.Message);

            }
        }
    }
}
