using System.ComponentModel.DataAnnotations;

namespace AwesomeSoft.Meet.Models
{
    /// <summary>
    /// The request object containing the info required to authenticate.
    /// </summary>
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
