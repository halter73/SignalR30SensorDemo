using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalR30SensorWebApplication
{
    public class SensorHub : Hub
    {
        public async IAsyncEnumerable<double> GetSensorData(string sensorName, CancellationToken cancellationToken)
        {
            var rng = new Random();
            double currentTemp = -18;
            double tempVelocity = 0.5;

            while (!cancellationToken.IsCancellationRequested)
            {
                tempVelocity += (rng.NextDouble() - .5) / 4;
                tempVelocity = Math.Clamp(tempVelocity, -2, 2);

                currentTemp += tempVelocity;

                if (currentTemp < -30)
                {
                    tempVelocity = .5; 
                }
                else if(currentTemp > 0)
                {
                    tempVelocity = -.5;
                }

                yield return currentTemp;

                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
