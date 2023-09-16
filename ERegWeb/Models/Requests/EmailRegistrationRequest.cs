using System.ComponentModel.DataAnnotations;

namespace ERegWeb.Models.Requests
{
    /// <summary>
    /// Model for email registration request parameters
    /// </summary>
    public class EmailRegistrationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
