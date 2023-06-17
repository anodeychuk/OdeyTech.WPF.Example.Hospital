// --------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using OdeyTech.Data.Provider.Interface;
using OdeyTech.ProductivityKit;
using OdeyTech.ProductivityKit.Enum;
using OdeyTech.ProductivityKit.Extension;
using OdeyTech.WPF.Common.Manager;
using OdeyTech.WPF.Common.ViewModel;
using OdeyTech.WPF.Example.Hospital.Model;
using OdeyTech.WPF.Example.Hospital.Model.Provider;
using OdeyTech.WPF.Example.Hospital.Properties;
using OdeyTech.WPF.Example.Hospital.Views;

namespace OdeyTech.WPF.Example.Hospital.ViewModel
{
    /// <summary>
    /// Represents the main view model of the application, responsible for managing the main window and patient data.
    /// </summary>
    public partial class MainViewModel : ObservableObject, IWindowViewModel, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ProductInfo productInfo;
        private readonly PatientProvider patientProvider;
        private Patient selectedPatient;
        private string searchText;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for dependency injection.</param>

        public MainViewModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.productInfo = new ProductInfo(Assembly.GetExecutingAssembly());
            this.patientProvider = this.serviceProvider.GetRequiredService<PatientProvider>();
            this.patientProvider.LoadingChanged += PatientProvider_LoadingChanged;
        }

        /// <summary>
        /// Gets the window title.
        /// </summary>
        public string WindowTitle => this.productInfo.ApplicationName;

        /// <summary>
        /// Gets or sets the current window instance.
        /// </summary>
        public Window CurrentWindow { get; set; }

        /// <summary>
        /// Gets the collection view of patients with search filtering.
        /// </summary>
        public ICollectionView Patients
        {
            get
            {
                List<Patient> sourceCollection = this.patientProvider.Items == null ? new() : this.patientProvider.Items.OrderBy(p => p.Birthday).ToList();
                ICollectionView collection = CollectionViewSource.GetDefaultView(sourceCollection);
                collection.Filter = patient => SearchText.IsNullOrEmpty()
                  || ((Patient)patient).Name.Contains(SearchText)
                  || ((Patient)patient).Surname.Contains(SearchText)
                  || ((Patient)patient).Patronymic.Contains(SearchText);
                return collection;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a patient can be added.
        /// </summary>
        public bool CanAdd => IsOperable;

        /// <summary>
        /// Gets a value indicating whether a patient can be edited.
        /// </summary>
        public bool CanEdit => IsOperable && SelectedPatient != null;

        /// <summary>
        /// Gets a value indicating whether a patient can be removed.
        /// </summary>
        public bool CanRemove => IsOperable && SelectedPatient != null;

        private bool IsOperable => !this.patientProvider.IsLoading;

        /// <summary>
        /// Gets or sets the selected patient.
        /// </summary>
        public Patient SelectedPatient
        {
            get => this.selectedPatient;
            set
            {
                SetProperty(ref this.selectedPatient, value);
                RefreshButton();
            }
        }

        /// <summary>
        /// Gets or sets the search text for filtering patients.
        /// </summary>
        public string SearchText
        {
            get => this.searchText;
            set
            {
                SetProperty(ref this.searchText, value);
                RefreshPatientsGrid();
            }
        }

        /// <summary>
        /// Disposes of resources used by the view model.
        /// </summary>
        public void Dispose() => this.patientProvider.Dispose();

        /// <summary>
        /// Adds a new patient.
        /// </summary>
        [RelayCommand]
        public void Add()
        {
            try
            {
                Patient patient = this.patientProvider.NewItem();
                var viewModel = new PatientViewModel(patient, Resources.PatientWindowTitleNew);
                ShowWindow<PatientWindow>(viewModel);
                if (viewModel.ResultButton == ButtonName.Save)
                {
                    this.patientProvider.Add(viewModel.Patient);
                    RefreshPatientsGrid();
                }
            }
            catch (Exception ex)
            {
                ShowError("Error when adding a patient.", ex);
            }
        }

        /// <summary>
        /// Edits the selected patient.
        /// </summary>
        [RelayCommand]
        public void Edit()
        {
            try
            {
                Patient patient = this.patientProvider.BeginEdit(SelectedPatient);
                var viewModel = new PatientViewModel(patient, Resources.PatientWindowTitleEdit);
                ShowWindow<PatientWindow>(viewModel);
                if (viewModel.ResultButton == ButtonName.Save)
                {
                    this.patientProvider.EndEdit(viewModel.Patient);
                    RefreshPatientsGrid();
                }
            }
            catch (Exception ex)
            {
                ShowError("Error when editing a patient.", ex);
            }
        }

        /// <summary>
        /// Removes the selected patient.
        /// </summary>
        [RelayCommand]
        public void Remove()
        {
            if (AskDelete() == ButtonName.No)
            {
                return;
            }

            try
            {
                this.patientProvider.Remove(SelectedPatient);
                RefreshPatientsGrid();
            }
            catch (Exception ex)
            {
                ShowError("Error when removing a patient.", ex);
            }
        }

        /// <summary>
        /// Handles the <see cref="PatientProvider.LoadingChanged"/> event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> containing event data.</param>

        private void PatientProvider_LoadingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IDataProvider<Patient>.IsLoading))
            {
                OnPropertyChanged(nameof(Patients));
                RefreshButton();
            }
        }

        /// <summary>
        /// Refreshes the state of the buttons.
        /// </summary>
        private void RefreshButton()
        {
            OnPropertyChanged(nameof(CanAdd));
            OnPropertyChanged(nameof(CanEdit));
            OnPropertyChanged(nameof(CanRemove));
        }

        /// <summary>
        /// Refreshes the patient collection view.
        /// </summary>
        private void RefreshPatientsGrid()
        {
            Patients.Refresh();
            OnPropertyChanged(nameof(Patients));
        }

        /// <summary>
        /// Asks for confirmation before deleting a patient.
        /// </summary>
        /// <returns>The <see cref="ButtonName"/> indicating the user's choice.</returns>
        private ButtonName AskDelete()
          => this.serviceProvider.GetRequiredService<IViewManager>().ShowAskView(
              Resources.ConfirmationDialogTitle,
              Resources.DeleteConfirmationMessage.Format(1),
              new[] { ButtonName.Yes, ButtonName.No },
              CurrentWindow);

        /// <summary>
        /// Shows a dialog window of type <typeparamref name="T"/> with the specified view model.
        /// </summary>
        /// <typeparam name="T">The type of the dialog window to show.</typeparam>
        /// <param name="viewModel">The view model for the dialog window.</param>
        private void ShowWindow<T>(IWindowViewModel viewModel) where T : Window, new()
          => this.serviceProvider.GetRequiredService<IViewManager>().ShowDialog<T>(viewModel, CurrentWindow);

        private void ShowError(string message, Exception ex)
            => this.serviceProvider.GetRequiredService<IViewManager>().ShowError(Resources.ErrorWindowTitle, new Exception(message, ex), CurrentWindow);
    }
}
