using Marvin.Pipeline;

namespace Marvin.Tasks.EmptyMessage
{
    /// <summary>
    /// The <see cref="EmptyMessageHandlerTask"/> extensions.
    /// </summary>
    public static class EmptyMessageHandlerTaskExtensions
    {
        /// <summary>
        /// Registers the <see cref="EmptyMessageHandlerTask"/> to the <see cref="BotMessagePipeline"/>.
        /// </summary>
        /// <param name="pipeline">The bot message pipeline.</param>
        /// <returns>Returns the <see cref="BotMessagePipeline"/> to enable subsequent calls.</returns>
        public static BotMessagePipeline AddEmptyHandler(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new EmptyMessageHandlerTask());
            return pipeline;
        }
    }
}