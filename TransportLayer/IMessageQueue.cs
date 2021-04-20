using Models;
using System.Collections.Generic;

namespace TransportLayer
{
    public interface IMessageQueue
    {
        void Publish(Message message);
        void Remove(Message message);
        IEnumerable<Message> GetMessages();
    }



}
