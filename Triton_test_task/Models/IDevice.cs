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

        /// <summary>
        /// Process input messages from device. Must throw MessageAnswerException in error comand statuses.
        /// </summary>
        /// <param name="receivedData"></param>
        public void ProcessMessage(byte[] receivedData);

        /// <summary>
        /// Create a message to send to device. Must throw MessageCreationException in error creation messages.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public byte[] CreateMessage(string type, Dictionary<string, string> parameters);

    }
}
