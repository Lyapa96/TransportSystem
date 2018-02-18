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

            TransportSystem.ChangeQuality(passengers);

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

        [HttpPost]
        public IActionResult ShowNew(Passenger[][] newPassengers)
        {
            var passengers = newPassengers.ToArray();

            TempData["passengers"] = JsonConvert.SerializeObject(passengers);
            return RedirectToAction("Show", "Home");
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