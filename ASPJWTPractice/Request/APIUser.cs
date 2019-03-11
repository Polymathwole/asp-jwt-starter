using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPJWTPractice.Request
{
    public class SignupUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, MinLength(2)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords mismatch")]
        public string ConfirmPassword { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "Phone number not valid.")]
        [MinLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        public DateTime DoB { get; set; }
    }

    public class LoginUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
