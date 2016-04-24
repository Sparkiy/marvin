using System.Threading.Tasks;

namespace Marvin.Pipeline
{
    public abstract class BotMessagePipelineTask
    {
        public virtual async Task<BotMessage> ResultMessage(BotMessage message)
        {
            return message;
        }

        public virtual async Task<BotMessage> HandleMessage(BotMessage message)
        {
            return message;
        }
    }
}