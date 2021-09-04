using System.ComponentModel.DataAnnotations;

namespace MediumProject.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "You have to fill in your e-mail address.")]
        [MaxLength(100, ErrorMessage = "E-mail cannot be longer than 100 characters.")]
        [EmailAddress(ErrorMessage = "Please enter your email address in the correct format")]
        public string EmailAddress { get; set; }
    }
}
