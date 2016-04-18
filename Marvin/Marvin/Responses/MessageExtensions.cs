using System;
using System.Collections.Generic;
using Microsoft.Bot.Connector;

namespace Marvin.Responses
{
    /// <summary>
    /// The message extensions.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Responds to the unknown intent with funny message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Returns random message from the sequence.</returns>
        public static Message RespondUnknownIntent(this Message message)
        {
            var responses = new List<string>()
            {
                $"I'm sorry {message.From.Name}, I'm afraid I can't do that.",
                "Ah well, there's always hope.",
                "Did you say something?"
            };

            return message.CreateReplyMessage(responses[new Random().Next(0, responses.Count)], "en");
        }

        /// <summary>
        /// The Pulp Fiction reference respons.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Returns next message in a row from "Say what again." scene. In the end, responds with 500 internal server error.</returns>
        /// <remarks>
        /// Uses WhatCounter property to save counter.
        /// Last message will reset counter to zero.
        /// </remarks>
        public static Message RespondWhat(this Message message)
        {
            var responses = new List<string>()
            {
                "Say what again.",
                "English motherfucker do you speak it!?",
                "SAY WHAT again! And I dare you, I double dare you motherfucker! Say what one more time.",
                "500 Internal Server Error"
            };

            // Construct an message
            var counter = message.GetBotPerUserInConversationData<int>("WhatCounter");
            var response = message.CreateReplyMessage(responses[counter], "en");

            // Increment counter
            response.SetBotPerUserInConversationData("WhatCounter", (counter + 1) % responses.Count);

            return response;
        }
    }
}