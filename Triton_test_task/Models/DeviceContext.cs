using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Triton_test_task.Models.Enums;

namespace Triton_test_task.Models
{
    public class DeviceContext : IDeviceContext
    {
        public Dictionary<int, Device> Devices { get; }

        public DeviceContext()
        {
            Devices = new Dictionary<int, Device>();
        }

        public event Action<int> AddNewDeviceEvent;

        public byte[] CreateMessage(int deviceId, string messageType, Dictionary<string, string> parameters = null)
        {
            switch (messageType)
            {
                case "LR":
                    //Add validation
                    return BitConverter.GetBytes(deviceId)
                        .Concat(Encoding.ASCII.GetBytes(messageType))
                        .ToArray();
                case "LW":
                    if (parameters == null)
                        throw new MessageCreationException(string.Format("Parameters for message type LW in class {0} cant be null", GetType().ToString()));
                    return CreateLWMessageType(deviceId, parameters);
            }
            throw new MessageCreationException(string.Format("CreateMessage method in class {0} dont know  message type = {1}", GetType().ToString(), messageType));
        }

        public void ProcessData(byte[] deviceData)
        {
            if (deviceData == null) return;
            int id = GetDeviceId(deviceData);
            if (!Devices.ContainsKey(id))
            {
                Devices[id] = new Device(id);
                if (AddNewDeviceEvent != null)
                    AddNewDeviceEvent.Invoke(id);
            }
            try
            {
                ProcessMessageForDevice(id, deviceData);
            }
            catch (MessageAnswerException e)
            {
                return; //TODO notification
            }
        }

        public void ProcessMessageForDevice(int deviceId, byte[] deviceData)
        {
            switch (deviceData.Length)
            {
                case 8: // values package
                    ProcessDataPackage(deviceId, deviceData);
                    break;
                case 12: // requery answer package
                    ProcessRequeryAnswer(deviceId, deviceData);
                    break;
            }
        }

        private int GetDeviceId(byte[] receivedData)
        {
            return BitConverter.ToInt32(receivedData, 0);
        }

        private byte[] CreateLWMessageType(int deviceId, Dictionary<string, string> parameters)
        {
            try
            {
                byte[] idb = BitConverter.GetBytes(deviceId);
                byte[] commandb = Encoding.ASCII.GetBytes("LW");
                byte[] upperThreshold = BitConverter.GetBytes(Int16.Parse(parameters["upper threshold"]));
                byte[] lowerThreshold = BitConverter.GetBytes(Int16.Parse(parameters["lower threshold"]));
                return idb
                       .Concat(commandb)
                       .Concat(upperThreshold)
                       .Concat(lowerThreshold)
                       .ToArray();
            }
            catch (KeyNotFoundException)
            {
                throw new MessageCreationException(string.Format("In class {0} in CreateMessage type LW parameters are wrong." +
                    " It must be lower threshold and upper threshold", GetType().ToString()));
            }
        }

        /// <summary>
        /// Process data package from device, where data is in 8 bytes length format
        /// </summary>
        /// <param name="receivedData">Input data from device</param>
        private void ProcessDataPackage(int deviceId, byte[] receivedData)
        {
            Devices[deviceId].ParameterOne = BitConverter.ToInt16(receivedData, 4);
            Devices[deviceId].ParameterTwo = BitConverter.ToInt16(receivedData, 6); 
        }


        /// <summary>
        /// Process answer for the changing thresholds limits requery, where answer is in 12 bytes length format.
        /// </summary>
        /// <param name="receivedData">Input data from device</param>
        private void ProcessRequeryAnswer(int deviceId, byte[] receivedData)
        {
            string command = BitConverter.ToString(receivedData, 4, 2); //command парсится не в исходный вид
            short commandStatus = BitConverter.ToInt16(receivedData, 6);

            switch (commandStatus)
            {
                case (short)DeviceCommandStatusCodes.Successes:
                    Devices[deviceId].UpperTheshold = BitConverter.ToInt16(receivedData, 8);
                    Devices[deviceId].LowerTheshold = BitConverter.ToInt16(receivedData, 10);    
                    break;
                case (short)DeviceCommandStatusCodes.WrongData or (short)DeviceCommandStatusCodes.UnknownError:
                    throw new MessageAnswerException(string.Format("Error command status: {0}",
                        ((DeviceCommandStatusCodes)commandStatus).ToString()));
            }
        }


    }
}
