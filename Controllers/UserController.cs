using ASP_SPD111.Data;
using ASP_SPD111.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace ASP_SPD111.Controllers
{
	public class UserController : Controller
	{
		private readonly DataContext _dataContext;

        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
		{
			return View();
		}

		public ViewResult Profile(String? id)
		{
			UserProfileViewModel model = new();

			if (id == null) // спроба доступа до свого кабінету
			{
				// перевіряємо аутентифікацію
				if(HttpContext.User.Identity?.IsAuthenticated ?? false)
				{
					model.IsPersonal = true;
                    // шукаємо дані користувача за Claim
                    String sid = HttpContext
						.User
						.Claims
						.First(claim => claim.Type == ClaimTypes.Sid)
						.Value;

					// шукаємо у бд
					model.User = _dataContext
						.Users
						.Find(Guid.Parse(sid));
                }
                else // спроба доступа без входу у систему
                {
                    model.IsPersonal= false;
					model.User = null;
                }
            }
			else // вказано id - доступ до "чужого" профілю
			{
				model.IsPersonal= false;
				model.User = _dataContext
					.Users
					.FirstOrDefault(u => u.Login == id);
			}

			return View(model);
		}

		[HttpPost]
		public async Task<JsonResult> UpdateProfile(String newName, String newEmail)
		{
            if (HttpContext.User.Identity?.IsAuthenticated ?? false)
			{
                // шукаємо дані користувача за Claim
                String sid = HttpContext
                    .User
                    .Claims
                    .First(claim => claim.Type == ClaimTypes.Sid)
                    .Value;

                var user = _dataContext
					.Users
					.Find(Guid.Parse(sid));

				if(user != null)
				{
					// вносимо зміни
					user.Name = newName;
					user.Email = newEmail;
					await _dataContext.SaveChangesAsync();

					return Json(new {status = 200});
				}
            }	

            return Json(new { status = 401 });
            
            
		}

		[HttpDelete]
		public async Task<JsonResult> DeleteProfile()
		{
			var user = this.GetAuthUser();

			if (user == null)
			{
				return Json(new { status = 401 });
			}

			// _dataContext.Users.Remove(user); повне видалення порушення зв'язків даних
			user.DeleteDt = DateTime.Now; // встановлюємо ознаку видалення
										  // за вимогами законодаства видаляємо персональні дані
			user.Name = null;
			user.Login = "";
			user.Email = "";

			if(user.Avatar != null)
			{
                String dir = Directory.GetCurrentDirectory();
                String avatarFileName = Path.Combine(dir, "wwwroot", "avatars", user.Avatar);

                if (System.IO.File.Exists(avatarFileName))
                {
                    System.IO.File.Delete(avatarFileName);
                }
                user.Avatar = null;
            }

			user.PasswordSalt = "";
			user.PasswordDk = "";

			await _dataContext.SaveChangesAsync();

			return Json(new { status = 200 });
		}

		private Data.Entities.User? GetAuthUser()
		{
			if (HttpContext.User.Identity?.IsAuthenticated ?? false)
			{
				// шукаємо дані користувача за Claim
				String sid = HttpContext
					.User
					.Claims
					.First(claim => claim.Type == ClaimTypes.Sid)
					.Value;

				return _dataContext
					.Users
					.Find(Guid.Parse(sid));
			}
			return null;
		}
	}
}
