using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace Marvin.Pipeline
{
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
}