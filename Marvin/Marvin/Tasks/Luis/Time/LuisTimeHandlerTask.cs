using System;
using System.Linq;
using System.Threading.Tasks;
using Marvin.Luis;
using Marvin.Pipeline;

namespace Marvin.Tasks.Luis.Time
{
    public class LuisTimeHandlerTask : BotMessagePipelineTask
    {
        public override async Task<BotMessage> HandleMessage(BotMessage message)
        {
            var luisMessage = message as ILuisMessage;
            if (luisMessage != null)
            {
                var recommendation = luisMessage.Luis.Intents.OrderByDescending(i => i.Score).FirstOrDefault();
                if (recommendation?.Intent == "GetTime")
                {
                    message.Response = $"Over here it's {DateTime.Now.ToShortTimeString()}.";
                    message.IsHandled = true;
                }
            }

            return message;
        }
    }
}