using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class DevicesController: Controller
    {

        private readonly INetworkHandler networkHandler;
        private readonly IDeviceContext deviceContext;

        public DevicesController(INetworkHandler networkHandler, IDeviceContext deviceContext)
        {
            this.networkHandler = networkHandler;
            this.deviceContext = deviceContext;
            Task.Run(() => ProcessDevicesMessages());
        }

        public IActionResult Index()
        {
            return View("Index", deviceContext.Devices);
        }


        public IActionResult GetDevicesTable()
        {
            return PartialView("DevicesView", deviceContext.Devices);
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

        private void ProcessDevicesMessages()
        {
            foreach (byte[] data in networkHandler.Recieve()) //А если не будет даты?
            {
                if (data != null)
                    deviceContext.ProcessData(data);
            }       
        }

    }
}
