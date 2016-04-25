using Marvin.Pipeline;

namespace Marvin.Luis
{
    /// <summary>
    /// The <see cref="LuisPipelineTask"/> extenions for <see cref="BotMessagePipeline"/>.
    /// </summary>
    public static class LuisPipelineTaskExtensions
    {
        /// <summary>
        /// Adds the LUIS task to the pipeline.
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns>Returns the pipeline to enable subsequent calls.</returns>
        public static BotMessagePipeline AddLuis(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new LuisPipelineTask());
            return pipeline;
        }
    }
}