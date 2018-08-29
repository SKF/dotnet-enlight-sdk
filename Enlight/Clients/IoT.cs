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

        public void IngestNodeData(Guid nodeId, IoTAPI.NodeData data)
        {
            var request = new IoTAPI.IngestNodeDataInput
            {
                NodeId = nodeId.ToString(),
                NodeData = data,
            };
            _client.IngestNodeData(request, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));
        }

        public string DeepPing()
        {
            return _client.DeepPing(new IoTAPI.PrimitiveVoid(), new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10))).ToString();
        }

        public void SetTaskStatus(Guid taskID, Guid userID, IoTAPI.TaskStatus taskStatus)
        {
            IoTAPI.SetTaskStatusInput request = new IoTAPI.SetTaskStatusInput
            {
                TaskId = taskID.ToString(),
                UserId = userID.ToString(),
                Status = taskStatus,
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            _client.SetTaskStatus(request, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));
        }

        public List<IoTAPI.TaskDescription> GetTasksByStatus(Guid hierarchyId, IoTAPI.TaskStatus taskStatus)
        {
            var list = new List<IoTAPI.TaskDescription>();

            var request = new IoTAPI.GetTasksByStatusInput
            {
                HierarchyId = hierarchyId.ToString(),
                Status = taskStatus,
            };
            var reply = _client.GetTasksByStatus(request, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));

            foreach (var tasklist in reply.TaskList)
            {
                list.Add(tasklist);
            }

            return list;
        }

        public IoTAPI.TaskDescription GetTaskByUUID(Guid taskId)
        {
            var request = new IoTAPI.GetTaskByUUIDInput
            {
                TaskId = taskId.ToString(),
            };
            var reply = _client.GetTaskByUUID(request, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));

            return reply.Task;
        }

        public IoTAPI.TaskDescription GetTaskByLongId(Int64 taskMicrologId)
        {
            var request = new IoTAPI.GetTaskByLongIdInput
            {
                TaskId = taskMicrologId,
            };
            var reply = _client.GetTaskByLongId(request, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));

            return reply.Task;
        }
    }
}
