using ASP_SPD111.Data;
using System.Security.Claims;

namespace ASP_SPD111.MiddleWare
{
    public class AuthSessionMiddleWare
    {
        // ланцюг MiddleWare утворюється при інсталяції проєкту
        // і кожен учасник (ланка) MiddleWare одержує при створені
        // посилання на наступну ланку (_next). Підключення MiddleWare
        // здійснюється у Program.cs
        private readonly RequestDelegate _next;

        public AuthSessionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        // є дві схеми включення MiddleWare - синхрона та асинхронна
        // для них передбачені методи Invoke або InvokeAsync
        public async Task InvokeAsync(HttpContext context, 
            DataContext _dataContext // інжекція у MiddleWare іде через метод
        )
        {
            // задача - перевірити наявнисть у сесії збереженного AuthUserId
            // за наявності - перевірити валідність шляхом пошуку у БД
            if(context.Session.Keys.Contains("AuthUserId"))
            {
                var user = _dataContext
                    .Users
                    .Find(Guid.Parse(context.Session.GetString("AuthUserId")!));

                if(user != null)
                {
                    // перекладаємо відомості про користувача до контексту
                    // Http у формалізмі Claims
                    Claim[] claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new (ClaimTypes.Name, user.Name ?? ""),
                        new (ClaimTypes.Email, user.Email),
                        new (ClaimTypes.UserData, user.Avatar ?? ""),
                    };
                    context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(claims, nameof(AuthSessionMiddleWare)
                        )
                    );
                }
            }

            // тіло MiddleWare ділиться на дві частини: 
            // прямий хід (до виклику наступної ланки) ...
            await _next(context);
            // та зворотній хід - після виклику.
        }
    }
}
