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
        DevicesHandler<Device> devicesHandler;

        public UdpHandlerTests()
        {
            udpHandler = new UDPHandler(62006, 62005);
            devicesHandler = new DevicesHandler<Device>();
        }

        [Fact]
        public void DataRecieveTest()
        {
            byte[] data = udpHandler.Receive();
            Assert.True(data.Length > 0);    
        }

        [Fact]
        public void DataSendTest()
        {
            devicesHandler.Add(1, DeviceByteMessagesConverter.GenerateTestDeviceValues(1, 10, 100));
            Device device = devicesHandler.Get(1);
            byte[] message = device.CreateMessage("LR");
            int sendedBytes = udpHandler.Send(message);
            Assert.True(sendedBytes > 0);   
        }

        [Fact]
        public void CreateDeviceByDataRecieveTest()
        {
            foreach (byte[] data in udpHandler.Listen())
            {
                devicesHandler.ProcessData(data);
                Assert.Single(devicesHandler.Devices);
                udpHandler.StopListen();
            }
        }

        [Fact]
        public void GetAnswerMessageFromSendingTest()
        {
            byte[] deviceData = udpHandler.Receive();
            devicesHandler.ProcessData(deviceData);
            Device device = devicesHandler.Devices.First().Value;
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                { "lower threshold", "10" },
                { "upper threshold", "100" } };
            udpHandler.Send(device.CreateMessage("WR", parameters)); //И как тестить изменения?

            bool isGettingAnswer = false;
            int waitingMessagesCount = 30;
            foreach (byte[] data in udpHandler.Listen())
            {
                if (waitingMessagesCount == 0)
                    udpHandler.StopListen();
                if (data.Length == 12)
                {
                    isGettingAnswer = true;
                    udpHandler.StopListen();
                }
                waitingMessagesCount--;
            }
            Assert.True(isGettingAnswer);
        }
    }
}
