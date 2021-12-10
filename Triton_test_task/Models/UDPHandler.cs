using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class UDPHandler: INetworkHandler, IDisposable
    {
        public int ListenPort { get; }
        public int SendPort { get; }
        public IPEndPoint ListenEndPoint { get; }
        public IPEndPoint SendEndPoint { get; }

        private UdpClient listener;

        private bool isListen = true;
  

        public event Action<byte[]> OnRecieve;

        public UDPHandler(int listenPort, int sendPort)
        {
            this.ListenPort = listenPort;
            this.SendPort = sendPort;
            ListenEndPoint = new IPEndPoint(IPAddress.Loopback, listenPort);
            SendEndPoint = new IPEndPoint(IPAddress.Loopback, sendPort);
            listener = new UdpClient(ListenPort);

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
                        OnRecieve.Invoke(reseivedResult.Buffer);
                    }
                }
            });
        }

        public void BeginReceive()
        {
            Listen();
        }


        private void StopListen()
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

        public void Dispose()
        {
            listener.Close();
            StopListen();
        }
    }
}
