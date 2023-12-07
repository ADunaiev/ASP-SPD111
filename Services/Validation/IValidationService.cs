namespace ASP_SPD111.Services.Validation
{
    public interface IValidationService
    {
        bool ValidateFirstName(String firstName);
        bool ValidateLastName(String lastName);
        bool ValidateFullName(String fullName);
        bool ValidateLogin(String login);
        bool ValidateEmail(String email);
        bool ValidatePhoneNumber(String phoneNumber);
    }
}
