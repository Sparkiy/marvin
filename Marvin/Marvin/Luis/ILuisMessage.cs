using Microsoft.Bot.Builder.Luis;

namespace Marvin.Luis
{
    /// <summary>
    /// The LUIS message.
    /// </summary>
    public interface ILuisMessage
    {
        /// <summary>
        /// The LUIS result for this instance.
        /// </summary>
        LuisResult Luis { get; set; }
    }
}