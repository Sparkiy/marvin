using Marvin.Pipeline;

namespace Marvin.Luis
{
    public static class LuisPipelineTaskExtensions
    {
        public static BotMessagePipeline AddLuis(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new LuisPipelineTask());
            return pipeline;
        }
    }
}