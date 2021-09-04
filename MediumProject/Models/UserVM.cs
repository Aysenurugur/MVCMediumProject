using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediumProject.Models
{
    public class UserVM
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "You have to fill in your e-mail address.")]
        [MaxLength(100, ErrorMessage = "E-mail cannot be longer than 100 characters.")]
        [EmailAddress(ErrorMessage = "Please enter your email address in the correct format")]
        public string EmailAddress { get; set; }

        [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string FullName { get; set; }
        public IFormFile PhotoFile { get; set; }
        public string PhotoPath { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "You have to fill in your username.")]
        [MaxLength(20, ErrorMessage = "Username cannot be longer than 20 characters.")]
        public string UserName { get; set; }        
        public string Url { get; set; }
    }
}
