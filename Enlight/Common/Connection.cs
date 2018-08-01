using System;
using System.IO;
using System.Linq;
using Grpc.Core;

namespace SKF.Enlight.Common
{
    public class Connection
    {
        private string _cacert;
        private string _cert;
        private string _key;

        private KeyCertificatePair _keypair;
        private SslCredentials _creadentials;

        public Channel Channel { get; private set; }


        public Connection(string cacert, string cert, string key)
        {
            _cacert = File.ReadAllText(cacert);
            _cert = File.ReadAllText(cert);
            _key = File.ReadAllText(key);
        }

        public void Open(string host, int port)
        {
            if (Channel != null)
            {
                throw new Exception($"gRPC channel is already open, unable to reopen towards {host}");
            }

            _keypair = new KeyCertificatePair(_cert, _key);
            _creadentials = new SslCredentials(_cacert, _keypair);

            Channel = new Channel(host, port, _creadentials);
        }

        public void Close()
        {
            if (Channel != null)
            {
                Channel.ShutdownAsync().Wait();
                Channel = null;
            }
        }
    }
}
