using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebTransportSystem.Models;
using WebTransportSystem.Utilities;

namespace WebTransportSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IViewRenderService _viewRenderService;

        public HomeController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        [HttpPost]
        public JsonResult GetId(int id)
        {
            //Thread.Sleep(1000);
            //id = DateTime.Now.ToString();
            id++;
            return new JsonResult(id);
        }


        [HttpPost]
        public IActionResult GetPartialView(int id)
        {
            //Thread.Sleep(1000);
            //id = DateTime.Now.ToString();
            id++;
            var view = _viewRenderService.RenderToStringAsync("Home/Passengers", id).Result;
            var result = new
            {
                id,
                view
            };
            return new JsonResult(result);
        }

        [HttpPost]
        public JsonResult GetNextStepPartialView(Passenger[][] passengers)
        {
            var view = _viewRenderService.RenderToStringAsync("Home/Passengers", passengers).Result;
            SetNeighborsPassengers(passengers);

            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    passengers[i][j].ChooseNextTransportType();
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                TransportSystem.ChangeQualityCoefficients(passengers[i]);
            }

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    passengers[i][j].UpdateSatisfaction();
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    passengers[i][j].Neighbors = new HashSet<Passenger>();
                }
            }

            var result = new
            {
                passengers,
                view
            };
            return new JsonResult(result);
        }

        [HttpGet]
        public IActionResult Show()
        {
            var jsonPassengers = (string) TempData["passengers"];
            var passengers = JsonConvert.DeserializeObject<Passenger[][]>(jsonPassengers);

            return View(passengers);
        }

        [Route("previous")]
        public IActionResult Create()
        {
            var passengers = GetPassengers();
            return View(passengers);
        }

        [Route("")]
        public IActionResult CreatePassengers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSandboxPassengers(int rowCount, int columnCount)
        {
            var passengers = new List<List<Passenger>>();
            for (var i = 0; i < rowCount; i++)
            {
                passengers.Add(new List<Passenger>());
                for (var j = 0; j < columnCount; j++)
                    passengers[i].Add(PassengersHelper.CreatePassenger(i * 10 + j));
            }

            return PartialView(passengers);
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

        private static List<Passenger> GetPassengers()
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
        public IActionResult ShowNew(Passenger[][] newPassengers)
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

            for (var i = 0; i < 9; i++)
                passengers[i].Neighbors = new HashSet<Passenger>();

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

        private void SetNeighborsPassengers(Passenger[][] passengers)
        {
            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (i > 0)
                    {
                        passengers[i][j].AddNeighbor(passengers[i-1][j]);
                    }
                    if (j > 0)
                    {
                        passengers[i][j].AddNeighbor(passengers[i][j-1]);
                    }
                    if (i < rowCount - 1)
                    {
                        passengers[i][j].AddNeighbor(passengers[i + 1][j]);
                    }
                    if (j < columnCount - 1)
                    {
                        passengers[i][j].AddNeighbor(passengers[i][j + 1]);
                    }
                }
            }
        }
    }
}