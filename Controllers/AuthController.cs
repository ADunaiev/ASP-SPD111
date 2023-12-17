using ASP_SPD111.Data;
using ASP_SPD111.Services.Hash;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;

namespace ASP_SPD111.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IHashService _hashService;

        public AuthController(DataContext dataContext, IHashService hashService)
        {
            _dataContext = dataContext;
            _hashService = hashService;
        }
        [HttpGet]   
        public object Authenticate(String login, String password) 
        {
            var user = _dataContext
                .Users
                .FirstOrDefault(x => x.Login == login);

            if (user == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return new { status = "Credentials rejected" };
            }
            String dk = _hashService.HexString(user.PasswordSalt + password);

            if(user.PasswordDk != dk) 
            {
                HttpContext.Response.StatusCode= StatusCodes.Status401Unauthorized;
                return new { status = "Credentials rejected" };
            }
            // зберігаємо у сесії факт успішної автентифікації
            HttpContext.Session.SetString("AuthUserId", user.Id.ToString());

            return new { status = "Auth Confirmed", login, password };
        }

        [HttpDelete]
        public object SignOutMethod() 
        {

            if (HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                HttpContext.Session.Clear();
                return new { status = "Вихід успішний" };
            }

            return new { status = "Вихід неможливий" };
        }
    }
}
/*
 * Контролери
 * поділяються на MVC та API
 * MVC: різні endpoints (адреси), однакові методи запиту (GET, рідше POST)
 *      GET   /home/index
 *      GET   /home/privacy
 *      тип повернення - IActionResult(View - частіше за все)
 * API: один endpoint, різні методи запиту
 *      GET   /api/auth
 *      POST  /api/auth
 *      PUT   /api/auth
 *      GET, POST, PUT, PATCH, DELETE - для використання
 *      OPTIONS, HEAD, CONNECT, TRACE - службові, краще не чіпати
 *      тип повернення - "сирі" дані: текст або JSON
 *      якщо тип повернення String, то передається текст, 
 *      інакше - автоматична JSON серіалізація повернутого об'єкту
 *      + використування статус-кодів для відповіді
 * */
/*
 * Авторізація та утримання автентифікації
 * Для того, щоб знати про попередні результати автентифікації
 * (не вимагати вводити пароль з кожним оновленням сторінки)
 * сервер зберігає ці відомості у себе.
 * При надходженні нових запитів попередньо проводиться перевірка на наявність 
 * збереженних даних.
 * Для реалізації "попередньості" є механізм MiddleWare
 * AuthSessionMiddleWare
 */
