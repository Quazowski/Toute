using System.Globalization;
using System.Windows.Controls;

namespace Toute
{
    /// <summary>
    /// Validation Rule that check if the string
    /// is match to RegEx condition
    /// </summary>
    public class MinimumCharacterRule : ValidationRule
    {
        /// <summary>
        /// Minimum character, that string should have
        /// </summary>
        public int MinimumCharacters { get; set; }

        /// <summary>
        /// Overrides base Validate rule
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="cultureInfo"></param>
        /// <returns>True if string match to RegEx condition, otherwise false</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            //Check if string is not null or empty, if yes, return false
            if (string.IsNullOrEmpty(charString))
                return new ValidationResult(false, $"Minimum length is {MinimumCharacters} characters.");

            //Check if string is of minimum character length, if not return false
            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"Minimum length is {MinimumCharacters} characters.");

            //If both condition are passed, return true
            return new ValidationResult(true, null);
        }
    }
}
