using System.Threading.Tasks;
using Marvin.Pipeline;
using Microsoft.Bot.Connector;

namespace Marvin.Tasks.EmptyMessage
{
    public class EmptyMessageHandlerTask : BotMessagePipelineTask
    {
        public override async Task<BotMessage> HandleMessage(BotMessage message)
        {
            if (!message.HasContent())
            {
                message.Response = "I didn't understand that :/";
                message.IsHandled = true;
            }

            return message;
        }
    }
}