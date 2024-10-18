using System.ComponentModel.DataAnnotations;

namespace TheDot.Models.User
{
    public class UserViewModel
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(380, MinimumLength = 3)]
        [EmailAddress]
        public string Email { get; set; }


    }

}
