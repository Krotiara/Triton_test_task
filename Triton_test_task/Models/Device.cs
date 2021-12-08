using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Triton_test_task.Models.Enums;

namespace Triton_test_task.Models
{
    public class Device
    {

        public Device(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public short ParameterOne { get; set; }

        public short ParameterTwo { get; set; }

        public short LowerTheshold { get; set; }

        public short UpperTheshold { get; set; }

    }

   
}
