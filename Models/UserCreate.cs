using System;
using System.ComponentModel.DataAnnotations;

namespace LoginMVC.Models
{
    public class UserCreate
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)] 
        public string Password { get; set; }
        public int UserHospitalId { get; set; }
    }
}