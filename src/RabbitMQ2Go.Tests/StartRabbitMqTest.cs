using System;
using System.Diagnostics;
using NUnit.Framework;
using RabbitMQ2Go.Helpers;
using RabbitMQ2Go.SingletonHelpers;

namespace RabbitMQ2Go.Tests
{
    [TestFixture]
    public class StartRabbitMqTest
    {
        [Test]
        public void StartupTest()
        {
            using (var rabbitMq = new RabbitMqProcess())
            {
                rabbitMq.Run().Wait();
            }
        }
        [Test]
        public void StartupTimedoutTest()
        {
            var timeoutErrorHappened = false;

            using (var rabbitMq = new RabbitMqProcess())
            {
                //clear the variable where erlang is, this should make rabbitmq fail to load
                rabbitMq.BinaryPathHelper.ErlangRoot = "";
                try
                {
                    try
                    {
                        rabbitMq.Run().Wait();
                    }
                    catch (Exception error)
                    {
                        throw error.InnerException ?? error;
                    }
                }
                catch (TimeoutException)
                {
                    timeoutErrorHappened = true;
                }
            }

            Debug.Assert(timeoutErrorHappened, "The timout for the process never fired.");
        }
    }
}
