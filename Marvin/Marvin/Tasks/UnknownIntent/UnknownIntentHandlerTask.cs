using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Marvin.Pipeline;

namespace Marvin.Tasks.UnknownIntent
{
    public class UnknownIntentHandlerTask : BotMessagePipelineTask
    {
        public override async Task<BotMessage> HandleMessage(BotMessage message)
        {
            var responses = new List<string>()
            {
                $"I'm sorry {message.From.Name}, I'm afraid I can't do that.",
                "Ah well, there's always hope.",
                "Did you say something?"
            };

            message.Response = responses[new Random().Next(0, responses.Count)];

            return message;
        }
    }
}