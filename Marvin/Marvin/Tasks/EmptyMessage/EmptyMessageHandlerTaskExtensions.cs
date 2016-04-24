using Marvin.Pipeline;

namespace Marvin.Tasks.EmptyMessage
{
    public static class EmptyMessageHandlerTaskExtensions
    {
        public static BotMessagePipeline AddEmptyHandler(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new EmptyMessageHandlerTask());
            return pipeline;
        }
    }
}