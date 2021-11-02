using System;
using System.ComponentModel.DataAnnotations;

namespace LoginMVC.Models
{
    public class UserCreate
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)] 
        public string Password { get; set; }
        public int UserHospitalId { get; set; }
    }
}