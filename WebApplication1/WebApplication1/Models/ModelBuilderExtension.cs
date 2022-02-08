using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var doctorsDane = new List<Doctor>();
            doctorsDane.Add(new Doctor 
                {IdDoctor = 1, FirstName = "John", LastName = "Bayer", Email = "adaD@ADas.sd"});
            doctorsDane.Add(new Doctor
                {IdDoctor = 2, FirstName = "Lana", LastName = "Rhoades", Email = "LasVegas@ADas.sd"});

            modelBuilder.Entity<Doctor>()
                .HasData(doctorsDane);


            var patientDane = new List<Patient>();
            patientDane.Add(new Patient
                {IdPatient = 1, FirstName = "Salvares", LastName = "Depo", BirthDate = new DateTime(2013, 6, 1)});
            patientDane.Add(new Patient
                {IdPatient = 2, FirstName = "Jack", LastName = "Stone", BirthDate = new DateTime(1999, 5, 2)});

            modelBuilder.Entity<Patient>()
                .HasData(patientDane);

            var prescriptionDane = new List<Prescription>();
            prescriptionDane.Add(new Prescription
            {
                IdPrescription = 1, Date = new DateTime(2020, 5, 2), DueDate = new DateTime(2021, 5, 2), IdPatient = 1,
                IdDoctor = 2
            });
            prescriptionDane.Add(new Prescription
            {
                IdPrescription = 2, Date = new DateTime(2021, 5, 2), DueDate = new DateTime(2022, 5, 2), IdPatient = 2,
                IdDoctor = 2
            });
            prescriptionDane.Add(new Prescription
            {
                IdPrescription = 3, Date = new DateTime(2021, 2, 21), DueDate = new DateTime(2022, 2, 21),
                IdPatient = 1, IdDoctor = 1
            });

            modelBuilder.Entity<Prescription>()
                .HasData(prescriptionDane);

            var medicamentDane = new List<Medicament>();
            medicamentDane.Add(new Medicament
                {IdMedicament = 1, Name = "Sutrapil", Description = "Lorem ipsum...", Type = "Lorem Ipsum"});
            medicamentDane.Add(new Medicament
                {IdMedicament = 2, Name = "Satadon", Description = "Lorem ipsum...", Type = "Lorem Ipsum"});
            medicamentDane.Add(new Medicament
                {IdMedicament = 3, Name = "Methadon", Description = "Lorem ipsum...", Type = "Lorem Ipsum"});

            modelBuilder.Entity<Medicament>()
                .HasData(medicamentDane);

            var prescriptionMedicamentDane = new List<PrescriptionMedicament>();
            prescriptionMedicamentDane.Add(new PrescriptionMedicament
                {IdMedicament = 1, IdPrescription = 1, Dose = 1000, Details = "Lorem ipsum..."});
            prescriptionMedicamentDane.Add(new PrescriptionMedicament
                {IdMedicament = 2, IdPrescription = 1, Dose = 1000, Details = "Lorem ipsum..."});
            prescriptionMedicamentDane.Add(new PrescriptionMedicament
                {IdMedicament = 3, IdPrescription = 1, Dose = 400, Details = "Lorem ipsum..."});
            prescriptionMedicamentDane.Add(new PrescriptionMedicament
                {IdMedicament = 3, IdPrescription = 2, Dose = 400, Details = "Lorem ipsum..."});
            prescriptionMedicamentDane.Add(new PrescriptionMedicament
                {IdMedicament = 1, IdPrescription = 3, Dose = 1000, Details = "Lorem ipsum..."});

            modelBuilder.Entity<PrescriptionMedicament>()
                .HasData(prescriptionMedicamentDane);
        }
    }
}