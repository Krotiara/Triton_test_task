using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Update(byte[] receivedData)
        {
            switch(receivedData.Length)
            {
                case 8:
                    // data package
                    ProcessDataPackage(receivedData);
                    break;
                case 12:
                    ProcessAnswerRequery(receivedData);
                    break;
            }   
        }

        
        private void ProcessDataPackage(byte[] receivedData)
        {
            int id = BitConverter.ToInt32(receivedData, 0);
            Params["lower threshold"] = BitConverter.ToInt16(receivedData, 4).ToString();
            Params["upper threshold"] = BitConverter.ToInt16(receivedData, 6).ToString();
        }

        private void ProcessAnswerRequery(byte[] receivedData)
        {
            throw new NotImplementedException();
        }
    }

    

}
