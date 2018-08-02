using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKF.Enlight.Common;
using SKF.Enlight.ProtocolBuffers;

namespace SKF.Enlight.Clients
{
    public class IoT
    {
        private Connection _conn;
        private ProtocolBuffers.IoT.IoTClient _client;

        public IoT(string cacert, string cert, string key, string host, int port)
        {
            _conn = new Connection(cacert, cert, key);
            _conn.Open(host, port);
            _client = new ProtocolBuffers.IoT.IoTClient(_conn.Channel);
        }

        public void Close()
        {
            _conn.Close();
        }

        public void IngestNodeData(Guid nodeid, NodeData data)
        {
            IngestNodeDataInput request = new IngestNodeDataInput
            {
                NodeId = nodeid.ToString(),
                NodeData = data,
            };
            _client.IngestNodeData(request);
        }

        public string DeepPing()
        {
            return _client.DeepPing(new PrimitiveVoid(), new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10))).ToString();
        }
    }

}

