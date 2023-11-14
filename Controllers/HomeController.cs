using ASP_SPD111.Models;
using ASP_SPD111.Models.HomeWork1;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ASP_SPD111.Models.Lesson1;

namespace ASP_SPD111.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult HomeWork1()
        {
            RandomLatter randomLatter = new()
            {
                MyLetter = (char)((new Random()).Next(26) + 65)
            };
            return View(randomLatter);
        }
        public IActionResult HomeWork1_Task3()
        {
            var restoraunt = DataWorker.GetOneRestoraunt();

            return View(restoraunt);
        }
        public IActionResult HomeWork1_Task4()
        {
            var restoraunts = DataWorker.GetAllRestoraunts();

            return View(restoraunts);
        }
        public IActionResult HomeWork1_Task5()
        {
            var countries = DataWorker.GetAllCountries();

            return View(countries);
        }
        public IActionResult Transfer()
        {
            TransferViewModel model = new()
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                ControllerName = this.GetType().Name,

            };
            return View(model);
        }
        public IActionResult Razor()
        {
            ViewData["formController"] = "Hello from Controller";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}