using System;
using Models;

namespace Consumer
{
    interface IMessageStrategy
    {
        Boolean ShouldProcess(Message message);
        Boolean ShouldStore(Message message);
    }


}

