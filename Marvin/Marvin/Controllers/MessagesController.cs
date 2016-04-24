using System.Threading.Tasks;
using System.Web.Http;
using Marvin.Luis;
using Marvin.Pipeline;
using Marvin.Tasks.EmptyMessage;
using Marvin.Tasks.Finny;
using Marvin.Tasks.Luis.Time;
using Marvin.Tasks.UnknownIntent;
using Marvin.WindowsAnalytics;
using Microsoft.Bot.Builder.Dialogs;
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
                return await message
                    .ConstructPipeline()
                    .AddEmptyHandler()
                    .AddSayWhat()
                    .AddLuis()
                    .AddLuisTime()
                    .AddLuisWindowsStoreAnalytics()
                    .AddUnknownIntention()
                    .RunAsync();
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