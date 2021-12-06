using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class DevicesController: Controller
    {

        private readonly INetworkHandler networkHandler;
        private readonly DevicesHandler<Device> devicesHandler;

        public DevicesController(INetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler; 
            this.devicesHandler = new DevicesHandler<Device>(); //without DI for now.
            Task.Run(() => ProcessTheDeviceMessage());
        }

        public IActionResult Index()
        {
            return View();
            //throw new NotImplementedException();
            //Каждый девайс - PartialView
        }

       

        [HttpGet]
        public IActionResult Get(int id)
        {
            throw new  NotImplementedException();
            //TODO мб без Id, т.к. ловить надо все
        }

        [HttpPost]
        public IActionResult Post(int id, int lowerThreshold, int upperThreshold)
        {
            throw new NotImplementedException();
            //TODO
        }

        private void ProcessTheDeviceMessage()
        {
            foreach (byte[] data in networkHandler.Listen()) //А если не будет даты?
            {
                devicesHandler.ProcessData(data);
            }
        }

    }
}
