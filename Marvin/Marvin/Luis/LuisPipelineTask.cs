using System.Threading.Tasks;
using Marvin.Pipeline;
using Microsoft.Bot.Builder.Luis;
using Newtonsoft.Json;

namespace Marvin.Luis
{
    /// <summary>
    /// The LUIS pipeline task.
    /// </summary>
    public class LuisPipelineTask : BotMessagePipelineTask
    {
        /// <summary>
        /// Handles the bot message.
        /// This will populate the message with LUIS response data.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Returns the <see cref="LuisMessage"/> that is populated with LUIS response data.</returns>
        public override async Task<BotMessage> HandleMessage(BotMessage message)
        {
            // Run LUIS task
            return await HandleMessageAsync(message);
        }

        /// <summary>
        /// Populates the specified message with LUIS response data.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Returns new instance of <see cref="LuisMessage"/> that is populated with LUIS response data.</returns>
        /// <remarks>
        /// Access the LUIS response data through <see cref="LuisMessage.Luis"/> property.
        /// </remarks>
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

        /// <summary>
        /// Queries the LUIS with text from message.
        /// </summary>
        /// <param name="query">The message text.</param>
        /// <returns>Returns the LUIS response for specified query.</returns>
        private static async Task<LuisResult> GetLuisResponseAsync(string query)
        {
            // Instantiate LUIS service
            var luisService = LuisProvider.GetLuis();

            // Send and handle luis response
            return await luisService.QueryAsync(query);
        }
    }
}