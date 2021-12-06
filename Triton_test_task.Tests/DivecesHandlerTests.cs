using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triton_test_task.Models;
using Xunit;

namespace Triton_test_task.Tests
{
    public class DivecesHandlerTests
    {
        private DevicesHandler<Device> devicesHandler;
       
        public DivecesHandlerTests()
        {
            devicesHandler = new DevicesHandler<Device>();
        }

        private byte[] GenerateTestDeviceValues(int id, short lowerThreshold, short upperThreshold)
        {
            byte[] idb = BitConverter.GetBytes(id);
            byte[] lowerThresholdb = BitConverter.GetBytes(lowerThreshold);
            byte[] upperThresholdb = BitConverter.GetBytes(upperThreshold);
            byte[] messageBody = idb.Concat(lowerThresholdb).Concat(upperThresholdb).ToArray();
            return messageBody;
        }

        private byte[] GenerateTestAnswer(int id, string command, short commandStatus, short lowerThreshold, short upperThreshold)
        {
            byte[] idb = BitConverter.GetBytes(id);
            byte[] commandb = Encoding.ASCII.GetBytes(command);
            byte[] commandStatusb = BitConverter.GetBytes(commandStatus);
            byte[] lowerThresholdb = BitConverter.GetBytes(lowerThreshold);
            byte[] upperThresholdb = BitConverter.GetBytes(upperThreshold);
            byte[] messageBody = idb
                .Concat(commandb)
                .Concat(commandStatusb)
                .Concat(lowerThresholdb)
                .Concat(upperThresholdb)
                .ToArray();
            return messageBody;
        }


        [Fact]
        public void GetDeviceTest()
        {
            devicesHandler.Add(1, GenerateTestDeviceValues(1, 10, 100));
            Assert.NotNull(devicesHandler.Get(1));   
        }

        [Fact]
        public void GetDeviceByNotExistIdTest()
        {
            devicesHandler.Add(1, GenerateTestDeviceValues(1, 10, 100));
            Assert.Null(devicesHandler.Get(1000));
        }

        [Fact]
        public void AddNewDeviceTest()
        {
            Assert.Empty(devicesHandler.Devices);
            devicesHandler.Add(1, GenerateTestDeviceValues(1,10,100));
            Assert.Single(devicesHandler.Devices);
            Device device = devicesHandler.Get(1);
            Assert.Equal("10", device.Params["lower parameter"]);
            Assert.Equal("100", device.Params["upper parameter"]);
        }

        [Fact]
        public void CreateMessageBodyLengthTest()
        {
            Device device = new Device(){Id = 1};
            Assert.Equal(6, device.CreateMessage("LR").Length);

            Dictionary<string, string> parameters = new Dictionary<string, string>() { 
                { "lower threshold", "10" }, 
                { "upper threshold", "100" } };

            Assert.Equal(10, device.CreateMessage("WR", parameters).Length);
        }

        [Fact]
        public void CreateMessageBodyWithWrongParametersTest()
        {
            Device device = new Device() { Id = 1 };
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                { "wrong parameter", "10" }};
            Assert.Throws<MessageCreationException>(() => device.CreateMessage("WR", parameters));
        }

        [Fact]
        public void CreateWRMessageWithoutParametersTest()
        {
            Device device = new Device() { Id = 1 };
            Assert.Throws<MessageCreationException>(() => device.CreateMessage("WR"));
        }


        [Fact]
        public void UpdateDeviceParamsTest()
        {
            
            devicesHandler.Add(1, GenerateTestDeviceValues(1, 10, 100));
            Device device = devicesHandler.Get(1);
            short testLowerParameter = 0;
            short testUpperParameter = 200;
            byte[] updateValuesData = GenerateTestDeviceValues(1, testLowerParameter, testUpperParameter);
            
            devicesHandler.Update(1, updateValuesData);
            Assert.Equal(testLowerParameter.ToString(), device.Params["lower parameter"]);
            Assert.Equal(testUpperParameter.ToString(), device.Params["upper parameter"]);
        }


        [Fact]
        public void UpdateDeviceThresholdsTest()
        {
            devicesHandler.Add(1, GenerateTestDeviceValues(1, 10, 100));
            Device device = devicesHandler.Get(1);
            short testLowerThreshold = 50;
            short testUpperThreshold = 80;
            short status = Convert.ToInt16("0x0000", 16); // hex to Int32
            byte[] answerData = GenerateTestAnswer(1, "WR", status, testLowerThreshold, testUpperThreshold);
            devicesHandler.Update(1, answerData);
            Assert.Equal(testLowerThreshold.ToString(), device.Params["lower threshold"]);
            Assert.Equal(testUpperThreshold.ToString(), device.Params["upper threshold"]);
        }

        [Fact]
        public void UpdateDeviceThresholdsWithErrorCode()
        {
            devicesHandler.Add(1, GenerateTestDeviceValues(1, 10, 100));
            Device device = devicesHandler.Get(1);
            short testLowerThreshold = 50;
            short testUpperThreshold = 80;
            short status = Convert.ToInt16("0x000F", 16); // hex to Int32
            byte[] answerData = GenerateTestAnswer(1, "WR", status, testLowerThreshold, testUpperThreshold);
            Assert.Throws<MessageAnswerException>(() => devicesHandler.Update(1, answerData));
        }
    }
}
