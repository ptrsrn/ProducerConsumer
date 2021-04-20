using System;
using Models;

namespace Consumer
{
    public class MessageStrategy : IMessageStrategy
    {
        private readonly ITimeProvider timeProvider;

        public MessageStrategy(ITimeProvider timeProvider)
        {
            this.timeProvider = timeProvider;
        }
        public Boolean ShouldProcess(Message message)
        {
            DateTime now = this.timeProvider.UtcNow();
            Boolean tooOld =  now.AddMinutes(-1) >= message.Sent;
            return !tooOld;
        }

        public Boolean ShouldStore(Message message) {
            DateTime now = this.timeProvider.UtcNow();
            Boolean isSecondEven = message.Sent.Second % 2 == 0;
            return isSecondEven;
        }
    }

}

