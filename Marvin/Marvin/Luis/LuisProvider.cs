using System.Threading.Tasks;
using Marvin.Configuration;
using Marvin.WindowsAnalytics;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Marvin.Luis
{
    /// <summary>
    /// The LUIS extensions.
    /// </summary>
    public static class LuisExtensions
    {
        public static async Task<LuisMessage> AddLuisAsync(this BotMessage message, bool enableDebug = false)
        {
            // Translate to LUIS message
            var luisMessage = BotMessage.Populate<BotMessage, LuisMessage>(message);

            // Do nothing if debug is set
            if (luisMessage.IsDebug)
                return luisMessage;

            // Pass message through luis pipeline
            if (enableDebug)
                await LuisDebugPipeAsync(luisMessage);
            else await LuisPipeAsync(luisMessage);

            return luisMessage;
        }

        private static async Task LuisPipeAsync(LuisMessage message)
        {
            message.Luis = await PopulateWithLuisAsync(message.Text);
        }

        private static async Task LuisDebugPipeAsync(LuisMessage message)
        {
            if (message.Text.StartsWith("debug.luis:"))
            {
                // Construct LUIS query and request response
                var luisQuery = message.Text.Replace("debug.luis:", "");
                message.Luis = await PopulateWithLuisAsync(luisQuery);

                // Populate debug data
                message.DebugData = JsonConvert.SerializeObject(message.Luis);
                message.IsDebug = true;
            }
            else await LuisPipeAsync(message);
        }

        private static async Task<LuisResult> PopulateWithLuisAsync(string query)
        {
            // Instantiate LUIS service
            var luisService = LuisProvider.GetLuis();

            // Send and handle luis response
            return await luisService.QueryAsync(query);
        }
    }

    /// <summary>
    /// The LUIS service provider.
    /// </summary>
    public static class LuisProvider
    {
        /// <summary>
        /// Gets <see cref="ILuisService"/>.
        /// </summary>
        /// <returns>Returns new instance of <see cref="ILuisService"/> class.</returns>
        public static ILuisService GetLuis()
        {
            return new LuisService(
                    new LuisModelAttribute(
                        Keys.LuisModelId,
                        Keys.LuisKey));
        }
    }
}