using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebTransportSystem.Models;

namespace WebTransportSystem.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Show()
        {
            var jsonPassengers = (string) TempData["passengers"];
            var passengers = JsonConvert.DeserializeObject<Passenger[]>(jsonPassengers);
            SetNeighborsPassengers(passengers);

            return View(passengers);
        }

        [Route("")]
        public IActionResult Create()
        {
            var passengers = CreatePassengers();
            return View(passengers);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact(Passenger[] p)
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        private static List<Passenger> CreatePassengers()
        {
            //плохие маршруты
            var p1 = new Passenger(TransportType.Bus, 0.3, 0.40, 1);
            var p2 = new Passenger(TransportType.Bus, 0.3, 0.40, 2);
            var p3 = new Passenger(TransportType.Bus, 0.3, 0.40, 3);

            //нормальные маршруты
            var p4 = new Passenger(TransportType.Bus, 0.4, 0.50, 4);
            var p5 = new Passenger(TransportType.Car, 0.5, 0.60, 5);
            var p6 = new Passenger(TransportType.Bus, 0.4, 0.50, 6);

            //хорошие маршруты
            var p7 = new Passenger(TransportType.Car, 0.45, 0.55, 7);
            var p8 = new Passenger(TransportType.Car, 0.45, 0.55, 8);
            var p9 = new Passenger(TransportType.Car, 0.7, 0.80, 9);

            p1.AddNeighbors(p2, p4);
            p2.AddNeighbors(p5, p3);
            p3.AddNeighbors(p2, p6);
            p4.AddNeighbors(p5, p7);
            p5.AddNeighbors(p2, p6, p4, p8);
            p6.AddNeighbors(p3, p5, p9);
            p7.AddNeighbors(p4, p8);
            p8.AddNeighbors(p7, p5, p9);


            return new List<Passenger>
            {
                p1,
                p2,
                p3,
                p4,
                p5,
                p6,
                p7,
                p8,
                p9
            };
        }

        [HttpPost]
        public IActionResult ShowNew(Passenger[] newPassengers)
        {
            var passengers = newPassengers.ToArray();
            //SetNeighborsPassengers(passengers);
            TempData["passengers"] = JsonConvert.SerializeObject(passengers);
            return RedirectToAction("Show", "Home");
        }

        [HttpPost]
        public IActionResult MakeNextStep(Passenger[] passengers)
        {
            SetNeighborsPassengers(passengers);
            foreach (var passenger in passengers)
                passenger.ChooseNextTransportType();

            TransportSystem.ChangeQualityCoefficients(passengers);

            foreach (var passenger in passengers)
                passenger.UpdateSatisfaction();

            for (int i = 0; i < 9; i++)
            {
                passengers[i].Neighbors = new HashSet<Passenger>();
            }

            TempData["passengers"] = JsonConvert.SerializeObject(passengers);

            return RedirectToAction("Show", "Home");
        }

        private void SetNeighborsPassengers(Passenger[] passengers)
        {
            passengers[0].AddNeighbors(passengers[1], passengers[3]);
            passengers[1].AddNeighbors(passengers[4], passengers[2]);
            passengers[2].AddNeighbors(passengers[1], passengers[5]);
            passengers[3].AddNeighbors(passengers[4], passengers[6]);
            passengers[4].AddNeighbors(passengers[1], passengers[5], passengers[3], passengers[7]);
            passengers[5].AddNeighbors(passengers[2], passengers[4], passengers[8]);
            passengers[6].AddNeighbors(passengers[3], passengers[7]);
            passengers[7].AddNeighbors(passengers[6], passengers[4], passengers[8]);
        }
    }
}