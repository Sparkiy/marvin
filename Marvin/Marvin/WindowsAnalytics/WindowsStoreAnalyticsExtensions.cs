using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Marvin.Configuration;
using Marvin.Controllers;
using Marvin.Luis;
using Marvin.Pipeline;
using Microsoft.Bot.Connector;
using Sparkiy.WindowsStore.Analytics.Client.Models.v10;

namespace Marvin.WindowsAnalytics
{
    /// <summary>
    /// The Windows Store Analyitics extensions.
    /// </summary>
    public class WindowsStoreAnalyticsHandlerTask : BotMessagePipelineTask
    {
        public override async Task<BotMessage> HandleMessage(BotMessage message)
        {
            var luisMessage = message as LuisMessage;
            if (luisMessage != null)
            {
                var recommendation = luisMessage.Luis.Intents.OrderByDescending(i => i.Score).FirstOrDefault();
                if (recommendation?.Intent == "GetStoreAcquisitions")
                {
                    var response = await this.HandleWindowsStoreAnalyticsQueryAsync(luisMessage);
                    response.IsHandled = true;
                    return response;
                }
            }

            return message;
        }

        /// <summary>
        /// Handles the Windows Store Analytics query.
        /// </summary>
        /// <param name="message">The LUIS message.</param>
        /// <returns>Returns the message with response to Windows Store Analytics API.</returns>
        public async Task<LuisMessage> HandleWindowsStoreAnalyticsQueryAsync(LuisMessage message)
        {
            if (message.Luis.Entities.Count <= 0)
            {
                message.Response = "Please tell me for what app(s) you want the stats for.";
                return message;
            }

            this.RetrieveWindowsStoreData(message);

            message.Response = "Let me contact Microsoft for that info...";
            return message;
        }

        private void RetrieveWindowsStoreData(LuisMessage message)
        {
            Task.Run(async () =>
            {
                var connector = new ConnectorClient();

                var client = new Sparkiy.WindowsStore.Analytics.Client.AnalyticsClient();
                await client.AuthorizeAsync(Keys.WindowsStoreAnalyticsId, Keys.WindowsStoreAnalyticsKey);

                var appNames = message.Luis.Entities.Where(e => e.Type == "StoreApp");
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