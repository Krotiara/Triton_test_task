using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public interface IDevice
    {
        public int Id { get; set; }

        public Dictionary<string, string> Params { get; set; }
        public void Update(byte[] receivedData);

    }
}
