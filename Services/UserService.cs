using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LoginMVC.Models;

namespace LoginMVC.Services
{
    public class UserService : IUserService
    {
        public UserService()
        {
            
        }
        public async Task<List<User>> GetUserList()
        {
            var userList = new List<User>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("users");

                if(response.IsSuccessStatusCode)
                {
                    var read = response.Content.ReadAsStringAsync().Result;

                    userList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(read);

                    return userList;
                }
            }

            return userList;
        }

        public async Task<bool> SaveUser(UserCreate user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Clear();
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonData,UnicodeEncoding.UTF8,"application/json");

                var res = await client.PostAsync("users",content);
                if (res.IsSuccessStatusCode)
                {
                    var read = res.Content.ReadAsStringAsync().Result;
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(read);

                    return result;
                }
            }

            return false;
        }
    }
}