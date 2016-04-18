using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Marvin.Luis;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;

namespace Marvin.Controllers
{
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
                
                // Instantiate LUIS service
                var luisService = LuisProvider.GetLuis();

                // Send and handle luis response
                var luisResponse = await luisService.QueryAsync(message.Text);
                if (luisResponse.Intents.Count > 0)
                {
                    var mainIntent = luisResponse.Intents.First();
                    if (mainIntent.Intent == "GetTime")
                    {
                        return message.CreateReplyMessage($"Over here it's {DateTime.Now.ToShortTimeString()}.", "en");
                    }
                }

                // Return our reply to the user
                return message.CreateReplyMessage($"I didn't understand that :(", "en");
            }
            else
            {
                return this.HandleSystemMessage(message);
            }
        }

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