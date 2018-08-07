using SKF.Enlight.Common;
using HierarchyAPI = SKF.Enlight.API.Hierarchy;

namespace SKF.Enlight.Clients
{
    public class Hierarchy : Client
    {
        private HierarchyAPI.Hierarchy.HierarchyClient _client;

        public Hierarchy(string cacert, string cert, string key, string host, int port) : base(cacert, cert, key, host, port)
        {
            _client = new HierarchyAPI.Hierarchy.HierarchyClient(_conn.Channel);
        }
    }
}
