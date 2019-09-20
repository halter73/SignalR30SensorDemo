using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalR30SensorWebApplication
{
    public class SensorHub : Hub
    {
        private readonly SensorCollection _sensorCollection;

        public SensorHub(SensorCollection sensorCollection)
        {
            _sensorCollection = sensorCollection;
        }

        public IAsyncEnumerable<double> GetSensorData(string sensorName, CancellationToken cancellationToken)
        {
            return _sensorCollection.GetSensorData(sensorName, cancellationToken);
        }

        public async Task PublishSensorData(string sensorName, IAsyncEnumerable<double> sensorData)
        {
            await foreach (var measurement in sensorData)
            {
                Console.WriteLine("Received sensor data: {0}", measurement);
                _sensorCollection.PublishSensorData(sensorName, measurement);
            }
        }
    }
}
