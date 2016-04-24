using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Marvin.Luis;
using Marvin.Responses;
using Marvin.WindowsAnalytics;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;

namespace Marvin.Controllers
{
    public static class UnknownIntentHandlerTaskExtensions
    {
        public static BotMessagePipeline AsUnknown(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new UnknownIntentHandlerTask());
            return pipeline;
        }
    }

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

    public static class PulpFictionHandlerTaskExtensions
    {
        public static BotMessagePipeline SayWhat(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new PulpFictionHandlerTask());
            return pipeline;
        }
    }

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

    public static class EmptyMessageHandlerTaskExtensions
    {
        public static BotMessagePipeline HandleEmpty(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new EmptyMessageHandlerTask());
            return pipeline;
        }
    }

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

    public static class LuisPipelineTaskExtensions
    {
        public static BotMessagePipeline AddLuis(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new LuisPipelineTask());
            return pipeline;
        }
    }

    public class LuisPipelineTask : BotMessagePipelineTask
    {
        public override async Task<BotMessage> HandleMessage(BotMessage message)
        {
            // Run LUIS task
            return await HandleMessageAsync(message);
        }

        private static async Task<LuisMessage> HandleMessageAsync(BotMessage message)
        {
            // Translate to LUIS message
            var luisMessage = BotMessage.Populate<BotMessage, LuisMessage>(message);

            // Populate LUIS data
            luisMessage.Luis = await GetLuisResponseAsync(message.Text);

            // Check if we need to debug the message
            if (luisMessage.IsDebug && luisMessage.DebugParam == "luis")
            {
                luisMessage.Response = JsonConvert.SerializeObject(luisMessage.Luis);
                luisMessage.IsHandled = true;
            }

            return luisMessage;
        }

        private static async Task<LuisResult> GetLuisResponseAsync(string query)
        {
            // Instantiate LUIS service
            var luisService = LuisProvider.GetLuis();

            // Send and handle luis response
            return await luisService.QueryAsync(query);
        }
    }

    public abstract class BotMessagePipelineTask
    {
        public virtual async Task<BotMessage> ResultMessage(BotMessage message)
        {
            return message;
        }

        public virtual async Task<BotMessage> HandleMessage(BotMessage message)
        {
            return message;
        }
    }

    public static class BotMessagePipelineExtensions
    {
        public static BotMessagePipeline ConstructPipeline(this Message message)
        {
            return new BotMessagePipeline(message);
        }
    }

    public class BotMessagePipeline
    {
        private readonly Message message;
        private readonly List<BotMessagePipelineTask> pipeline = new List<BotMessagePipelineTask>();


        public BotMessagePipeline(Message message)
        {
            this.message = message;
        }


        public BotMessagePipelineTask RegisterTask(BotMessagePipelineTask task)
        {
            this.pipeline.Add(task);
            return task;
        }

        public async Task<Message> RunAsync()
        {
            // Translate to bot message
            var botMessage = BotMessage.Populate<Message, BotMessage>(this.message);

            // First run through pipeline
            var pipelineTaskIndex = 0;
            for (; pipelineTaskIndex < this.pipeline.Count; pipelineTaskIndex++)
            {
                botMessage = await this.pipeline[pipelineTaskIndex].HandleMessage(botMessage);

                // Break the pipeline execution if message was handled
                if (botMessage.IsHandled)
                    break;
            }

            // Reverse run through pipeline
            for (; pipelineTaskIndex >= 0; pipelineTaskIndex--)
                botMessage = await this.pipeline[pipelineTaskIndex].ResultMessage(botMessage);

            var responseMessage = botMessage.CreateReplyMessage(botMessage.Response, "en");
            responseMessage.BotConversationData = botMessage.BotConversationData;
            responseMessage.BotPerUserInConversationData = botMessage.BotPerUserInConversationData;
            responseMessage.BotUserData = botMessage.BotUserData;

            return responseMessage;
        }
    }

    public static class LuisTimeHandlerTaskExtensions
    {
        public static BotMessagePipeline HandleLuisTime(this BotMessagePipeline pipeline)
        {
            pipeline.RegisterTask(new LuisTimeHandlerTask());
            return pipeline;
        }
    }

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

    /// <summary>
    /// The messages controller.
    /// </summary>
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                return await message
                    .ConstructPipeline()
                    .HandleEmpty()
                    .SayWhat()
                    .AddLuis()
                    .HandleLuisTime()
                    .AsUnknown()
                    .RunAsync();
            }
            else
            {
                return this.HandleSystemMessage(message);
            }
        }

        /// <summary>
        /// Handles the system messages.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Returns message containing the response to an system message; <c>null</c> if system message is unknown.</returns>
        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}