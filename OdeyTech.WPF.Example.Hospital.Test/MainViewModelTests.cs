// --------------------------------------------------------------------------
// <copyright file="MainViewModelTests.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OdeyTech.ProductivityKit;
using OdeyTech.ProductivityKit.Enum;
using OdeyTech.SqlProvider.Entity.Database.Checker;
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Executor;
using OdeyTech.SqlProvider.Query;
using OdeyTech.WPF.Common.Manager;
using OdeyTech.WPF.Common.ViewModel;
using OdeyTech.WPF.Example.Hospital.Model;
using OdeyTech.WPF.Example.Hospital.Model.Provider;
using OdeyTech.WPF.Example.Hospital.Repository;
using OdeyTech.WPF.Example.Hospital.Test.Provider;
using OdeyTech.WPF.Example.Hospital.Test.Repository;
using OdeyTech.WPF.Example.Hospital.ViewModel;

namespace OdeyTech.WPF.Example.Hospital.Test
{
    [TestClass]
    public class MainViewModelTests
    {
        private MainViewModel viewModel;
        private Patient patient;
        private Mock<ISqlQueryGenerator> queryGeneratorMock;
        private Mock<ISqlExecutor> sqlExecutorMock;
        private bool isEdit;

        [TestInitialize]
        public void TestInitialize() => this.viewModel = new MainViewModel(GetServiceProvider());

        [TestMethod]
        public void Add_AddsNewPatient()
        {
            // Arrange
            this.isEdit = false;
            // Act
            this.viewModel.Add();
            // Assert
            this.queryGeneratorMock.Verify(m => m.Insert(It.IsAny<SqlTable>()), Times.Once);
            this.sqlExecutorMock.Verify(m => m.Query(It.Is<string>(s => s.StartsWith("INSERT")), It.IsAny<DbParameter[]>()), Times.Once);
            Assert.AreEqual(1, this.viewModel.Patients.Cast<object>().Count());
            PatientTestHelper.ComparePatients(this.patient, (Patient)this.viewModel.Patients.CurrentItem);
        }

        [TestMethod]
        public void Edit_EditsSelectedPatient()
        {
            // Arrange
            this.viewModel.Add();
            this.isEdit = true;
            this.viewModel.SelectedPatient = this.patient;
            // Act
            this.viewModel.Edit();

            // Assert
            this.queryGeneratorMock.Verify(m => m.Update(It.IsAny<SqlTable>()), Times.Once);
            this.sqlExecutorMock.Verify(m => m.Query(It.Is<string>(s => s.StartsWith("UPDATE")), It.IsAny<DbParameter[]>()), Times.Once);
            Assert.AreEqual(1, this.viewModel.Patients.Cast<object>().Count());
            PatientTestHelper.ComparePatients(this.patient, (Patient)this.viewModel.Patients.CurrentItem);
        }

        [TestMethod]
        public void Remove_RemovesSelectedPatient()
        {
            // Arrange
            this.isEdit = false;
            this.viewModel.Add();
            this.viewModel.SelectedPatient = this.patient;

            // Act
            this.viewModel.Remove();

            // Assert
            this.queryGeneratorMock.Verify(m => m.Delete(It.IsAny<SqlTable>()), Times.Once);
            this.sqlExecutorMock.Verify(m => m.Query(It.Is<string>(s => s.StartsWith("DELETE")), It.IsAny<DbParameter[]>()), Times.Once);
            Assert.AreEqual(0, this.viewModel.Patients.Cast<object>().Count());
        }

        private ServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            try
            {
                services.AddTransient(c => GetConnection());
                services.AddTransient<PatientRepository>(provider => GetPatientRepository(provider.GetRequiredService<IDbConnection>()));
                services.AddTransient<PatientProvider, MockPatientProvider>();
                services.AddSingleton(c => GetViewManager());
                services.AddTransient<PatientViewModel>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while configuring services.", ex);
            }

            return services.BuildServiceProvider();
        }

        private IDbConnection GetConnection() => new Mock<IDbConnection>().Object;

        private MockPatientRepository GetPatientRepository(IDbConnection connection)
        {
            var dbChecker = new Mock<IDbChecker>();
            this.sqlExecutorMock = new Mock<ISqlExecutor>();
            this.sqlExecutorMock.Setup(e => e.Query(It.IsAny<string>(), It.IsAny<DbParameter[]>()));

            this.queryGeneratorMock = new Mock<ISqlQueryGenerator>();
            this.queryGeneratorMock.Setup(q => q.Insert(It.IsAny<SqlTable>())).Returns("INSERT");
            this.queryGeneratorMock.Setup(q => q.Update(It.IsAny<SqlTable>())).Returns("UPDATE");
            this.queryGeneratorMock.Setup(q => q.Delete(It.IsAny<SqlTable>())).Returns("DELETE");

            var patientRepositoryMock = new MockPatientRepository(connection);
            patientRepositoryMock.SetSqlExecutor(this.sqlExecutorMock.Object);
            patientRepositoryMock.SetDbChecker(dbChecker.Object);
            patientRepositoryMock.SetSqlQueryGenerator(this.queryGeneratorMock.Object);
            return patientRepositoryMock;
        }

        private IViewManager GetViewManager()
        {
            var mockViewManager = new Mock<IViewManager>();
            mockViewManager.Setup(v => v.ShowDialog<Window>(It.IsAny<IWindowViewModel>(), It.IsAny<Window>()))
               .Callback<IWindowViewModel, Window>(
                (viewModel, window) =>
                {
                    if (viewModel is PatientViewModel patientViewModel)
                    {
                        this.patient = this.isEdit
                            ? PatientTestHelper.GenerateTestPatient(PatientTestHelper.PatientModelType.Valid, patientViewModel.Patient.Identifier)
                            : PatientTestHelper.GenerateTestPatient(PatientTestHelper.PatientModelType.Valid);

                        patientViewModel.Patient = this.patient;
                        patientViewModel.ResultButton = ButtonName.Save;
                    }
                });

            return mockViewManager.Object;
        }

        // update query
        // update once
        // items updated

        // delete query
        // delete once
        // items.count
        // search deleted item

        // Add
        // Add
        // Edit
        // Add
        // Remove
        // Add

    }
}
