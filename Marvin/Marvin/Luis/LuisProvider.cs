using Marvin.Configuration;
using Microsoft.Bot.Builder.Luis;

namespace Marvin.Luis
{
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
                        "9651ba69-29fe-4331-97c3-42bd349643b7",
                        Keys.LuisKey));
        }
    }
}