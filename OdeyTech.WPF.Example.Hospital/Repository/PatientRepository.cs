// --------------------------------------------------------------------------
// <copyright file="PatientRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using OdeyTech.Data.Repository;
using OdeyTech.SqlProvider.Entity.Database;
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Entity.Table.Column;
using OdeyTech.SqlProvider.Entity.Table.Column.Constraint;
using OdeyTech.SqlProvider.Entity.Table.Column.DataType;
using OdeyTech.SqlProvider.Entity.Table.Column.NameConverter;
using OdeyTech.SqlProvider.Entity.Table.Column.ValueConverter;
using OdeyTech.WPF.Example.Hospital.Model;

namespace OdeyTech.WPF.Example.Hospital.Repository
{
    /// <summary>
    /// Represents a repository for managing patient data in the database.
    /// </summary>
    public class PatientRepository : ModelRepository<Patient>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientRepository"/> class.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbChecker">The database checker.</param>
        public PatientRepository(IDbConnection dbConnection) : base(DatabaseType.SQLite, dbConnection)
        {
            TableTemplate.SetName("patient");
            var converter = new SQLiteValueConverter();
            TableTemplate.Columns.Add(nameof(Patient.Identifier), new SQLiteDataType(SQLiteDataType.DataType.Integer), converter);
            TableTemplate.Columns.Add(nameof(Patient.Name), new SQLiteDataType(SQLiteDataType.DataType.Text), converter);
            TableTemplate.Columns.Add(nameof(Patient.Surname), new SQLiteDataType(SQLiteDataType.DataType.Text), converter);
            TableTemplate.Columns.Add(nameof(Patient.Patronymic), new SQLiteDataType(SQLiteDataType.DataType.Text), converter);

            TableTemplate.Columns.Add(
                new SqlColumn(
                    nameof(Patient.Birthday),
                    new SQLiteDataType(SQLiteDataType.DataType.Date),
                    null,
                    converter,
                    new SQLiteDateNameConverter()));

            TableTemplate.Columns.Add(nameof(Patient.Phone), new SQLiteDataType(SQLiteDataType.DataType.Text), converter);
            TableTemplate.Columns.Add(nameof(Patient.Postcode), new SQLiteDataType(SQLiteDataType.DataType.Integer), converter);
            TableTemplate.Columns.Add(nameof(Patient.Country), new SQLiteDataType(SQLiteDataType.DataType.Text), converter);
            TableTemplate.Columns.Add(nameof(Patient.City), new SQLiteDataType(SQLiteDataType.DataType.Text), converter);
            TableTemplate.Columns.Add(nameof(Patient.Address), new SQLiteDataType(SQLiteDataType.DataType.Text), converter);

            var primaryKey = new PrimaryKeyConstraint() { ColumnNames = new List<string> { nameof(Patient.Identifier) } };
            TableTemplate.Columns.AddConstraints(primaryKey);
        }

        /// <summary>
        /// Saves a patient item to the database.
        /// </summary>
        /// <param name="tableSource">The SQL table source.</param>
        /// <param name="item">The patient item to save.</param>
        protected override void SaveItem(SqlTable tableSource, Patient item)
        {
            base.SaveItem(tableSource, item);
            tableSource.Columns.SetValue(nameof(Patient.Name), item.Name);
            tableSource.Columns.SetValue(nameof(Patient.Surname), item.Surname);
            tableSource.Columns.SetValue(nameof(Patient.Patronymic), item.Patronymic);
            tableSource.Columns.SetValue(nameof(Patient.Birthday), item.Birthday);
            tableSource.Columns.SetValue(nameof(Patient.Phone), item.Phone);
            tableSource.Columns.SetValue(nameof(Patient.Postcode), item.Postcode);
            tableSource.Columns.SetValue(nameof(Patient.Country), item.Country);
            tableSource.Columns.SetValue(nameof(Patient.City), item.City);
            tableSource.Columns.SetValue(nameof(Patient.Address), item.Address);
        }

        /// <summary>
        /// Gets a patient item from a data row.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>The patient item.</returns>
        protected override Patient GetItem(DataRow row)
        {
            Patient patient = base.GetItem(row);
            patient.Name = row.Field<string>(1);
            patient.Surname = row.Field<string>(2);
            patient.Patronymic = row.Field<string>(3);
            patient.Birthday = GetDate(row[4].ToString());
            patient.Phone = row.Field<string>(5);
            patient.Postcode = (int)Convert.ToInt64(row[6]);
            patient.Country = row.Field<string>(7);
            patient.City = row.Field<string>(8);
            patient.Address = row.Field<string>(9);
            return patient;
        }

        private DateTime GetDate(string date)
            => DateTime.TryParseExact(date, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime result)
                ? result
                : throw new InvalidCastException($"Unable to convert '{date}' to a DateTime object. Expected format is 'yyyy-MM-dd'.");
    }
}
