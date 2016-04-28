using Marvin.Pipeline;

namespace Marvin.Tasks.Funny
{
    public static class PulpFictionHandlerTaskExtensions
    {
        public static BotMessagePipeline AddSayWhat(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new PulpFictionHandlerTask());
            return pipeline;
        }
    }
}