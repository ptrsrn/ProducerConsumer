using System;
using Models;

namespace Consumer
{
    class SecondsEvenShouldStoreStrategy : IShouldStoreStrategy
    {
        public Boolean ShouldStore(Message message)
        {
            return message.Sent.Second % 2 == 0;
        }
    }


}

