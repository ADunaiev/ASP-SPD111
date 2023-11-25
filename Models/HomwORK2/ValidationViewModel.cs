namespace ASP_SPD111.Models.HomwORK2
{
    public class ValidationViewModel
    {
        public ValidationModel? ValidationModel { get; set; }

        public bool IsFirstNameValid { get; set; } = false;
        public bool IsLastNameValid { get; set; } = false;
        public bool IsEmailValid { get; set;} = false;
        public bool IsPhoneNumberValid { get; set; } = false;
        public String Result { get; set; } = null;
    }
}
