using System;

namespace Models
{
        public class Message
    {
        public int MessageId { get; set; }
        public DateTime Sent { get; set; }
        public DateTime Received {get; set; }
        public string DeliveryTag {get; set; }
    }
}
