using System;
using SKF.Enlight.Common;

namespace SKF.Enlight.Clients
{
    public abstract class Client
    {
        protected Connection _conn;

        public Client(string cacert, string cert, string key, string host, int port)
        {
            _conn = new Connection(cacert, cert, key);
            _conn.Open(host, port);
        }

        public void Close()
        {
            _conn.Close();
        }
    }
}
