using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LoginMVC.Models
{
    public class UserInfo
    {
        public string Email { get; set; }
        [DataType(DataType.Password)] 
        public string Password { get; set; }

    }
}