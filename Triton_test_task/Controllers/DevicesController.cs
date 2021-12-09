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
            this.deviceContext.AddNewDeviceEvent += GetThresholds;
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
        public void GetThresholds(int deviceId)
        {
            networkHandler.Send(deviceContext.CreateMessage(deviceId, "LR"));
        }

        [HttpGet]
        public IActionResult ChangeThresholds()
        {
            return PartialView("EditThresholdsView", deviceContext.Devices.Keys.ToList());
        }

        [HttpPost]
        public void ChangeThresholds(int deviceId, int lowerThreshold, int upperThreshold)
        {
            networkHandler.Send(deviceContext.CreateMessage(deviceId, "LW",
                new Dictionary<string, string>() {
                   { "upper threshold", upperThreshold.ToString() },
                   { "lower threshold", lowerThreshold.ToString()} }));
            networkHandler.Send(deviceContext.CreateMessage(deviceId, "LR"));
        }

        private void ProcessDevicesMessages()
        {
            foreach (byte[] data in networkHandler.Receive()) //А если не будет даты?
            {
                if (data != null)
                    deviceContext.ProcessData(data);
            }       
        }

    }
}
