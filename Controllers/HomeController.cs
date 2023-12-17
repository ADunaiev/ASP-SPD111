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
using ASP_SPD111.Models.Lesson4;

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
                model.IsFullNameValid = _validationService.ValidateFullName(validationModel.FullName);
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
        public ViewResult SignUp()
        {
            SignupViewModel viewModel = new();
            // перевіряємо чи є дані від форми

            if (HttpContext.Session.Keys.Contains("formStatus"))
            {
                // декодуємо статус
                viewModel.FormStatus = Convert.ToBoolean(HttpContext.Session.GetString("formStatus"));
                // перевіряємо - якщо помилковий, то у сесії дані валідації і моделі
                HttpContext.Session.Remove("formStatus");

                if (viewModel.FormStatus ?? false)
                {
                    viewModel.FormModel = null;
                    viewModel.FormValidation = null;
                }
                else
                {
                    viewModel.FormModel = JsonSerializer.Deserialize<SignupFormModel>(
                        HttpContext.Session.GetString("formModel")!);
                    HttpContext.Session.Remove("formModel");

                    viewModel.FormValidation = JsonSerializer.Deserialize<SignupFormValidation>(
                        HttpContext.Session.GetString("formValidation")!);
                    HttpContext.Session.Remove("formValidation");
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public RedirectToActionResult SignupForm(SignupFormModel model) 
        {
            SignupFormValidation results = new();
            bool isFormValid = true;

            if (String.IsNullOrEmpty(model.Login))
            {
                results.LoginErrorMessage = "Логін не може бути порожним!";
                isFormValid = false;
            }
            else if(_validationService.ValidateLogin(model.Login) == false)
            {
                results.LoginErrorMessage = "Логін не валідний!";
                isFormValid = false;
            }
            if (String.IsNullOrEmpty(model.Name))
            {
                results.NameErrorMessage = "ПІБ не може бути порожним!";
                isFormValid = false;
            }
            else if(_validationService.ValidateFullName(model.Name) == false)
            {
                results.NameErrorMessage = "Повне ім'я не валідне!";
                isFormValid = false;
            }
            if (String.IsNullOrEmpty(model.Email))
            {
                results.EmailErrorMessage = "Пошта не може бути порожним!";
                isFormValid = false;
            }
            else if(_validationService.ValidateEmail(model.Email) == false)
            {
                results.EmailErrorMessage = "Email не валідний!";
                isFormValid = false;
            }
            if (String.IsNullOrEmpty(model.Password))
            {
                results.PasswordErrorMessage = "Пароль не може бути порожним!";
                isFormValid = false;
            }
            else if(model.Password.Length <= 2)
            {
                results.PasswordErrorMessage = "Пароль закороткий!";
                isFormValid = false;
            }
            if (model.Repeat != model.Password)
            {
                results.RepeatErrorMessage = "Повтор не збігається з паролем!";
                isFormValid = false;
            }

            if(isFormValid && model.Avatar != null && model.Avatar.Length > 0) // поле не обов'язкове, але якщо є, то перевіряємо
            {
                // при збереженні файлів (uploading) файлів слід міняти їх імена.
                int dotPosition = model.Avatar.FileName.LastIndexOf(".");
                if (dotPosition == -1)
                {
                    results.AvatarErrorMessage = "Файли без розширення не приймаються";
                    isFormValid = false;
                }
                else
                {
                    String ext = model.Avatar.FileName.Substring(dotPosition);
                    // TO DO: додати перевірку розширення на перелік дозволенних
                    List<String> allowedExtansions = new()
                    {
                        ".jpg",
                        ".png"
                    };
                    if (!allowedExtansions.Any(s => s.Equals(ext)))
                    {
                        results.AvatarErrorMessage = " Розширення не є допустимим! Дозволені розширення:";

                        foreach(var item in allowedExtansions)
                        {
                            results.AvatarErrorMessage += " " + item;
                        }
                        results.AvatarErrorMessage += ".";

                        isFormValid = false;
                    }
                    else
                    {
                        // генеруємо випадкове ім'я файлу, зберігаємо розширення
                        // контролюємо, що такого імені не має у сховищі
                        String dir = Directory.GetCurrentDirectory();

                        String savedName;
                        String fileName;
                        do
                        {
                            fileName = Guid.NewGuid() + ext;
                            savedName = Path.Combine(dir, "wwwroot", "avatars", fileName);
                        }
                        while (System.IO.File.Exists(savedName));

                        using Stream stream = System.IO.File.OpenWrite(savedName);
                        model.Avatar.CopyTo(stream);

                        // до цього місця доходимо у разі відсутності помилок валідації
                        // відповідно додаємо нового користувача до БД
                        String salt = _hashService.HexString(Guid.NewGuid().ToString());
                        String dk = _hashService.HexString(salt + model.Password);

                        _context.Users.Add(new()
                        {
                            Id = Guid.NewGuid(),
                            Login = model.Login,
                            Name = model.Name,
                            Avatar = fileName,
                            RegisterDt = DateTime.Now,
                            DeleteDt = null,
                            Email = model.Email,
                            PasswordSalt = salt,
                            PasswordDk = dk,
                        });
                        _context.SaveChanges();
                    }
                }

            }

            if (!isFormValid)
            {
                HttpContext.Session.SetString("formModel",
                    JsonSerializer.Serialize(model));
                HttpContext.Session.SetString("formValidation",
                    JsonSerializer.Serialize(results));
            }

            HttpContext.Session.SetString("formStatus",
                isFormValid.ToString());

            return RedirectToAction(nameof(SignUp));
        }
        public ViewResult HomeWork3()
        {
            return View();
        }
        public ViewResult HomeWork5()
        {
            return View();
        }
        public ViewResult HomeWork6()
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