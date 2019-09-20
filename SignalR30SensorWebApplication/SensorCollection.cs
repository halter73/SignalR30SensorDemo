using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;

namespace SignalR30SensorWebApplication
{
    public class SensorCollection
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<Channel<double>>> _sensors = new ConcurrentDictionary<string, ConcurrentQueue<Channel<double>>>();

        public IEnumerable<string> GetSensorNames()
        {
            return _sensors.Keys;
        }

        public void PublishSensorData(string sensorName, double measurement)
        {
            var subscriberQueue = _sensors.GetOrAdd(sensorName, _ => new ConcurrentQueue<Channel<double>>());

            foreach (var subscriber in subscriberQueue)
            {
                Trace.Assert(subscriber.Writer.TryWrite(measurement));
            }
        }

        public IAsyncEnumerable<double> GetSensorData(string sensorName, CancellationToken cancellationToken = default)
        {
            var subscriberQueue = _sensors.GetOrAdd(sensorName, _ => new ConcurrentQueue<Channel<double>>());

            var channel = Channel.CreateBounded<double>(new BoundedChannelOptions(10)
            {
                FullMode = BoundedChannelFullMode.DropOldest
            });

            subscriberQueue.Enqueue(channel);

            return channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}
