using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PaperShip
{
    public class Server
    {
		string host;
		int port;

        public string Host
        {
            set { host = value; }
            get { return Host; }
        }

        public int Port
        {
            set { port = value; }
            get { return port; }
        }

		TcpClient client;
		NetworkStream network;

        public TcpClient Client
        {
            get
            {
                if (client != null)
                    return client;

                throw new SocketException();
            }
        }

        public NetworkStream Network
        {
            get
            {
                if (network != null)
                    return network;
                if (client != null && network == null)
                    return client.GetStream();

                throw new Exception();
            }
        }

        static Server server;

        public static Server GetServer()
        {
            if (server == null)
                server = new Server();
            return server;
        }

        protected Server()
		{
            host = "172.22.32.152";
            port = 41500;
		}

        public async Task ConnectAsync()
		{
			if (client != null)
				return;

			client = new TcpClient();

            try
			{
				await client.ConnectAsync(host, port);
				network = client.GetStream();
			} catch (SocketException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

        public void Disconnected()
		{
			client.Close();
			network.Close();
			Environment.Exit(0);
		}
    }
}
