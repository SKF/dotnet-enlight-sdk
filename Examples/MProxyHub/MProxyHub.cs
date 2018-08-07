using System;
using System.IO;
using SKF.Enlight.API.MProxyHub;

namespace SKF.Enlight.Examples
{
    public class MProxyHub
    {
        static void Main(string[] args)
        {
            // Setup, fetch certs etc.
            const string basePath = "../../../certs";
            const string envHost = "ENLIGHT_GRPC_MPROXYHUB_HOST";
            const string envPort = "ENLIGHT_GRPC_MPROXYHUB_PORT";

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
                var client = new Clients.MProxyHub(Path.Combine(basePath, "mproxyhub_ca.crt"),
                    Path.Combine(basePath, "mproxyhub_client.crt"),
                    Path.Combine(basePath, "mproxyhub_client.key"),
                    host, int.Parse(port));

                // Test the DeepPing method
                Console.WriteLine($"Sending ping to: {host}:{port}");
                Console.WriteLine("Reply: " + client.DeepPing());

                // All done, close cleanly.
                client.Close();
            }

            Console.WriteLine("<press any key to exit>");
            Console.ReadKey();
        }
    }
}
