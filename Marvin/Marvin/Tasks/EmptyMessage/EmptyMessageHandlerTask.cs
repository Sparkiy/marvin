using System.Threading.Tasks;
using Marvin.Pipeline;
using Microsoft.Bot.Connector;

namespace Marvin.Tasks.EmptyMessage
{
    /// <summary>
    /// The empty message handler.
    /// </summary>
    public class EmptyMessageHandlerTask : BotMessagePipelineTask
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        /// <summary>
        /// Handles the empty message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>If message has no content, message is returned to user.</returns>
        public override async Task<BotMessage> HandleMessage(BotMessage message)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!message.HasContent())
            {
                message.Response = "I didn't understand that :/";
                message.IsHandled = true;
            }

            return message;
        }
    }
}