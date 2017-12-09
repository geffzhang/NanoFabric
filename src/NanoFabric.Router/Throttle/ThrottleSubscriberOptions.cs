using System;

namespace NanoFabric.Router.Throttle
{
    public class ThrottleSubscriberOptions
    {
        public static readonly ThrottleSubscriberOptions Default = new ThrottleSubscriberOptions();

        public int MaxUpdatesPerPeriod { get; set; } = 5;

        public TimeSpan MaxUpdatesPeriod { get; set; } = TimeSpan.FromSeconds(10);
    }
}