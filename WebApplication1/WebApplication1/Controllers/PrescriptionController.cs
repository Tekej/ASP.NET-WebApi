using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/prescription")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        [HttpGet("{index}")]
        public IActionResult GetPrescription(int index)
        {
            var context = new MainDbContext();
            var checker = context.Prescription.Where(x => x.IdPrescription == index);
            if (checker.Any())
            {
                using StreamWriter file = new("log.txt", true);
                file.WriteLineAsync("Prescription is not found");
                return Ok("Prescription is not found");
            }

            IEnumerable<object> result = context.Prescription.Where(x => x.IdPrescription == index).Select(x => new
            {
                PatientFirstName = x.Patient.FirstName,
                PatientLastName = x.Patient.LastName,
                PatientBirthDate = x.Patient.BirthDate,
                DoctorFirstName = x.Doctor.FirstName,
                DoctorLastName = x.Doctor.LastName,
                DoctorEmail = x.Doctor.Email,
                ListOfMedicaments = x.PrescriptionMedicament.Join(context.Medicament, y => y.IdMedicament,
                    u => u.IdMedicament, (y, u) => new {u.Name, y.Dose})
            }).ToList();
            return Ok(result);
        }
    }
}