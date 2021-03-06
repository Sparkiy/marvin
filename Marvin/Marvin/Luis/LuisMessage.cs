using Marvin.Pipeline;
using Microsoft.Bot.Builder.Luis;
using Newtonsoft.Json;

namespace Marvin.Luis
{
    /// <summary>
    /// The LUIS message.
    /// </summary>
    public class LuisMessage : BotMessage, ILuisMessage
    {
        /// <summary>
        /// The LUIS result for this instance.
        /// </summary>
        [JsonProperty(PropertyName = "Luis")]
        public LuisResult Luis { get; set; }
    }
}