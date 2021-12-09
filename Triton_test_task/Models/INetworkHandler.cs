using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public interface INetworkHandler
    {
        public IEnumerable<byte[]> Recieve();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Return number of sended bytes</returns>
        public int Send(byte[] data);
    }
}
