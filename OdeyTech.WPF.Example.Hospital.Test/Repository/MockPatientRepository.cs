// --------------------------------------------------------------------------
// <copyright file="MockPatientRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Data;
using OdeyTech.SqlProvider.Entity.Database.Checker;
using OdeyTech.SqlProvider.Executor;
using OdeyTech.SqlProvider.Query;
using OdeyTech.WPF.Example.Hospital.Repository;

namespace OdeyTech.WPF.Example.Hospital.Test.Repository
{
    internal class MockPatientRepository : PatientRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockPatientRepository"/> class.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbChecker">The database checker.</param>
        public MockPatientRepository(IDbConnection dbConnection) : base(dbConnection)
        { }

        public void SetSqlExecutor(ISqlExecutor sqlExecutor) => SqlExecutor = sqlExecutor;

        public void SetDbChecker(IDbChecker dbChecker) => DbChecker = dbChecker;

        public void SetSqlQueryGenerator(ISqlQueryGenerator sqlQueryGenerator) => SqlQueryGenerator = sqlQueryGenerator;
    }
}
