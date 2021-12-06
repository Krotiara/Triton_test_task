using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triton_test_task.Models;
using Xunit;

namespace Triton_test_task.Tests
{
    public class DivecesHandlerTests
    {
        private DevicesHandler<Device> devicesHandler;
       
        public DivecesHandlerTests()
        {
            devicesHandler = new DevicesHandler<Device>();
        }

        [Fact]
        public void AddNewDeviceTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void UpdateDeviceTest()
        {
            throw new NotImplementedException();
        }



    }
}
