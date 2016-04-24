using System.Collections.Generic;
using System.Threading.Tasks;
using Marvin.Pipeline;
using Microsoft.Bot.Connector;

namespace Marvin.Tasks.Finny
{
    public class PulpFictionHandlerTask : BotMessagePipelineTask
    {
        public override async Task<BotMessage> HandleMessage(BotMessage message)
        {
            if (message.Text.ToLowerInvariant().Trim().Trim('?', '!', '.', ';', '-').Equals("what"))
            {
                var responses = new List<string>()
                {
                    "Say what again.",
                    "English motherfucker do you speak it!?",
                    "SAY WHAT again! And I dare you, I double dare you motherfucker! Say what one more time.",
                    "500 Internal Server Error"
                };

                // Construct an message
                var counter = message.GetBotPerUserInConversationData<int>("WhatCounter");
                message.Response = responses[counter];

                // Increment counter
                message.SetBotPerUserInConversationData("WhatCounter", (counter + 1)%responses.Count);

                // Handle message
                message.IsHandled = true;
            }

            return message;
        }
    }
}