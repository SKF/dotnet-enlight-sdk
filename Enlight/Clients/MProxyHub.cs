using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SKF.Enlight.Common;
using MProxyHubAPI = SKF.Enlight.API.MProxyHub;
using DSKFFile = SKF.Enlight.API.MProxyHub.AvailableDSKFStreamOutput;

namespace SKF.Enlight.Clients
{
    public class MProxyHub : Client
    {
        private MProxyHubAPI.MicrologProxyHub.MicrologProxyHubClient _client;

        public MProxyHub(string cacert, string cert, string key, string host, int port) : base(cacert, cert, key, host, port)
        {
            _client = new MProxyHubAPI.MicrologProxyHub.MicrologProxyHubClient(_conn.Channel);
        }

        public string DeepPing()
        {
            return _client.DeepPing(new MProxyHubAPI.Void(), new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10))).ToString();
        }

        public void SetTaskStatus(string taskId, string userId, MProxyHubAPI.TaskStatus taskStatus)
        {
            MProxyHubAPI.SetTaskStatusInput request = new MProxyHubAPI.SetTaskStatusInput
            {
                TaskId = taskId,
                UserId = userId,
                Status = taskStatus,
            };
            _client.SetTaskStatus(request, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));
        }

        public Grpc.Core.IAsyncStreamReader<DSKFFile> AvailableDSKFFile()
        {
            using (var response = _client.AvailableDSKFStream(new MProxyHubAPI.AvailableDSKFStreamInput(), new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10))))
            {
                return response.ResponseStream;
            }
        }
    }
}
