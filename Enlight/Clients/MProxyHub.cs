using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SKF.Enlight.Common;
using MProxyHubAPI = SKF.Enlight.API.MProxyHub;
using DSKFFile = SKF.Enlight.API.MProxyHub.AvailableDSKFStreamOutput;

namespace SKF.Enlight.Clients
{
    public class MProxyHub
    {
        private Connection _conn;
        private MProxyHubAPI.MicrologProxyHub.MicrologProxyHubClient _client;

        public MProxyHub(string cacert, string cert, string key, string host, int port)
        {
            _conn = new Connection(cacert, cert, key);
            _conn.Open(host, port);
            _client = new MProxyHubAPI.MicrologProxyHub.MicrologProxyHubClient(_conn.Channel);
        }

        public void Close()
        {
            _conn.Close();
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

        public List<DSKFFile> AvailableDSKFFiles()
        {
            Task<List<DSKFFile>> task = AvailableDSKFFilesAsync();
            task.Wait();
            return task.Result;
        }

        async public Task<List<DSKFFile>> AvailableDSKFFilesAsync()
        {
            var files = new List<DSKFFile>();

            using (var response = _client.AvailableDSKFStream(new MProxyHubAPI.AvailableDSKFStreamInput(), new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10))))
            {
                while (await response.ResponseStream.MoveNext(new CancellationToken()))
                {
                    var file = new DSKFFile{
                        DskfFile = response.ResponseStream.Current.DskfFile,
                        TaskId = response.ResponseStream.Current.TaskId,
                    };
                    files.Add(file);
                }
            }
            return files;
        }
    }
}
