using Microsoft.Bot.Connector;

namespace Marvin.Pipeline
{
    public static class BotMessagePipelineExtensions
    {
        public static BotMessagePipeline ConstructPipeline(this Message message)
        {
            return new BotMessagePipeline(message);
        }
    }
}