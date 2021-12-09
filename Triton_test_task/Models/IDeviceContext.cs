using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public interface IDeviceContext
    {
        public Dictionary<int, Device> Devices { get; }

        public void ProcessData(byte[] deviceData);

        public byte[] CreateMessage(int deviceId, string messageType, Dictionary<string,string> parameters = null);

        public void ProcessMessageForDevice(int deviceId, byte[] deviceData);

        public event Action<int> AddNewDeviceEvent;
    }
}
