using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR30SensorClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hubConnectionBuilder = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/sensors");

            await using var hubConnection = hubConnectionBuilder.Build();

            await hubConnection.StartAsync();

            Console.WriteLine("Started sensor {0}.", args[0]);

            var cancellationTokenSource = new CancellationTokenSource();

            await hubConnection.SendAsync("PublishSensorData", args[0], GenerateSensorData(cancellationTokenSource.Token));

            Console.ReadLine();

            cancellationTokenSource.Cancel();

            Console.ReadLine();
        }

        static async IAsyncEnumerable<double> GenerateSensorData([EnumeratorCancellation] CancellationToken cancellationToken)
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
