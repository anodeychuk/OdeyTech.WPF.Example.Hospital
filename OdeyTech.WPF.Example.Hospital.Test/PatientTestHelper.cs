// --------------------------------------------------------------------------
// <copyright file="PatientTestHelper.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OdeyTech.WPF.Example.Hospital.Model;

namespace OdeyTech.WPF.Example.Hospital.Test
{
    public class PatientTestHelper
    {
        public enum PatientModelType
        {
            Valid,
            Invalid,
            Empty
        }

        public static Patient GenerateTestPatient(PatientModelType modelType, ulong? identifier = null)
        {
            var random = new Random();
            string[] names = { "John", "Michael", "Sarah", "Jessica", "Robert" };
            string[] surnames = { "Smith", "Johnson", "Williams", "Brown", "Jones" };
            string[] patronymics = { "Johnsonovich", "Robertovich", "Williamsenovich" };
            string[] countries = { "USA", "Canada", "Australia", "UK", "Germany" };
            string[] cities = { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix" };

            var patient = identifier == null ? new Patient() : new Patient(identifier.Value);

            switch (modelType)
            {
                case PatientModelType.Valid:
                    patient.Name = names[random.Next(names.Length)];
                    patient.Surname = surnames[random.Next(surnames.Length)];
                    patient.Patronymic = patronymics[random.Next(patronymics.Length)];
                    patient.Birthday = new DateTime(random.Next(1920, 2003), random.Next(1, 13), random.Next(1, 29));
                    patient.Phone = $"555-0{random.Next(100, 1000)}-{random.Next(1000, 9999)}";
                    patient.Postcode = random.Next(10000, 99999);
                    patient.Country = countries[random.Next(countries.Length)];
                    patient.City = cities[random.Next(cities.Length)];
                    patient.Address = $"{random.Next(1, 9999)} Main St";
                    break;
                case PatientModelType.Invalid:
                    // Assuming the invalid model is a model with some invalid data, e.g., the phone number
                    patient.Name = names[random.Next(names.Length)];
                    patient.Surname = surnames[random.Next(surnames.Length)];
                    patient.Patronymic = patronymics[random.Next(patronymics.Length)];
                    patient.Birthday = new DateTime(random.Next(1920, 2003), random.Next(1, 13), random.Next(1, 29));
                    patient.Phone = "123"; // Invalid phone number
                    patient.Postcode = random.Next(10000, 99999);
                    patient.Country = countries[random.Next(countries.Length)];
                    patient.City = cities[random.Next(cities.Length)];
                    patient.Address = $"{random.Next(1, 9999)} Main St";
                    break;
                case PatientModelType.Empty:
                    // All properties are left with their default values
                    break;
            }

            return patient;
        }

        public static void ComparePatients(Patient expectedPatient, Patient actualPatient)
        {
            Assert.IsNotNull(expectedPatient);
            Assert.IsNotNull(actualPatient);

            Assert.AreEqual(expectedPatient.Identifier, actualPatient.Identifier);
            Assert.AreEqual(expectedPatient.Name, actualPatient.Name);
            Assert.AreEqual(expectedPatient.Surname, actualPatient.Surname);
            Assert.AreEqual(expectedPatient.Patronymic, actualPatient.Patronymic);
            Assert.AreEqual(expectedPatient.Birthday, actualPatient.Birthday);
            Assert.AreEqual(expectedPatient.Phone, actualPatient.Phone);
            Assert.AreEqual(expectedPatient.Postcode, actualPatient.Postcode);
            Assert.AreEqual(expectedPatient.Country, actualPatient.Country);
            Assert.AreEqual(expectedPatient.City, actualPatient.City);
            Assert.AreEqual(expectedPatient.Address, actualPatient.Address);
        }
    }
}
