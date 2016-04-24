using System.Threading.Tasks;
using Marvin.Pipeline;
using Microsoft.Bot.Builder.Luis;
using Newtonsoft.Json;

namespace Marvin.Luis
{
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
}