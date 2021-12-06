using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class UDPHandler: INetworkHandler
    {
        private int listenPort;
        private int sendPort;
        
        public bool IsListen { get; set; }

        public UDPHandler(int listenPort, int sendPort)
        {
            this.listenPort = listenPort;
            this.sendPort = sendPort;
        }

        //public UdpClient SetConnectionParams(IPAddress Ip, int port)
        //{
        //    UdpClient client = new UdpClient();
        //    client.Connect(Ip, port);   
        //    return client;
        //}

        public IEnumerable<byte[]> Listen()
        {
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, listenPort);
            using (UdpClient listener = new UdpClient(listenPort))
            {
                IsListen = true;
                while (IsListen)
                {
                    byte[] bytes = listener.Receive(ref broadcastEndPoint);
                    yield return bytes;
                }
            }
        }


        public byte[] Send(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
