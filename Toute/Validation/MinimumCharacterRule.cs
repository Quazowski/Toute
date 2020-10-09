using System.Globalization;
using System.Windows.Controls;

namespace Toute
{
    public class MinimumCharacterRule : ValidationRule
    {
        public int MinimumCharacters { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            if(string.IsNullOrEmpty(charString))
                return new ValidationResult(false, $"Minimum length is {MinimumCharacters} characters.");

            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"Minimum length is {MinimumCharacters} characters.");

            return new ValidationResult(true, null);
        }
    }
}
