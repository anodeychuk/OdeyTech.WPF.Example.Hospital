// --------------------------------------------------------------------------
// <copyright file="PatientProvider.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using OdeyTech.Data.Provider;
using OdeyTech.WPF.Example.Hospital.Repository;

namespace OdeyTech.WPF.Example.Hospital.Model.Provider
{
    /// <summary>
    /// Provides access to patient data and manages patient-related operations.
    /// </summary>
    public class PatientProvider : DataProvider<Patient>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public PatientProvider(IServiceProvider serviceProvider) : base(serviceProvider.GetRequiredService<PatientRepository>())
        { }
    }
}
