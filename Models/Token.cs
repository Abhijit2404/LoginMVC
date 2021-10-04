using Newtonsoft.Json;

namespace LoginMVC.Models{

    public class Token{
        [JsonProperty("token")]
        public string tokenString { get; set; }
    }
}