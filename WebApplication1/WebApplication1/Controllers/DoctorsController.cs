using System.IO;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DoctorsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var context = new MainDbContext();
            var result = context.Doctor.Select(x => x);
            return Ok(result);
        }

        [HttpGet("{index}")]
        public IActionResult GetOneDoctor(int index)
        {
            var context = new MainDbContext();
            var result = context.Doctor.Where(x => x.IdDoctor == index);
            if (result.Any())
            {
                return Ok(result);
            }

            using StreamWriter file = new("log.txt", true);
            file.WriteLineAsync("Doctor with this id is not found");
            return Ok("Doctor with this id is not found");
        }

        [HttpPost]
        public IActionResult AddDoctor(Doctor dane)
        {
            var context = new MainDbContext();
            if (dane.FirstName.IsNullOrWhiteSpace() ||
                dane.LastName.IsNullOrWhiteSpace() ||
                dane.Email.IsNullOrWhiteSpace())
            {
                using StreamWriter file = new("log.txt", true);
                file.WriteLineAsync("Data is not complete");
                return Ok("Data is not complete");
            }

            var idDoctor = context.Doctor.Max(x => x.IdDoctor) + 1;
            var newDoctor = new Doctor
            {
                IdDoctor = idDoctor,
                FirstName = dane.FirstName,
                LastName = dane.LastName,
                Email = dane.Email
            };
            context.Add(newDoctor);
            context.SaveChanges();
            return Ok("Successfully added new Doctor");
        }

        [HttpPut("{index}")]
        public IActionResult ChangeOneDoctor(int index, Doctor dane)
        {
            var context = new MainDbContext();
            var chosenOneDoctor = context.Doctor.Where(x => x.IdDoctor == index).ToList();
            if (!chosenOneDoctor.Any())
            {
                using StreamWriter file = new("log.txt", true);
                file.WriteLineAsync("Doctor with this id is not found");
                return Ok("Doctor with this id is not found");
            }

            if (dane.FirstName.IsNullOrWhiteSpace() &&
                dane.LastName.IsNullOrWhiteSpace() &&
                dane.Email.IsNullOrWhiteSpace())
            {
                using StreamWriter file = new("log.txt", true);
                file.WriteLineAsync("Changes were not insert");
                return Ok("Changes were not insert");
            }

            if (dane.FirstName.IsNullOrWhiteSpace() &&
                dane.LastName.IsNullOrWhiteSpace())
            {
                foreach (var elem in chosenOneDoctor) elem.Email = dane.Email;
                context.SaveChanges();
                return Ok("Doctor's dane successfully changed");
            }

            if (dane.LastName.IsNullOrWhiteSpace() &&
                dane.Email.IsNullOrWhiteSpace())
            {
                foreach (var elem in chosenOneDoctor) elem.FirstName = dane.FirstName;
                context.SaveChanges();
                return Ok("Doctor's dane successfully changed");
            }

            if (dane.FirstName.IsNullOrWhiteSpace() &&
                dane.Email.IsNullOrWhiteSpace())
            {
                foreach (var elem in chosenOneDoctor) elem.LastName = dane.LastName;
                context.SaveChanges();
                return Ok("Doctor's dane successfully changed");
            }

            if (dane.FirstName.IsNullOrWhiteSpace())
            {
                foreach (var elem in chosenOneDoctor)
                {
                    elem.LastName = dane.LastName;
                    elem.Email = dane.Email;
                }

                context.SaveChanges();
                return Ok("Doctor's dane successfully changed");
            }

            if (dane.LastName.IsNullOrWhiteSpace())
            {
                foreach (var elem in chosenOneDoctor)
                {
                    elem.FirstName = dane.FirstName;
                    elem.Email = dane.Email;
                }

                context.SaveChanges();
                return Ok("Doctor's dane successfully changed");
            }

            if (dane.Email.IsNullOrWhiteSpace())
            {
                foreach (var elem in chosenOneDoctor)
                {
                    elem.FirstName = dane.FirstName;
                    elem.LastName = dane.LastName;
                }

                context.SaveChanges();
                return Ok("Doctor's dane are successfully changed");
            }


            foreach (var elem in chosenOneDoctor)
            {
                elem.FirstName = dane.FirstName;
                elem.LastName = dane.LastName;
                elem.Email = dane.Email;
            }

            context.SaveChanges();
            return Ok("Doctor's dane successfully changed");
        }

        [HttpDelete("{index}")]
        public IActionResult DeleteDoctor(int index)
        {
            var context = new MainDbContext();
            var findDoctor = context.Doctor.Where(x => x.IdDoctor == index);
            if (findDoctor.Any())
            {
                using StreamWriter file = new("log.txt", true);
                file.WriteLineAsync("Doctor with this id is not found");
                return Ok("Doctor with this id is not found");
            }

            var orderOnDeleting = findDoctor.FirstOrDefault();
            context.Doctor.Remove(orderOnDeleting);
            context.SaveChanges();
            return Ok("Doctor was successfully deleted");
        }
    }
}