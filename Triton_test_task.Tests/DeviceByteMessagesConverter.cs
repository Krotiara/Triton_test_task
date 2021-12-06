using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triton_test_task.Tests
{
    public static class DeviceByteMessagesConverter
    {
        public static byte[] GenerateTestDeviceValues(int id, short firstParameter, short secondParameter)
        {
            byte[] idb = BitConverter.GetBytes(id);
            byte[] firstParameterb = BitConverter.GetBytes(firstParameter);
            byte[] secondParameterb = BitConverter.GetBytes(secondParameter);
            byte[] messageBody = idb.Concat(firstParameterb).Concat(secondParameterb).ToArray();
            return messageBody;
        }

        public static byte[] GenerateTestDeviceAnswer(int id, string command, short commandStatus, short lowerThreshold, short upperThreshold)
        {
            byte[] idb = BitConverter.GetBytes(id);
            byte[] commandb = Encoding.ASCII.GetBytes(command);
            byte[] commandStatusb = BitConverter.GetBytes(commandStatus);
            byte[] upperThresholdb = BitConverter.GetBytes(upperThreshold);
            byte[] lowerThresholdb = BitConverter.GetBytes(lowerThreshold);
           
            byte[] messageBody = idb
                .Concat(commandb)
                .Concat(commandStatusb)
                .Concat(upperThresholdb)
                .Concat(lowerThresholdb)
                .ToArray();
            return messageBody;
        }
    }
}
