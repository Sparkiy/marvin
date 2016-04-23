using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marvin.Configuration;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
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
            if (result.Entities.Count <= 0)
                return message.CreateReplyMessage("Please tell me for what app(s) you want the stats for.");

            RetrieveWindowsStoreData(message, result);

            return message.CreateReplyMessage("Let me contact Microsoft for that info...", "en");
        }

        private static void RetrieveWindowsStoreData(Message message, LuisResult result)
        {
            Task.Run(async () =>
            {
                var connector = new ConnectorClient();

                var client = new Sparkiy.WindowsStore.Analytics.Client.AnalyticsClient();
                await client.AuthorizeAsync(Keys.WindowsStoreAnalyticsId, Keys.WindowsStoreAnalyticsKey);

                var appNames = result.Entities.Where(e => e.Type == "StoreApp");
                foreach (var entity in appNames)
                {
                    // TODO Resolve application id from database
                    string appId = string.Empty;
                    if (entity.Entity == "sparkiy")
                        appId = "9NBLGGH10R5Z";
                    else if (entity.Entity == "quantastic")
                        appId = "9WZDNCRDRKSB";
                    else
                    {
                        connector.Messages.SendMessage(
                            message.CreateReplyMessage($"I don't know an app named {entity.Entity}.", "en"));
                        continue;
                    }

                    var storeResponse = (await client.GetAppAcquisitionsAsync(appId, DateTime.MinValue, DateTime.MaxValue, 1000, 0, true)).ToList();

                    var lastWeekDate = DateTime.Now.ToUniversalTime().Subtract(TimeSpan.FromDays(7)).ToUniversalTime().Date;
                    var totalAcquisitionsCount = storeResponse
                        .Sum(a => a.AcquisitionQuantity)
                        .ToString(CultureInfo.InvariantCulture);
                    var dayAcquisitionsCount = storeResponse
                        .Where(a => a.Date >= lastWeekDate)
                        .Sum(a => a.AcquisitionQuantity)
                        .ToString(CultureInfo.InvariantCulture);

                    connector.Messages.SendMessage(
                        message.CreateReplyMessage(
                            $"{entity.Entity} has total {totalAcquisitionsCount} (+{dayAcquisitionsCount} last 7 days) acquisitions",
                            "en"));
                }
            });
        }
    }
}