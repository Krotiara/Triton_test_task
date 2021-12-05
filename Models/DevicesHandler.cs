using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class DevicesHandler<T> where T: IDevice
    {

        public DevicesHandler()
        {
            Devices = new Dictionary<int, T>();
        }

        public Dictionary<int, T> Devices { get; set; }

        public void ProcessData(byte[] recievedData)
        {
            int id = GetId(recievedData);
            if (Devices.ContainsKey(id))
                Devices[id].Update(recievedData);
            else
            {
                T entry = (T)Activator.CreateInstance(typeof(T));
                entry.Id = id;
                Devices[id] = entry;
              
            }
        }

        private int GetId(byte[] receivedData)
        {
            return BitConverter.ToInt32(receivedData, 0);
        }
    }
}
