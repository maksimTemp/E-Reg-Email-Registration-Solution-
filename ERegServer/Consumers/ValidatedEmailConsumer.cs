using MassTransit;
using SharedLibrary.Messages;

namespace ERegServer.Consumers
{
    /// <summary>
    /// Consumer for processing ValidatedEmailCodeMessage messages.
    /// </summary>
    public class ValidatedEmailConsumer : IConsumer<ValidatedEmailCodeMessage>
    {
        private readonly ILogger<ValidatedEmailConsumer> _logger;

        public ValidatedEmailConsumer(ILogger<ValidatedEmailConsumer> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Consume method to process incoming validated email code messages.
        /// </summary>
        public async Task Consume(ConsumeContext<ValidatedEmailCodeMessage> context)
        {
            var message = context.Message;
            _logger.LogInformation($"Email {message.Email} has been verified with the code {message.InputCode}");
        }

    }
}
