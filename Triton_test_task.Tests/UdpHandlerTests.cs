using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Triton_test_task.Models;
using Xunit;

namespace Triton_test_task.Tests
{
    public class UdpHandlerTests
    {
        UDPHandler udpHandler;

        public UdpHandlerTests()
        {
            udpHandler = new UDPHandler(62006, 62005);
        }

        [Fact]
        public void DataRecieveTest()
        {
            foreach(byte[] data in udpHandler.Listen())
            {
                Assert.True(data.Length > 0);
                udpHandler.IsListen = false;
            }
        }

        [Fact]
        public void DataSendTest()
        {
            throw new NotImplementedException();
            //Send, get answer and check status
        }



    }
}
