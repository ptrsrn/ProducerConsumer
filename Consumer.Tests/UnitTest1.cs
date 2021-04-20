using System;
using Xunit;
using Consumer;
using Models;

namespace Consumer.Tests
{
    /// should probably be made into a mock
    public class TestTimeProvider : ITimeProvider
    {
        private readonly DateTime utc;

        public TestTimeProvider(DateTime utc)
        {
            this.utc = utc;
        }

        public DateTime UtcNow()
        {
            return utc;
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void TestShouldStore()
        {
            DateTime now = new DateTime(2021, 4, 21, 10, 0, 0);
            MessageStrategy strategy = new MessageStrategy(new TestTimeProvider(now));
            
            Message evenMessage = new Message(){Sent = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0)};
            Message oddMessage = new Message(){Sent = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 1)};
            Assert.True(strategy.ShouldStore(evenMessage), "EVEN should be stored");
            Assert.False(strategy.ShouldStore(oddMessage), "ODD should not be stored");
        }

        [Fact]
        public void TestShoudProcess()
        {
            DateTime now = new DateTime(2021, 4, 21, 10, 0, 0);
            MessageStrategy strategy = new MessageStrategy(new TestTimeProvider(now));
  
            Message notTooOld = new Message(){Sent =  now.AddSeconds(-59)};
            Assert.True(strategy.ShouldProcess(notTooOld), "not too old should be processed");
            
            Message tooOld = new Message(){Sent = now.AddMinutes(-1)};
            Assert.False(strategy.ShouldProcess(tooOld), "too old should not be processed");

        }
    }
}
