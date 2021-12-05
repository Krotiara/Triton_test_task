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
            throw new NotImplementedException();
        }

       
    }
}
