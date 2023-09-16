using System.ComponentModel.DataAnnotations;

namespace ERegWeb.Models.Requests
{
    /// <summary>
    /// Model for email code validation request parameters
    /// </summary>
    public class ValidateEmailCodeRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
