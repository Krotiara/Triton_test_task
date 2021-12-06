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

        private Dictionary<int, T> Devices { get; }

        public void ProcessData(byte[] deviceData)
        {
            int id = GetId(deviceData);
            if (!Devices.ContainsKey(id))
                Add(id, deviceData);
            Update(id, deviceData);  
        }

        public T Add(int id, byte[] deviceData)
        {
            T entry = (T)Activator.CreateInstance(typeof(T));
            entry.Id = id;
            Devices[id] = entry;
            entry.Update(deviceData);
            return entry;
        }

        public T Update(int id, byte[] deviceData)
        {
            Devices[id].Update(deviceData);
            return Devices[id];
        }

        private int GetId(byte[] receivedData)
        {
            return BitConverter.ToInt32(receivedData, 0);
        }
    }
}
