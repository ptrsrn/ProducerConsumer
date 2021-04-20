using System;

namespace Consumer
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

    }


}

