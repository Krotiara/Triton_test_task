using System;
using System.Collections.Concurrent;
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

        private UdpClient listener;

        private bool isListen;

        private Queue<byte[]> buffer;

      

        public UDPHandler(int listenPort, int sendPort)
        {
            this.ListenPort = listenPort;
            this.SendPort = sendPort;
            ListenEndPoint = new IPEndPoint(IPAddress.Loopback, listenPort);
            SendEndPoint = new IPEndPoint(IPAddress.Loopback, sendPort);
            buffer = new Queue<byte[]>();
            listener = new UdpClient(ListenPort);
            Listen();
        }


        private async void Listen()
        {
            isListen = true;
            await Task.Run(async () =>
            {
                using (listener)
                {
                    while (isListen)
                    {
                        UdpReceiveResult reseivedResult = await listener.ReceiveAsync();
                        buffer.Enqueue(reseivedResult.Buffer);
                    }
                }
            });
           
        }

        public IEnumerable<byte[]> Receive()
        {
            byte[] dequeueRes;

            while(buffer.TryDequeue(out dequeueRes))
            {
                yield return dequeueRes;
            }
        }


        public void StopListen()
        {
            isListen = false;
        }


        public int Send(byte[] data)
        {    
            using (UdpClient sender = new UdpClient())
            {            
                return sender.Send(data, data.Length, SendEndPoint);
            }
        }
    }
}
