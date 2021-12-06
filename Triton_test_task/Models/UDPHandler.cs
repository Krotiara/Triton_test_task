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
        public int ListenPort { get; }
        public int SendPort { get; }
        public IPEndPoint ListenEndPoint { get; }
        public IPEndPoint SendEndPoint { get; }

        private bool isListen;

        public UDPHandler(int listenPort, int sendPort)
        {
            this.ListenPort = listenPort;
            this.SendPort = sendPort;
            ListenEndPoint = new IPEndPoint(IPAddress.Broadcast, listenPort);
            SendEndPoint = new IPEndPoint(IPAddress.Broadcast, sendPort);
        }


        public IEnumerable<byte[]> Listen()
        {
            isListen = true;
            while (isListen)
            {
                yield return Receive();
            }
        }

        public byte[] Receive()
        {
            using (UdpClient listener = new UdpClient(ListenPort))
            {
                IPEndPoint listenEndPoint = ListenEndPoint;
                return listener.Receive(ref listenEndPoint);
            }
        }

        public void StopListen()
        {
            isListen = false;
        }


        public int Send(byte[] data)
        {    
            using (UdpClient sender = new UdpClient(ListenPort))
            {
                return sender.Send(data, data.Length, SendEndPoint);
            }
        }
    }
}
