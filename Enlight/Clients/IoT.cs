using System;
using SKF.Enlight.Common;
using IoTAPI = SKF.Enlight.API.IoT;
using System.Collections.Generic;

namespace SKF.Enlight.Clients
{
    public class IoT : Client
    {
        private IoTAPI.IoT.IoTClient _client;

        public IoT(string cacert, string cert, string key, string host, int port) : base(cacert, cert, key, host, port)
        {
            _client = new IoTAPI.IoT.IoTClient(_conn.Channel);
        }

        public void IngestNodeData(Guid nodeid, IoTAPI.NodeData data)
        {
            var request = new IoTAPI.IngestNodeDataInput
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

        public List<IoTAPI.TaskDescription> GetTasksByStatus(string id, IoTAPI.TaskStatus status)
        {
            var list = new List<IoTAPI.TaskDescription>();

            var request = new IoTAPI.GetTasksByStatusInput
            {
                HierarchyId = id,
                Status = status,
            };
            var reply = _client.GetTasksByStatus(request, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));

            foreach (var tasklist in reply.TaskList)
            {
                list.Add(tasklist);
            }

            return list;
        }

        public IoTAPI.TaskDescription GetTaskByUUID(Guid id)
        {
            var request = new IoTAPI.GetTaskByUUIDInput
            {
                TaskId = id.ToString(),
            };
            var reply = _client.GetTaskByUUID(request, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));

            return reply.Task;
        }

        public IoTAPI.TaskDescription GetTaskByLongId(UInt64 id)
        {
            var request = new IoTAPI.GetTaskByLongIdInput
            {
                TaskId = id,
            };
            var reply = _client.GetTaskByLongId(request, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));

            return reply.Task;
        }
    }
}
