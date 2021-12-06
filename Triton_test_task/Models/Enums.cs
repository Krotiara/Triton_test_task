using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class Enums
    {
        public enum DeviceCommandStatusCodes
        {
            Successes = 0,
            WrongData = 15,
            UnknownError = 255
        }
    }
}
