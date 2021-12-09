using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Parameter one")]
        public short ParameterOne { get; set; }

        [DisplayName("Parameter two")]
        public short ParameterTwo { get; set; }

        [DisplayName("Lower theshold")]
        public short LowerTheshold { get; set; }

        [DisplayName("Upper theshold")]
        public short UpperTheshold { get; set; }

        public bool IsParameterTwoAcceptable => ParameterTwo >= LowerTheshold && ParameterTwo <= UpperTheshold;

    }

   
}
