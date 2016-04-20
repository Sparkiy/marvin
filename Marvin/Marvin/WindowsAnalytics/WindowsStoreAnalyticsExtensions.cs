using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Marvin.Configuration;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Sparkiy.WindowsStore.Analytics.Client.Models.v10;

namespace Marvin.WindowsAnalytics
{
    /// <summary>
    /// The Windows Store Analyitics extensions.
    /// </summary>
    public static class WindowsStoreAnalyticsExtensions
    {
        /// <summary>
        /// Handles the Windows Store Analytics query.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="result">The LUIS response.</param>
        /// <returns>Returns the message with response to Windows Store Analytics API.</returns>
        public static async Task<Message> HandleWindowsStoreAnalyticsQueryAsync(this Message message, LuisResult result)
        {
            var client = new Sparkiy.WindowsStore.Analytics.Client.AnalyticsClient();
            await client.AuthorizeAsync(Keys.WindowsStoreAnalyticsId, Keys.WindowsStoreAnalyticsKey);

            var storeResponse = await client.GetAppAcquisitionsAsync("9NBLGGH10R5Z", DateTime.MinValue, DateTime.MaxValue, 10000, 0, true);

            var response = message.CreateReplyMessage(storeResponse.Sum(a => a.AcquisitionQuantity).ToString(CultureInfo.InvariantCulture), "en");

            return response;
        }
    }
}