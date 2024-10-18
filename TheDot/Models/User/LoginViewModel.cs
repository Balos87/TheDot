using System.ComponentModel.DataAnnotations;

namespace TheDot.Models.User
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(380, MinimumLength = 1)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
