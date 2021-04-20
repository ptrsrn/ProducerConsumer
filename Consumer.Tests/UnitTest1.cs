using System;
using Xunit;
using Consumer;
using Models;

namespace Consumer.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            SecondsEvenShouldStoreStrategy strategy = new SecondsEvenShouldStoreStrategy();
            Message evenMessage = new Message(){Sent = new DateTime(2021, 1, 1, 0, 0, 2)};
            Message oddMessage = new Message(){Sent = new DateTime(2021, 1, 1, 0, 0, 3)};
            Assert.True(strategy.ShouldStore(evenMessage), "EVEN should be stored");
            Assert.False(strategy.ShouldStore(oddMessage), "ODD should not be stored");
        }
    }
}
