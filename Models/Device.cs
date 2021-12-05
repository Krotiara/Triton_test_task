using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class Device
    {
        public Device(int id, int value, int lowerTheshold, int upperThreshold)
        {
            Id = id;
            CurrentValue = value;
            LowerThreshold = lowerTheshold;
            UpperThreshold = upperThreshold;
        }

        public int Id { get; set; }

        public int CurrentValue { get; set; }

        public int LowerThreshold { get; set; }

        public int UpperThreshold { get; set; }

        public void Update(int value)
        {
            Update(value, LowerThreshold, UpperThreshold);
        }

        public void Update(int value, int lowerTheshold, int upperThreshold)
        {
            CurrentValue = value;
            LowerThreshold = lowerTheshold;
            UpperThreshold = upperThreshold;
        }

    }
}
