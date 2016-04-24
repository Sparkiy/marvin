using Marvin.Controllers;
using Marvin.Pipeline;

namespace Marvin.WindowsAnalytics
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