using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SignalR30SensorWebApplication
{
    public class SensorHub : Hub
    {
        private readonly SensorCollection _sensorCollection;

        public SensorHub(SensorCollection sensorCollection)
        {
            _sensorCollection = sensorCollection;
        }

        public IEnumerable<string> GetSensorNames()
        {
            return _sensorCollection.GetSensorNames();
        }

        public IAsyncEnumerable<double> GetSensorData(string sensorName, CancellationToken cancellationToken)
        {
            return _sensorCollection.GetSensorData(sensorName, cancellationToken);
        }

        public async Task PublishSensorData(string sensorName, IAsyncEnumerable<double> sensorData)
        {
            try
            {
                await foreach (var measurement in sensorData)
                {
                    _sensorCollection.PublishSensorData(sensorName, measurement);
                }
            }
            finally
            {
                _sensorCollection.DisconnectSensor(sensorName);
            }
        }
    }
}
