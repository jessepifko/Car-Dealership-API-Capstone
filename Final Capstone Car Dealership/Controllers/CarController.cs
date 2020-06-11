using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Final_Capstone_Car_Dealership.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Capstone_Car_Dealership.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarDBContext _context;
        public CarController(CarDBContext context)
        {
            _context = context;
        }

        public HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44326/");

            return client;
        }
        public async Task<List<Cars>> GetCarById(int id)
        {
            var client = GetClient();
            var response = await client.GetAsync($"api/car?id={id}");

            var car = await response.Content.ReadAsAsync<List<Cars>>();
            return car;
        }

        //GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<List<Cars>>> GetCars()
        {
            var car = await _context.Cars.ToListAsync();
            return car;
        }

        //GET: api/Car/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Cars>> GetCars(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                //returns a 404 error code if an employee with the given
                //id does not exist in the database
                return NotFound();
            }
            else
            {
                return car;
            }
        }

        //DELETE api/car/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            else
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                return NoContent();
                //204 status code -- successful and the API is not returning any content
            }
        }

        //POST: api/Car
        [HttpPost]
        public async Task<ActionResult<Cars>> AddCar(Cars newCar)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Add(newCar);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCars), new { id = newCar.Id }, newCar);
                //returns HTTP 201 status code - standard response for HTTP Post methods that create new
                //resources on the server
                //nameof(GetEmployee) - adds a location to the response, specifies the URI 
                //of the newly created employee (AKA where we can access the new employee)
                //C# "nameof" is used to avoid hard-coding the action in the CreatedAtAction call
            }
            else
            {
                return BadRequest();
            }
        }

        //PUT: api/Car/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutEmployee(int id, Cars updatedCar)
        {
            if (id != updatedCar.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(updatedCar).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

    }

}