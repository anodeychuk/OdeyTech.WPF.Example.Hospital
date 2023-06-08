// --------------------------------------------------------------------------
// <copyright file="DependencyInjectionConfig.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Microsoft.Extensions.DependencyInjection;
using OdeyTech.SqlProvider.Entity.Database.Checker;
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
            // Registers IDbConnection service as a transient service
            // Each time it's requested a new SQLiteConnection will be created
            services.AddTransient<IDbConnection>(provider => new SQLiteConnection(ConfigurationManager.ConnectionStrings["SQLiteDbConnection"].ConnectionString));

            // Registers PatientRepository as a transient service
            // Each time it's requested a new PatientRepository will be created with a new IDbConnection and IDbChecker
            services.AddTransient(provider =>
            {
                IDbConnection connection = provider.GetRequiredService<IDbConnection>();
                var dbChecker = new SQLiteChecker();
                return new PatientRepository(connection, dbChecker);
            });

            services.AddTransient<PatientProvider>();
            services.AddSingleton<IViewManager, ViewManager>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<PatientViewModel>();
        }
    }
}
