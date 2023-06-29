// --------------------------------------------------------------------------
// <copyright file="MockPatientProvider.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using OdeyTech.WPF.Example.Hospital.Model;
using OdeyTech.WPF.Example.Hospital.Model.Provider;

namespace OdeyTech.WPF.Example.Hospital.Test.Provider
{
    internal class MockPatientProvider : PatientProvider
    {
        public MockPatientProvider(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override IEnumerable<Patient> LoadInternal() => new List<Patient>();
    }
}
