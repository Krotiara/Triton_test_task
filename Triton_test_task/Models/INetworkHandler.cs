using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public interface INetworkHandler
    {
        public void BeginReceive();

        public void StopReceive();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Return number of sended bytes</returns>
        public int Send(byte[] data);

        public event Action<byte[]> OnRecieve;
    }
}
