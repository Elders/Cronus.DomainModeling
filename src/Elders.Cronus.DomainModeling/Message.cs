using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Elders.Cronus.DomainModeling
{
    [DataContract(Name = "e965dc00-dcc0-43a9-a515-39f7d9d1d315")]
    public sealed class Message : IMessage
    {
        Message()
        {
            Headers = new Dictionary<string, string>();
        }

        public Message(object payload, Dictionary<string, string> messageHeaders = null)
        {
            this.Payload = payload;
            this.Headers = messageHeaders ?? new Dictionary<string, string>();
        }

        public Message(Message message)
        {
            this.Payload = message.Payload;
            this.Headers = message.Headers;
        }

        [DataMember(Order = 1)]
        public Dictionary<string, string> Headers { get; private set; }

        [DataMember(Order = 2)]
        public object Payload { get; private set; }

        public override string ToString()
        {
            return Payload.ToString();
        }
    }
}
