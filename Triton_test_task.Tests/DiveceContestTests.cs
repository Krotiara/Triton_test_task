using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triton_test_task.Models;
using Xunit;

namespace Triton_test_task.Tests
{
    public class DiveceContestTests
    {
        private DeviceContext devicesContext;
       
        public DiveceContestTests()
        {
            devicesContext = new DeviceContext();
        }


        [Fact]
        public void AddNewDeviceTest()
        {
            devicesContext.ProcessData(DeviceByteMessagesConverter.GenerateTestDeviceValues(1, 10, 100));
            Device device = devicesContext.Devices[1];
            
            Assert.Single(devicesContext.Devices);
            Assert.Equal(10, device.ParameterOne);
            Assert.Equal(100, device.ParameterTwo);
        }

        [Fact]
        public void CreateMessageBodyLengthTest()
        {    
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                { "lower threshold", "10" },
                { "upper threshold", "100" } };

            Assert.Equal(6, devicesContext.CreateMessage(1, "LR").Length);
            Assert.Equal(10, devicesContext.CreateMessage(1,"LW", parameters).Length);
        }

        [Fact]
        public void CreateMessageBodyWithWrongParametersTest()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                { "wrong parameter", "10" }};

            Assert.Throws<MessageCreationException>(() => devicesContext.CreateMessage(1, "LW", parameters));
        }

        [Fact]
        public void CreateWRMessageWithoutParametersTest()
        {
            Assert.Throws<MessageCreationException>(() => devicesContext.CreateMessage(1,"LW"));
        }


        [Fact]
        public void UpdateDeviceParamsTest()
        {
            int testId = 1;
            devicesContext.ProcessData(DeviceByteMessagesConverter.GenerateTestDeviceValues(testId, 10, 100)); 
            short parameterOne = 0;
            short parameterTwo = 200;
            byte[] updateValuesData = DeviceByteMessagesConverter.GenerateTestDeviceValues(testId, parameterOne, parameterTwo);
            
            devicesContext.ProcessMessageForDevice(testId, updateValuesData);

            Assert.Equal(parameterOne, devicesContext.Devices[testId].ParameterOne);
            Assert.Equal(parameterTwo, devicesContext.Devices[testId].ParameterTwo);
        }


        [Fact]
        public void UpdateDeviceThresholdsTest()
        {
            int testId = 1;
            devicesContext.ProcessData(DeviceByteMessagesConverter.GenerateTestDeviceValues(testId, 10, 100));
            
            short testLowerThreshold = 50;
            short testUpperThreshold = 80;
            short status = Convert.ToInt16("0x0000", 16); // hex to Int32
            byte[] answerData = DeviceByteMessagesConverter.GenerateTestDeviceAnswer(testId, "LW", status, testLowerThreshold, testUpperThreshold);

            devicesContext.ProcessMessageForDevice(testId, answerData);

            Assert.Equal(testLowerThreshold, devicesContext.Devices[testId].LowerTheshold);
            Assert.Equal(testUpperThreshold, devicesContext.Devices[testId].UpperTheshold);
        }

        [Fact]
        public void UpdateDeviceThresholdsWithErrorCode()
        {
            int testId = 1;
            devicesContext.ProcessData(DeviceByteMessagesConverter.GenerateTestDeviceValues(testId, 10, 100));

            short testLowerThreshold = 50;
            short testUpperThreshold = 80;
            short status = Convert.ToInt16("0x000F", 16); // hex to Int32
            byte[] answerData = DeviceByteMessagesConverter.GenerateTestDeviceAnswer(testId, "LW", status, testLowerThreshold, testUpperThreshold);

            Assert.Throws<MessageAnswerException>(() => devicesContext.ProcessMessageForDevice(testId, answerData));
        }
    }
}
