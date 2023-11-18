using Microsoft.AspNetCore.Mvc;

namespace ASP_SPD111.Models.Lesson1
{
    // Моделі форм мають автоматичний мапер, тобто значення полів (властивостей) моделі 
    // заповнюється за збігом з іменами полів форм. Однак, часто в HTML прийнято вживати 
    // kebab-casing (user-firstname), який не доволений у C#. У такому разі мапінг зазначається атрибутами.
    // FromQuery - from URL (get параметри)
    // FromForm - from body (post параметри)
    public class TransferFormModel
    {
        [FromForm(Name = "user-firstname")]
        public String UserFirstname { get; set; } = null;

        [FromForm(Name = "user-lastname")]
        public String UserLastname { get; set; } = null;
    }
}
