using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class Device: IDevice
    {

        public Device()
        {
            Params = new Dictionary<string, string>();
        }

        public int Id { get; set; }

        public Dictionary<string, string> Params { get; set; }

        public byte[] CreateMessage(string type, Dictionary<string, string> parameters = null)
        {
            switch (type)
            {
                case "LR":
                    //Add validation
                    return BitConverter.GetBytes(Id)
                        .Concat(Encoding.ASCII.GetBytes(type))
                        .ToArray();
                case "WR":
                    if (parameters == null)
                        throw new MessageCreationException(string.Format("Parameters for message type WR in class {0} cant be null", GetType().ToString()));
                    return CreateWRMessageType(parameters);
            }
            throw new MessageCreationException(string.Format("CreateMessage method in class {0} dont know  message type = {1}", GetType().ToString(), type));
        }

        private byte[] CreateWRMessageType(Dictionary<string, string> parameters)
        {
            try
            {
                byte[] idb = BitConverter.GetBytes(Id);
                byte[] commandb = Encoding.ASCII.GetBytes("WR");
                byte[] lowerThreshold = BitConverter.GetBytes(Int16.Parse(parameters["lower threshold"])); // checkIfWrongKey
                byte[] upperThreshold = BitConverter.GetBytes(Int16.Parse(parameters["upper threshold"]));
                return idb
                       .Concat(commandb)
                       .Concat(lowerThreshold)
                       .Concat(upperThreshold)
                       .ToArray();
            }
            catch(KeyNotFoundException)
            {
                throw new MessageCreationException(string.Format("In class {0} in CreateMessage type WR parameters are wrong." +
                    " It must be lower threshold and upper threshold", GetType().ToString()));
            }
        }

        public void ProcessMessage(byte[] receivedData)
        {
            switch(receivedData.Length)
            {
                case 8: // values package
                    ProcessDataPackage(receivedData);
                    break;
                case 12: // requery answer package
                    ProcessRequeryAnswer(receivedData);
                    break;
            }   
        }

        
        /// <summary>
        /// Process data package from device, where data is in 8 bytes length format
        /// </summary>
        /// <param name="receivedData">Input data from device</param>
        private void ProcessDataPackage(byte[] receivedData)
        {
            int id = BitConverter.ToInt32(receivedData, 0);
            Params["lower parameter"] = BitConverter.ToInt16(receivedData, 4).ToString();
            Params["upper parameter"] = BitConverter.ToInt16(receivedData, 6).ToString();
        }



        /// <summary>
        /// Process answer for the changing thresholds limits requery, where answer is in 12 bytes length format.
        /// </summary>
        /// <param name="receivedData">Input data from device</param>
        private void ProcessRequeryAnswer(byte[] receivedData)
        {
            int id = BitConverter.ToInt32(receivedData, 0);
            string command = BitConverter.ToString(receivedData, 4, 2);
            string commandStatus = BitConverter.ToString(receivedData,6,2).Replace("-", "");
            int lowerThreshold = BitConverter.ToInt16(receivedData, 8);
            int upperThreshold = BitConverter.ToInt16(receivedData, 10);

            switch (commandStatus)
            {
                case "0x0000": // Successes
                    Params["lower threshold"] = lowerThreshold.ToString();
                    Params["upper threshold"] = upperThreshold.ToString();
                    break;
                case "0x000F": // WrongData
                    break;
                case "0x00FF": //Unknown Error
                    break;
            }
        }
    }
}
