using Marvin.Pipeline;

namespace Marvin.Tasks.UnknownIntent
{
    public static class UnknownIntentHandlerTaskExtensions
    {
        public static BotMessagePipeline AddUnknownIntention(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new UnknownIntentHandlerTask());
            return pipeline;
        }
    }
}