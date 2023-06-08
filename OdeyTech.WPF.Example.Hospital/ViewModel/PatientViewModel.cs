// --------------------------------------------------------------------------
// <copyright file="PatientViewModel.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using CommunityToolkit.Mvvm.Input;
using OdeyTech.ProductivityKit.Enum;
using OdeyTech.WPF.Common.ViewModel;
using OdeyTech.WPF.Example.Hospital.Model;

namespace OdeyTech.WPF.Example.Hospital.ViewModel
{
    /// <summary>
    /// Represents the view model for the patient dialog window.
    /// </summary>
    public partial class PatientViewModel : ModalViewModel
    {
        /// <summary>
        /// Gets or sets the patient.
        /// </summary>
        public Patient Patient { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientViewModel"/> class.
        /// </summary>
        /// <param name="patient">The patient to edit or create.</param>
        /// <param name="windowTitle">The title of the dialog window.</param>
        public PatientViewModel(Patient patient, string windowTitle)
        {
            Patient = patient;
            WindowTitle = windowTitle;
        }

        /// <summary>
        /// Saves the changes made to the patient and closes the dialog.
        /// </summary>
        [RelayCommand]
        public void Save()
        {
            ResultButton = ButtonName.Save;
            Close();
        }
    }
}
