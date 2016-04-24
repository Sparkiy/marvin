using Marvin.Pipeline;

namespace Marvin.Tasks.Luis.Time
{
    public static class LuisTimeHandlerTaskExtensions
    {
        public static BotMessagePipeline AddLuisTime(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new LuisTimeHandlerTask());
            return pipeline;
        }
    }
}