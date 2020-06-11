using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Final_Capstone_Car_Dealership.Models
{
    public class Car_DAL
    {
        public HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44326/");
        
            return client;
        }
        public async Task<List<Cars>> GetCarById(int id)
        {
            var client = GetClient();
            var response = await client.GetAsync($"car/api?id={id}");
           
            var car = await response.Content.ReadAsAsync<List<Cars>>();
            return car;
        }
    }
}
