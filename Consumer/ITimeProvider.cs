using System;

namespace Consumer
{
    public interface ITimeProvider
    {
        DateTime UtcNow();
    }


}

