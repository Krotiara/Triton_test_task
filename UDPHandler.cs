using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Triton_test_task
{
    public class UDPHandler: INetworkHandler
    {
        private int listenPort;
        private int sendPort;

        public UDPHandler(int listenPort, int sendPort)
        {
            this.listenPort = listenPort;
            this.sendPort = sendPort;
        }

        public void Listen(Action<byte[]> getDataAction)
        {
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, listenPort);
            using(UdpClient listener = new UdpClient(listenPort))
            {
                try
                {
                    byte[] bytes = listener.Receive(ref broadcastEndPoint);
                    getDataAction(bytes);
                }
                catch(SocketException e)
                {
                    Console.WriteLine(e); //Убрать
                }
            }
        }


        public void Send(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
