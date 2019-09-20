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
        private readonly ILogger<SensorHub> _logger;

        public SensorHub(SensorCollection sensorCollection, ILogger<SensorHub> logger)
        {
            _sensorCollection = sensorCollection;
            _logger = logger;
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
            await foreach (var measurement in sensorData)
            {
                _logger.LogDebug("Received sensor data from {sensorName}: {measurement}", sensorName, measurement);
                _sensorCollection.PublishSensorData(sensorName, measurement);
            }
        }
    }
}
