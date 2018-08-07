using SKF.Enlight.Common;
using HierarchyAPI = SKF.Enlight.API.Hierarchy;

namespace SKF.Enlight.Clients
{
    public class Hierarchy
    {
        private Connection _conn;
        private HierarchyAPI.Hierarchy.HierarchyClient _client;

        public Hierarchy()
        {
            _conn = new Connection("dummy_cacert_path", "dummy_cert_path", "dummy_key_path");
            _conn.Open("a host", 443);
            _client = new HierarchyAPI.Hierarchy.HierarchyClient(_conn.Channel);
        }

        public void Close()
        {
            _conn.Close();
        }
    }
}
