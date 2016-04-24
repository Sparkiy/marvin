using System.Collections;
using System.Linq;
using System.Reflection;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Marvin.WindowsAnalytics
{
    /// <summary>
    /// The message extensions.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Translates the message to bot message.
        /// </summary>
        /// <param name="message">The message to translate.</param>
        /// <returns>Returns new instance of <see cref="BotMessage"/> with properties populated from common serializable properties with specified message.</returns>
        public static BotMessage AsBotMessage(this Message message)
        {
            return BotMessage.Populate<Message, BotMessage>(message);
        }
    }

    /// <summary>
    /// The bot message.
    /// </summary>
    public class BotMessage : Message
    {
        /// <summary>
        /// The handled message flag.
        /// </summary>
        [JsonProperty(PropertyName = "IsHandled")]
        public bool IsHandled { get; set; }

        /// <summary>
        /// The debug message flag.
        /// </summary>
        [JsonProperty(PropertyName = "IsDebug")]
        public bool IsDebug { get; set; }

        /// <summary>
        /// The debug parameter.
        /// </summary>
        [JsonProperty(PropertyName = "DebugParam")]
        public string DebugParam { get; set; }

        /// <summary>
        /// The response text.
        /// </summary>
        [JsonProperty(PropertyName = "Response")]
        public string Response { get; set; }


        /// <summary>
        /// Populates the new instance of message with data from specified message.
        /// </summary>
        /// <typeparam name="TSource">The source message type.</typeparam>
        /// <typeparam name="TTarget">The target message type.</typeparam>
        /// <param name="message">The message from which the data will be pulled to populate new instance of target object type.</param>
        /// <returns>Returns new instance of specified target message type that has it's common serializable properties, with specified source message instance, populated from the source message. </returns>
        public static TTarget Populate<TSource, TTarget>(TSource message)
            where TTarget : BotMessage, new()
            where TSource : Message
        {
            // Retrieve all serializable properties from target message
            var serializableTargetProperties = typeof(TTarget)
                .GetProperties()
                .Where(property => property.GetCustomAttributes<JsonPropertyAttribute>().Any());

            // Retrieve all serializable properties from source message
            var serializableSourceProperties = typeof(TSource)
                .GetProperties()
                .Where(property => property.GetCustomAttributes<JsonPropertyAttribute>().Any());

            // Take only properties that are available in source and targer
            var commonProperties = serializableSourceProperties.Where(ssp => serializableTargetProperties.Any(stp => stp.Name.Equals(ssp.Name)));

            // Map common properties to new instance of target object type
            var result = new TTarget();
            foreach (var commonProperty in commonProperties)
                commonProperty.SetValue(result, commonProperty.GetValue(message));

            return result;
        }
    }
}