using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class DevicesController: Controller
    {
        private Dictionary<int, Device> devices;
        private readonly INetworkHandler networkHandler;

        public DevicesController(INetworkHandler networkHandler)
        {
            devices = new Dictionary<int, Device>();
            this.networkHandler = networkHandler;
            Task.Run(() => networkHandler.Listen(UpdateDevice));
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

        private void UpdateDevice(byte[] deviceData)
        {

        }

    }
}
