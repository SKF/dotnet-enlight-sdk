using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKF.Enlight.Common;
using IoTAPI = SKF.Enlight.API.IoT;

namespace SKF.Enlight.Clients
{
    public class IoT
    {
        private Connection _conn;
        private IoTAPI.IoT.IoTClient _client;

        public IoT(string cacert, string cert, string key, string host, int port)
        {
            _conn = new Connection(cacert, cert, key);
            _conn.Open(host, port);
            _client = new IoTAPI.IoT.IoTClient(_conn.Channel);
        }

        public void Close()
        {
            _conn.Close();
        }

        public void IngestNodeData(Guid nodeid, IoTAPI.NodeData data)
        {
            IoTAPI.IngestNodeDataInput request = new IoTAPI.IngestNodeDataInput
            {
                NodeId = nodeid.ToString(),
                NodeData = data,
            };
            _client.IngestNodeData(request, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));
        }

        public string DeepPing()
        {
            return _client.DeepPing(new IoTAPI.PrimitiveVoid(), new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10))).ToString();
        }
    }

}

