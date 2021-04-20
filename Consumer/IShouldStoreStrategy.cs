using System;
using Models;

namespace Consumer
{
    interface IShouldStoreStrategy
    {
        Boolean ShouldStore(Message message);
    }


}

