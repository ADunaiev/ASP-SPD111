using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ASP_SPD111.Services.Validation
{
    public class MyValidationService : IValidationService
    {
        public bool ValidateEmail(string email)
        {
            if (email != null)
            {
                string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

                Regex rg = new Regex(pattern);

                MatchCollection match = rg.Matches(email);

                if (match.Count != 0) { return true; }
            }

            return false;
        }

        public bool ValidateFirstName(string firstName)
        {

            if (firstName != null)
            {
                string pattern = @"\d";

                Regex rg = new Regex(pattern);

                MatchCollection matches = rg.Matches(firstName);

                if (matches.Count == 0) { return true; }
            }

            return false;
        }

        public bool ValidateLastName(string lastName)
        {
            if (lastName != null)
            {
                string pattern = @"\d";

                Regex rg = new Regex(pattern);

                MatchCollection matches = rg.Matches(lastName);

                if (matches.Count == 0) { return true; }
            }

            return false;
        }

        public bool ValidateFullName(string fullName)
        {
            if (fullName != null)
            {
                string pattern = @"\b([A-ZА-ЯІЇЄ]{1}[a-zа-яіїє]{1,30}[- ]{0,1}|[A-ZА-ЯІЇЄ]{1}[- \']{1}[A-ZА-ЯІЇЄ]{0,1}  
                                    [a-zа-яіїє]{1,30}[- ]{0,1}|[a-zа-яіїє]{1,2}[ -\']{1}[A-ZА-ЯІЇЄ]{1}[a-zа-яіїє]{1,30}){2,5}";

                Regex rg = new Regex(pattern);

                MatchCollection matches = rg.Matches(fullName);

                if (matches.Count != 0 && matches[0].Length == fullName.Length)
                { return true; }
            }
            return false;
        }
        
        public bool ValidateLogin(string fullName)
        {
            if (fullName != null)
            {
                string pattern = @"^[a-zA-Z]{1}[a-zA-Z1-9]{1,9}";

                Regex rg = new Regex(pattern);

                MatchCollection matches = rg.Matches(fullName);

                if (matches.Count != 0 && matches[0].Length == fullName.Length)
                { return true; }
            }
            return false;
        }

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            if (phoneNumber != null)
            {
                string pattern = @"^\+38\(0\d\d\)([- ]?\d){7}$";

                Regex rg = new Regex(pattern);

                MatchCollection matches = rg.Matches(phoneNumber);

                if (matches.Count != 0) { return true; }
            }
            
            return false;
        }
    }
}
