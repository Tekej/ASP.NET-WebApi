using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Table("Prescription", Schema = "Doc")]
    public class Prescription
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        
        public int IdDoctor { get; set; }

        public int IdPatient { get; set; }
        public virtual Doctor Doctor { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual ICollection<PrescriptionMedicament> PrescriptionMedicament { get; set; }

    }
}
