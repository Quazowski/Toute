using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Toute
{
    public class IsEmailRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            if (string.IsNullOrEmpty(charString))
                return new ValidationResult(false, $"Email is invalid.");

            if (!Regex.IsMatch(charString, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"))
                return new ValidationResult(false, $"Email is invalid.");

            return new ValidationResult(true, null);
        }
    }
}
