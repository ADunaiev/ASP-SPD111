using ASP_SPD111.Models;
using ASP_SPD111.Models.HomeWork1;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ASP_SPD111.Models.Lesson1;
using ASP_SPD111.Services.Hash;
using System.Text.Json;

namespace ASP_SPD111.Controllers
{
    public class HomeController : Controller
    {
        // залежність (від служби) заявляється як private readonly поле
        private readonly IHashService _hashService; // DIP - тип залежності це інтерфейс
        private readonly ILogger<HomeController> _logger;

        // конструктор зааначає необхідні залежності, їх передає - Resolver (Injector)
        public HomeController(ILogger<HomeController> logger, IHashService hashService)
        {
            _logger = logger;
            _hashService = hashService;
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
        public ViewResult Transfer()
        {
            TransferFormModel? formModel;

            if (HttpContext.Session.Keys.Contains("formModel"))
            {
                // є збережені у сесії дані - відновлюємо їх та видаляємо з сесії
                formModel = JsonSerializer.Deserialize<TransferFormModel>(
                    HttpContext.Session.GetString("formModel")!
                );
                HttpContext.Session.Remove("formModel");
            }
            else
            {
                formModel = null;
            }

            TransferViewModel model = new()
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                ControllerName = this.GetType().Name,
                FormModel = formModel
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult ProcessTransferForm(TransferFormModel? formModel)
        {
            // Модель у параметрах автоматично збирається з даних, що 
            // передаються у запиті.

            // зберігаємо у сесії серіалізований об'єкт formModel з 
            // іменем "formModel"
            if (formModel != null) {
                HttpContext.Session.SetString(
                    "formModel",
                    JsonSerializer.Serialize(formModel)
                );
            }

            return RedirectToAction(nameof(Transfer));
        }
        public ViewResult Ioc() 
        {
            // використовуваємо сервіс 
            ViewData["hash"] = _hashService.HexString("123");
            ViewData["objHash"] = _hashService.GetHashCode();
            return View(); 
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