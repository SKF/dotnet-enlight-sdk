using System;
using System.IO;
using SKF.Enlight.API.IoT;

namespace SKF.Enlight.Examples
{
    public class IoT
    {
        public static void Main(string[] args)
        {
            // Setup, fetch certs etc.
            const string basePath = "../../../certs";
            const string envHost = "ENLIGHT_GRPC_IOT_HOST";
            const string envPort = "ENLIGHT_GRPC_IOT_PORT";

            var host = Environment.GetEnvironmentVariable(envHost);
            var port = Environment.GetEnvironmentVariable(envPort);

            if (host == null || host.Length <= 0)
            {
                Console.WriteLine($"Missing host environment variable: {envHost}");
                Environment.ExitCode = 1;
            }
            else if (port == null || port.Length <= 0)
            {
                Console.WriteLine($"Missing port environment variable: {envPort}");
                Environment.ExitCode = 1;
            }
            else
            {
                // Certs etc are available, create a new client
                var client = new Clients.IoT(Path.Combine(basePath, "iot_ca.crt"),
                    Path.Combine(basePath, "iot_client.crt"),
                    Path.Combine(basePath, "iot_client.key"),
                    host, int.Parse(port));

                // Test the DeepPing method
                Console.WriteLine($"Sending ping to: {host}:{port}");
                Console.WriteLine("Reply: " + client.DeepPing());

                // Test the IngestData method
                Console.WriteLine("Sending data to IngestNodeData");

                // Test with DataPoint input
                client.IngestNodeData(
                    Guid.NewGuid(), // Use a specific node id instead
                    new NodeData
                    {
                        ContentType = NodeDataContentType.DataPoint,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        DataPoint = new DataPoint
                        {
                            Coordinate = new Coordinate
                            {
                                X = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                                Y = 75,
                            },
                            XUnit = "ms",
                            YUnit = "coffe",
                        }
                    }
                );

                // Test with TimeSeries input
                var timeseries = new TimeSeries
                {
                    XUnit = "ms",
                    YUnit = "gE",
                };
                timeseries.Coordinates.Add(new Coordinate
                {
                    X = 1,
                    Y = 2,
                });
                timeseries.Coordinates.Add(new Coordinate
                {
                    X = 2,
                    Y = 3,
                });

                client.IngestNodeData(
                    Guid.NewGuid(), // Use a specific node id instead
                    new NodeData
                    {
                        ContentType = NodeDataContentType.TimeSeries,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        TimeSeries = timeseries,
                    }
                );

                // All done, close cleanly.
                client.Close();
            }

            Console.WriteLine("<press any key to exit>");
            Console.ReadKey();
        }
    }
}
