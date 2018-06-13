using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebTransportSystem.Models;
using WebTransportSystem.Models.TransportChooseAlgorithm;
using WebTransportSystem.Utilities;

namespace WebTransportSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly PassengerBehaviour passengerBehaviour;

        public HomeController(PassengerBehaviour passengerBehaviour)
        {
            this.passengerBehaviour = passengerBehaviour;
        }

        [HttpPost]
        public JsonResult GetNextStepPassengers(Passenger[][] passengers)
        {
            PassengersHelper.SetNeighborsPassengers(passengers, passengerBehaviour);
            MainAlgorithm.Run(passengers);
            PassengersHelper.ClearNeighborsPassengers(passengers);

            return new JsonResult(new {passengers});
        }

        [HttpPost]
        public IActionResult GetNextStepPartialView(Passenger[][] passengers)
        {
            return PartialView(passengers);
        }

        [HttpGet]
        public IActionResult Show()
        {
            var jsonPassengers = (string) TempData["passengers"];
            if (jsonPassengers == null)
                return RedirectToAction("CreatePassengers");
            var passengers = JsonConvert.DeserializeObject<Passenger[][]>(jsonPassengers);

            return View(passengers);
        }

        [Route("")]
        public IActionResult CreatePassengers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSandboxPassengers(int rowCount, int columnCount, int transmissionType)
        {
            var passengers = new List<List<Passenger>>();
            for (var i = 0; i < rowCount; i++)
            {
                passengers.Add(new List<Passenger>());
                for (var j = 0; j < columnCount; j++)
                    passengers[i]
                        .Add(PassengersHelper.CreatePassenger(passengerBehaviour, i*10 + j, (TransmissionType) transmissionType));
            }

            return PartialView(passengers);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpPost]
        public IActionResult ShowNew(Passenger[][] newPassengers)
        {
            var passengers = newPassengers.ToArray();

            TempData["passengers"] = JsonConvert.SerializeObject(passengers);
            return RedirectToAction("Show", "Home");
        }
    }
}