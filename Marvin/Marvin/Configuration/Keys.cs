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

        /// <summary>
        /// The Windows Store Analytics account identifier.
        /// </summary>
        public static string WindowsStoreAnalyticsId => ConfigurationManager.AppSettings["WindowsAnalyticsId"];

        /// <summary>
        /// The Windows Store Analytics account key.
        /// </summary>
        public static string WindowsStoreAnalyticsKey => ConfigurationManager.AppSettings["WindowsAnalyticsKey"];
    }
}