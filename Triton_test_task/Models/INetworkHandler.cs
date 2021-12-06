using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public interface INetworkHandler
    {
        public IEnumerable<byte[]> Listen();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Return answer for sending query</returns>
        public byte[] Send(byte[] data);
    }
}
