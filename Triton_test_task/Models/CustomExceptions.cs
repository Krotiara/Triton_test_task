using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class MessageCreationException : Exception
    {
        public MessageCreationException(string errorMessage)
        {

        }
    }

    public class MessageAnswerException: Exception
    {
        public MessageAnswerException(string errorMessage)
        {

        }
    }


}
