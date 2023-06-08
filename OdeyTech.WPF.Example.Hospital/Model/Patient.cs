// --------------------------------------------------------------------------
// <copyright file="Patient.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using OdeyTech.Data.Model;
using OdeyTech.Data.Model.Interface;

namespace OdeyTech.WPF.Example.Hospital.Model
{
    /// <summary>
    /// Represents a patient model.
    /// </summary>
    public class Patient : BasicModel
    {
        private string name;
        private string surname;
        private string patronymic;
        private DateTime birthday;
        private string phone;
        private int postcode;
        private string country;
        private string city;
        private string address;

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class.
        /// </summary>
        public Patient() : base()
        {
            Birthday = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the model.</param>
        public Patient(ulong identifier) : base(identifier)
        { }

        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        public string Name
        {
            get => this.name;
            set => SetProperty(ref this.name, value);
        }

        /// <summary>
        /// Gets or sets the surname of the patient.
        /// </summary>
        public string Surname
        {
            get => this.surname;
            set => SetProperty(ref this.surname, value);
        }

        /// <summary>
        /// Gets or sets the patronymic of the patient.
        /// </summary>
        public string Patronymic
        {
            get => this.patronymic;
            set => SetProperty(ref this.patronymic, value);
        }

        /// <summary>
        /// Gets or sets the birthday of the patient.
        /// </summary>
        public DateTime Birthday
        {
            get => this.birthday;
            set => SetProperty(ref this.birthday, value);
        }

        /// <summary>
        /// Gets or sets the phone number of the patient.
        /// </summary>
        public string Phone
        {
            get => this.phone;
            set => SetProperty(ref this.phone, value);
        }

        /// <summary>
        /// Gets or sets the postcode of the patient.
        /// </summary>
        public int Postcode
        {
            get => this.postcode;
            set => SetProperty(ref this.postcode, value);
        }

        /// <summary>
        /// Gets or sets the country of the patient.
        /// </summary>
        public string Country
        {
            get => this.country;
            set => SetProperty(ref this.country, value);
        }

        /// <summary>
        /// Gets or sets the city of the patient.
        /// </summary>
        public string City
        {
            get => this.city;
            set => SetProperty(ref this.city, value);
        }

        /// <summary>
        /// Gets or sets the address of the patient.
        /// </summary>
        public string Address
        {
            get => this.address;
            set => SetProperty(ref this.address, value);
        }

        /// <summary>
        /// Copies the data from the specified model to the current patient.
        /// </summary>
        /// <param name="source">The source model to copy from.</param>
        public override void CopyFrom(IModel source)
        {
            if (source is not Patient patient)
            {
                throw new ArgumentException("Cannot copy from an object that is not a Patient model.", nameof(source));
            }

            Name = patient.Name;
            Surname = patient.Surname;
            Patronymic = patient.Patronymic;
            Birthday = patient.Birthday;
            Phone = patient.Phone;
            Postcode = patient.Postcode;
            Country = patient.Country;
            City = patient.City;
            Address = patient.Address;
        }
    }
}
