using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Toute
{
    /// <summary>
    /// Validation Rule that check if the string
    /// is match to RegEx condition
    /// </summary>
    public class IsEmailRule : ValidationRule
    {
        /// <summary>
        /// Overrides base Validate rule
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="cultureInfo"></param>
        /// <returns>True if string match to RegEx condition, otherwise false</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            //Check if there is any string, if it is null or empty, return false
            if (string.IsNullOrEmpty(charString))
                return new ValidationResult(false, $"Email is invalid.");

            //Check if string match to email pattern, if not return false
            if (!Regex.IsMatch(charString, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"))
                return new ValidationResult(false, $"Email is invalid.");

            //if both conditions are passed, return true
            return new ValidationResult(true, null);
        }
    }
}
