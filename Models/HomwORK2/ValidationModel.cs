using Microsoft.AspNetCore.Mvc;

namespace ASP_SPD111.Models.HomwORK2
{
    public class ValidationModel
    {
        [FromForm(Name = "first-name")]
        public String FirstName { get; set; } = String.Empty;
        [FromForm(Name = "last-name")]
        public String LastName { get; set; } = String.Empty;
        [FromForm(Name = "e-mail")]
        public String Email { get; set; } = String.Empty;
        [FromForm(Name = "phone")]
        public String Phone {  get; set; } = String.Empty;
    }
}
