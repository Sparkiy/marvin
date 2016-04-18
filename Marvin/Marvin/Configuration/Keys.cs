using System.Configuration;

namespace Marvin.Configuration
{
    /// <summary>
    /// The keys configuration.
    /// </summary>
    public static class Keys
    {
        /// <summary>
        /// Gets the LUIS key.
        /// </summary>
        public static string LuisKey => ConfigurationManager.AppSettings["LuisKey"];

        /// <summary>
        /// Gets the Microsoft Linguistic Analysis API key.
        /// </summary>
        public static string LinguisticKey => ConfigurationManager.AppSettings["LinguisticKey"];
    }
}