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

        public Dictionary<int, T> Devices { get; }

        public void ProcessData(byte[] deviceData)
        {
            int id = GetId(deviceData);
            if (!Devices.ContainsKey(id))
                Add(id, deviceData);
            Update(id, deviceData);  
        }

        public T Get(int id)
        {
            return Devices.ContainsKey(id) ? Devices[id] : default(T);
        }

        public T Add(int id, byte[] deviceData)
        {
            T entry = (T)Activator.CreateInstance(typeof(T));
            entry.Id = id;
            Devices[id] = entry;
            entry.ProcessMessage(deviceData);
            return entry;
        }

        public T Update(int id, byte[] deviceData)
        {
            Devices[id].ProcessMessage(deviceData);
            return Devices[id];
        }

        private int GetId(byte[] receivedData)
        {
            return BitConverter.ToInt32(receivedData, 0);
        }

    }
}
