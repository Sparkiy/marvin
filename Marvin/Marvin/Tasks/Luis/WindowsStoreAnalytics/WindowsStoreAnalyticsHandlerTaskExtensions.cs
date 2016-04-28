using Marvin.Pipeline;

namespace Marvin.Tasks.Luis.WindowsStoreAnalytics
{
    public static class WindowsStoreAnalyticsHandlerTaskExtensions
    {
        public static BotMessagePipeline AddLuisWindowsStoreAnalytics(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new WindowsStoreAnalyticsHandlerTask());
            return pipeline;
        }
    }
}