using Microsoft.AspNetCore.Mvc;

namespace ASP_SPD111.Models.Lesson1
{
    public record TransferViewModel
    {
        public DateOnly Date {get;set;}
        public TimeOnly Time { get;set;}
        public string ControllerName { get;set;} = string.Empty;
    }
}
