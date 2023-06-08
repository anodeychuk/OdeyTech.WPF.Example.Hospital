// --------------------------------------------------------------------------
// <copyright file="App.xaml.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OdeyTech.WPF.Common.Manager;
using OdeyTech.WPF.Common.Utility;
using OdeyTech.WPF.Example.Hospital.Configuration;
using OdeyTech.WPF.Example.Hospital.ViewModel;

namespace OdeyTech.WPF.Example.Hospital
{
    /// <summary>
    /// Represents the entry point of the application and manages application-level events.
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// Configures and builds the dependency injection container.
        /// </summary>
        public App()
        {
            var services = new ServiceCollection();
            DependencyInjectionConfig.ConfigureServices(services);
            this.serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Handles the <see cref="Application.OnStartup"/> event of the application.
        /// </summary>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Current.SetupExceptionHandling(this.serviceProvider);
        }

        /// <summary>
        /// Handles the <see cref="Application.Startup"/> event of the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainViewModel mainViewModel = this.serviceProvider.GetRequiredService<MainViewModel>();
            IViewManager viewManager = this.serviceProvider.GetRequiredService<IViewManager>();
            viewManager.Show<MainWindow>(mainViewModel);
        }
    }
}
