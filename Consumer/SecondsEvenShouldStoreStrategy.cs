using System;
using Models;

namespace Consumer
{
    public class SecondsEvenShouldStoreStrategy : IShouldStoreStrategy
    {
        public Boolean ShouldStore(Message message)
        {
            return message.Sent.Second % 2 == 0;
        }
    }


}

