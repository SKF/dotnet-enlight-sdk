using System;
using System.IO;
using SKF.Enlight.Clients;

namespace SKF.Enlight.Examples
{
    public class DeepPing
    {
        public static void Main(string[] args)
        {
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
                var iotClient = new IoT(Path.Combine(basePath, "iot_ca.crt"),
                    Path.Combine(basePath, "iot_client.crt"),
                    Path.Combine(basePath, "iot_client.key"),
                    host, int.Parse(port));

                Console.WriteLine($"Sending ping to: {host}:{port}");
                Console.WriteLine("Reply: " + iotClient.DeepPing());

                iotClient.Close();
            }

            Console.WriteLine("<press any key to exit>");
            Console.ReadKey();
        }
    }
}
