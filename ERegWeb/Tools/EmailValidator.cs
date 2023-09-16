using System.Text.RegularExpressions;

namespace ERegWeb.Tools
{
    /// <summary>
    /// Utility class for email validation.
    /// </summary>
    public static class EmailValidator
    {
        private static readonly Regex EmailRegex = new Regex(
        @"^(?!\.)[a-zA-Z0-9\.\-_]+@[a-zA-Z0-9\.\-_]+\.[a-zA-Z]{2,5}$",
        RegexOptions.Compiled);

        /// <summary>
        /// Validates an email address.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>True if the email is valid, otherwise false.</returns>
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }
    }
}
