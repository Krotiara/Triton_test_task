using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Triton_test_task.Models
{
    public class DevicesController : Controller
    {

        private readonly INetworkHandler networkHandler;
        private readonly IDeviceContext deviceContext;

        public DevicesController(INetworkHandler networkHandler, IDeviceContext deviceContext)
        {
            this.networkHandler = networkHandler;
            this.deviceContext = deviceContext;    
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index", deviceContext.Devices);
        }


        [HttpGet]
        public IActionResult GetDevicesTable()
        {
            return PartialView("DevicesView", deviceContext.Devices);
        }


        [HttpGet]
        public IActionResult ChangeThresholds()
        {
            return PartialView("EditThresholdsView", deviceContext.Devices.Keys.ToList());
        }

        [HttpPost]
        public IActionResult ChangeThresholds(int deviceId, int lowerThreshold, int upperThreshold)
        {
            networkHandler.Send(deviceContext.CreateMessage(deviceId, "LW",
                new Dictionary<string, string>() {
                   { "upper threshold", upperThreshold.ToString() },
                   { "lower threshold", lowerThreshold.ToString()} }));
            networkHandler.Send(deviceContext.CreateMessage(deviceId, "LR"));
            return RedirectToAction("Index");
        }
    }
}
