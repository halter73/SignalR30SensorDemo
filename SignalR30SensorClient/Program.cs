using System;
using System.Collections.Generic;
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

            await hubConnection.SendAsync("PublishSensorData", args[0], GenerateSensorData());

            Console.ReadLine();
        }

        static async IAsyncEnumerable<double> GenerateSensorData()
        {
            var rng = new Random();

            while (true)
            {
                yield return rng.NextDouble() * 10;
                await Task.Delay(1000);
            }
        }
    }
}
