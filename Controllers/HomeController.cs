using ASP_SPD111.Models;
using ASP_SPD111.Models.HomeWork1;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ASP_SPD111.Models.Lesson1;
using ASP_SPD111.Services.Hash;
using System.Text.Json;
using ASP_SPD111.Services.Validation;
using ASP_SPD111.Models.HomwORK2;
using ASP_SPD111.Data;
using ASP_SPD111.Data.Entities;

namespace ASP_SPD111.Controllers
{
    public class HomeController : Controller
    {
        // залежність (від служби) заявляється як private readonly поле
        private readonly IHashService _hashService; // DIP - тип залежності це інтерфейс
        private readonly IValidationService _validationService;
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        // конструктор зааначає необхідні залежності, їх передає - Resolver (Injector)
        public HomeController(
            ILogger<HomeController> logger,
            IHashService hashService,
            IValidationService validationService,
            DataContext context
            )
        {
            _logger = logger;
            _hashService = hashService;
            _validationService = validationService;
            _context = context;
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
        public ViewResult HomeWork2()
        {
            ValidationModel? validationModel = null;

            if (HttpContext.Session.Keys.Contains("validationModel"))
            {
                validationModel = JsonSerializer.Deserialize<ValidationModel>(
                    HttpContext.Session.GetString("validationModel")!
                );
                HttpContext.Session.Remove("validationModel");
            }
            else
            {
                validationModel = null;
            }

            ValidationViewModel model = new()
            {
                ValidationModel = validationModel,
                IsFirstNameValid = false,
            };

            if (validationModel != null)
            {
                model.IsFirstNameValid = _validationService.ValidateFirstName(validationModel.FirstName);
                model.IsLastNameValid = _validationService.ValidateLastName(validationModel.LastName);
                model.IsEmailValid = _validationService.ValidateEmail(validationModel.Email);
                model.IsPhoneNumberValid = _validationService.ValidatePhoneNumber(validationModel.Phone);
                model.Result = "User was not added to database!";

                if (model.IsEmailValid && model.IsPhoneNumberValid && model.IsLastNameValid && model.IsFirstNameValid)
                {
                    string result = "User is exists already!";

                    var users = _context.HomeWotkUsers;

                    bool checkIsExist = users.Any(u => u.FirstName == validationModel.FirstName &&
                                                       u.LastName == validationModel.LastName);

                    if (!checkIsExist)
                    {
                        HomeWotkUser newUser = new HomeWotkUser();
                        newUser.FirstName = validationModel.FirstName;
                        newUser.LastName = validationModel.LastName;
                        newUser.Email = validationModel.Email;
                        newUser.Phone = validationModel.Phone;
                        newUser.Moment = DateTime.Now;

                        users.Add(newUser);
                        _context.SaveChanges();
                        result = "User added successfully!";
                    }

                    model.Result = result;
                }
            
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult ProcessValidationForm(ValidationModel? validationModel)
        {
            if (validationModel != null)
            {
                HttpContext.Session.SetString(
                    "validationModel",
                    JsonSerializer.Serialize(validationModel)
                );
            }

            return RedirectToAction(nameof(HomeWork2));
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
        public ViewResult Db()
        {
            return View();
        }
        public ViewResult HomeWork3()
        {
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