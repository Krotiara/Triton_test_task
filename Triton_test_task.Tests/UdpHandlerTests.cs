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
        DeviceContext deviceContext;

        public UdpHandlerTests()
        {
            udpHandler = new UDPHandler(62006, 62005);
            deviceContext = new DeviceContext();
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
            int testId = 1;
            deviceContext.ProcessData(DeviceByteMessagesConverter.GenerateTestDeviceValues(testId, 10, 100));
            byte[] message = deviceContext.CreateMessage(1,"LR");

            int sendedBytes = udpHandler.Send(message);

            Assert.True(sendedBytes > 0);   
        }

        [Fact]
        public void CreateDeviceByDataRecieveTest()
        {
            byte[] data = udpHandler.Receive();

            deviceContext.ProcessData(data);

            Assert.Single(deviceContext.Devices);  
            
        }

        [Fact]
        public void GetAnswerMessageFromSendingTest()
        {
            Device device = GetDeviceByFirstRecieve();
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                { "lower threshold", "10" },
                { "upper threshold", "200" } };
            
            udpHandler.Send(deviceContext.CreateMessage(device.Id, "LW", parameters));
            byte[] answer = udpHandler.Receive();

            Assert.True(answer.Length == 12);

            udpHandler.Send(deviceContext.CreateMessage(device.Id, "LR"));
            answer = udpHandler.Receive();

            Assert.True(answer.Length == 12);
        }


        [Fact]
        public void SetDataBySendTest()
        {
            Device device = GetDeviceByFirstRecieve();
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                { "lower threshold", "10" },
                { "upper threshold", "200" } };

            udpHandler.Send(deviceContext.CreateMessage(device.Id, "LW", parameters));
            byte[] answer = udpHandler.Receive();
            deviceContext.ProcessData(answer);

            Assert.True(device.LowerTheshold == 10);
            Assert.True(device.UpperTheshold == 200);
        }


        private Device GetDeviceByFirstRecieve()
        {
            byte[] deviceData = udpHandler.Receive();
            deviceContext.ProcessData(deviceData);
            Device device = deviceContext.Devices.First().Value;
            return device;
        }

    }
}
