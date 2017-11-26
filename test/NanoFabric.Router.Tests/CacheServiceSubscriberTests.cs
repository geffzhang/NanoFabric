using NanoFabric.Core;
using NanoFabric.Router.Cache;
using NanoFabric.Router.Cache.Internal;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NanoFabric.Router.Tests
{
    public class CacheServiceSubscriberTests
    {
        [Fact]
        public async Task Endpoints_PopulatesCacheImmediately()
        {
            var fixture = new CacheServiceSubscriberFixture();
            fixture.ServiceSubscriber.Endpoints().Returns(Task.FromResult(new List<RegistryInformation>()));

            var subscriber = fixture.CreateSut();
            await subscriber.Endpoints();

            fixture.Cache.Received(1).Set(Arg.Any<object>(), Arg.Any<List<RegistryInformation>>());
            fixture.Cache.Received(1).Get<List<RegistryInformation>>(Arg.Any<string>());
        }

        [Fact]
        public async Task StartSubscription_InitialCacheSetThrowsException_ExceptionBubblesUp()
        {
            var fixture = new CacheServiceSubscriberFixture();
            fixture.ServiceSubscriber.Endpoints().Returns(Task.FromResult(new List<RegistryInformation>()));

            var expectedException = new Exception();

            fixture.Cache.Set(Arg.Any<object>(), Arg.Any<List<RegistryInformation>>()).Throws(expectedException);

            var subscriber = fixture.CreateSut();

            Exception actualException = null;
            try
            {
                await subscriber.StartSubscription();

            }
            catch (Exception ex)
            {
                actualException = ex;
            }

            Assert.Same(expectedException, actualException);
        }

        [Fact]
        public async Task StartSubscription_InitialServiceCallThrowsException_NothingSetInCacheAndExceptionBubblesUp()
        {
            var fixture = new CacheServiceSubscriberFixture();
            var expectedException = new Exception();
            fixture.ServiceSubscriber.Endpoints(Arg.Any<CancellationToken>()).Throws(expectedException);

            var subscriber = fixture.CreateSut();

            Exception actualException = null;
            try
            {
                await subscriber.StartSubscription();

            }
            catch (Exception ex)
            {
                actualException = ex;
            }

            Assert.Same(expectedException, actualException);

            fixture.Cache.DidNotReceive().Set(Arg.Any<object>(), Arg.Any<List<RegistryInformation>>());
        }

        [Fact]
        public async Task Endpoints_CalledWhenDisposed_ThrowsDisposedException()
        {
            var fixture = new CacheServiceSubscriberFixture();
            var subscriber = fixture.CreateSut();
            subscriber.Dispose();

            ObjectDisposedException actualException = null;
            try
            {
                await subscriber.Endpoints();
            }
            catch (ObjectDisposedException ex)
            {
                actualException = ex;
            }
            Assert.IsType<ObjectDisposedException>(actualException);
        }

        [Fact]
        public void Dispose_DisposedTwice_DoesNotRemoveFromCacheTwice()
        {
            var fixture = new CacheServiceSubscriberFixture();
            var subscriber = fixture.CreateSut();
            subscriber.Dispose();
            subscriber.Dispose();

            fixture.Cache.Received(1).Remove(Arg.Any<string>());
        }

        [Fact]
        public async Task SubscriptionLoop_ReceivesChangedEndpoints_UpdatesCacheAndFiresEvent()
        {
            var result1 = new List<RegistryInformation>
                {
                    new RegistryInformation
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 123
                    }
                };
            var result2 = new List<RegistryInformation>
                {
                    new RegistryInformation
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 321
                    }
                };

            var fixture = new CacheServiceSubscriberFixture();
            fixture.ServiceSubscriber.Endpoints(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(result1), Task.FromResult(result2));

            var eventWasCalled = false;
            using (var subscriber = fixture.CreateSut())
            {
                subscriber.EndpointsChanged += (sender, args) => eventWasCalled = true;

                await subscriber.Endpoints();
                Thread.Sleep(1000);
                Received.InOrder(() =>
                {
                    fixture.Cache.Set(Arg.Any<string>(), result1);
                    fixture.Cache.Set(Arg.Any<string>(), result2);
                });

                fixture.Cache.Received(2).Set(Arg.Any<string>(), Arg.Any<List<RegistryInformation>>());
                Assert.True(eventWasCalled);
            }
        }

        [Fact]
        public async Task SubscriptionLoop_ReceivesSameEndpoints_DoesNotUpdateCacheOrFireEvent()
        {
            var result = new List<RegistryInformation>
                {
                    new RegistryInformation
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 123
                    }
                };

            var fixture = new CacheServiceSubscriberFixture();
            fixture.ServiceSubscriber.Endpoints()
                .Returns(Task.FromResult(result));

            var eventWasCalled = false;
            using (var subscriber = fixture.CreateSut())
            {
                subscriber.EndpointsChanged += (sender, args) => eventWasCalled = true;

                await subscriber.Endpoints();
                Thread.Sleep(1000);
                fixture.Cache.Received(1).Set(Arg.Any<string>(), Arg.Any<List<RegistryInformation>>());
                Assert.False(eventWasCalled);
            }
        }

        [Fact]
        public async Task SubscriptionLoop_ReceivesDifferentCountOfEndpoints_UpdatesCacheAndFiresEvent()
        {
            var result1 = new List<RegistryInformation>
                {
                    new RegistryInformation
                    {
                         Address = Guid.NewGuid().ToString(),
                        Port = 123
                    }
                };

            var result2 = new List<RegistryInformation>
                {
                    new RegistryInformation
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 789
                    },
                    new RegistryInformation
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 456
                    }
                };

            var fixture = new CacheServiceSubscriberFixture();
            fixture.ServiceSubscriber.Endpoints(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(result1), Task.FromResult(result2));

            var eventWasCalled = false;
            using (var subscriber = fixture.CreateSut())
            {
                subscriber.EndpointsChanged += (sender, args) => eventWasCalled = true;

                await subscriber.StartSubscription();
                await subscriber.Endpoints();
                Thread.Sleep(1000);
                Received.InOrder(() =>
                {
                    fixture.Cache.Set(Arg.Any<string>(), result1);
                    fixture.Cache.Set(Arg.Any<string>(), result2);
                });

                fixture.Cache.Received(2).Set(Arg.Any<string>(), Arg.Any<List<RegistryInformation>>());
                Assert.True(eventWasCalled);
            }
        }

        [Fact]
        public async Task SubscriptionLoop_WithoutUpdateEvent_UpdatesCacheWithoutFiringEventAndDying()
        {
            var result1 = new List<RegistryInformation>
                {
                    new RegistryInformation
                    {
                         Address = Guid.NewGuid().ToString(),
                        Port = 123
                    }
                };

            var result2 = new List<RegistryInformation>
                {
                    new RegistryInformation
                    {
                         Address = Guid.NewGuid().ToString(),
                        Port = 789
                    },
                    new RegistryInformation
                    {
                         Address = Guid.NewGuid().ToString(),
                        Port = 456
                    }
                };

            var fixture = new CacheServiceSubscriberFixture();
            fixture.ServiceSubscriber.Endpoints(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(result1), Task.FromResult(result2));

            using (var subscriber = fixture.CreateSut())
            {
                Thread.Sleep(1000);
                await subscriber.Endpoints();
                Received.InOrder(() =>
                {
                    fixture.Cache.Set(Arg.Any<string>(), result1);
                    fixture.Cache.Set(Arg.Any<string>(), result2);
                });

                fixture.Cache.Received(2).Set(Arg.Any<string>(), Arg.Any<List<RegistryInformation>>());
            }
        }
    }

    public class CacheServiceSubscriberFixture
    {
        public IServiceSubscriber ServiceSubscriber { get; set; }

        public ICacheClient Cache { get; set; }

        public CacheServiceSubscriberFixture()
        {
            ServiceSubscriber = Substitute.For<IServiceSubscriber>();
            Cache = Substitute.For<ICacheClient>();
        }

        public IPollingServiceSubscriber CreateSut()
        {
            return new CacheServiceSubscriber(ServiceSubscriber, Cache);
        }
    }
}
