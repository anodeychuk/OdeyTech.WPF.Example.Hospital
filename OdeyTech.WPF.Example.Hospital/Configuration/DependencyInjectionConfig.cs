// --------------------------------------------------------------------------
// <copyright file="DependencyInjectionConfig.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Microsoft.Extensions.DependencyInjection;
using OdeyTech.ProductivityKit;
using OdeyTech.WPF.Common.Manager;
using OdeyTech.WPF.Example.Hospital.Model.Provider;
using OdeyTech.WPF.Example.Hospital.Repository;
using OdeyTech.WPF.Example.Hospital.ViewModel;

namespace OdeyTech.WPF.Example.Hospital.Configuration
{
    /// <summary>
    /// Static class for configuring the dependency injection services.
    /// </summary>
    public static class DependencyInjectionConfig
    {
        /// <summary>
        /// Configures services and registers them with the provided service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        public static void ConfigureServices(IServiceCollection services)
        {
            ThrowHelper.ThrowIfNull(services, nameof(services));
            try
            {
                services.AddTransient<IDbConnection>(provider => new SQLiteConnection(ConfigurationManager.ConnectionStrings["SQLiteDbConnection"].ConnectionString));
                services.AddTransient(provider => new PatientRepository(provider.GetRequiredService<IDbConnection>()));
                services.AddTransient<PatientProvider>();
                services.AddSingleton<IViewManager, ViewManager>();
                services.AddSingleton<MainViewModel>();
                services.AddTransient<PatientViewModel>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while configuring services.", ex);
            }
        }
    }
}
