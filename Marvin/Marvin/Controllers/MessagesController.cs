using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Marvin.Luis;
using Marvin.Responses;
using Marvin.WindowsAnalytics;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;

namespace Marvin.Controllers
{
    /// <summary>
    /// The messages controller.
    /// </summary>
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                // Respond to empty message
                if (!message.HasContent())
                    return message.CreateReplyMessage("I didn't understand that :/", "en");

                // Respont to what message
                if (message.Text.ToLowerInvariant().Trim().Trim('?', '!', '.', ';', '-').Equals("what"))
                    return message.RespondWhat();

                // Instantiate LUIS service
                var luisService = LuisProvider.GetLuis();

                // Send and handle luis response
                var luisResponse = await luisService.QueryAsync(message.Text);
                if (luisResponse.Intents.Count > 0)
                {
                    var mainIntent = luisResponse.Intents.OrderByDescending(i => i.Score).First();
                    if (mainIntent.Intent == "GetTime")
                    {
                        return message.CreateReplyMessage($"Over here it's {DateTime.Now.ToShortTimeString()}.", "en");
                    }
                    else if (mainIntent.Intent == "GetStoreAcquisitions")
                    {
                        return await message.HandleWindowsStoreAnalyticsQueryAsync(luisResponse);
                    }
                }

                // Return our reply to the user
                return message.RespondUnknownIntent();
            }
            else
            {
                return this.HandleSystemMessage(message);
            }
        }

        /// <summary>
        /// Handles the system messages.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Returns message containing the response to an system message; <c>null</c> if system message is unknown.</returns>
        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}