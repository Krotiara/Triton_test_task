using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task
{
    public interface INetworkHandler
    {
        public void Listen(Action<byte[]> getDataAction);

        public void Send(byte[] data);
    }
}
