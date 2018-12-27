// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Zion.Data
{
    /// <summary>
    /// Builds data table instances.
    /// </summary>
    public abstract class DataTableBuilder : IDisposable
    {
        /// <summary>
        /// Gets a <see cref="DataTable"/>instance.
        /// </summary>
        public DataTable InternalDataTable { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableBuilder"/> class.
        /// </summary>
        protected DataTableBuilder()
        {
            InternalDataTable = new DataTable();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableBuilder"/> class with the specified table name.
        /// </summary>
        /// <param name="tableName">The name to give the table.</param>
        protected DataTableBuilder(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(tableName));

            // Create the data table.
            // Note: If tableName is null or an empty string, a default name 
            // Note: is given when added to the DataTableCollection.
            InternalDataTable = new DataTable(tableName);
        }

        /// <summary>
        /// Adds columns to the <see cref="DataTable"/> instance.
        /// </summary>
        protected abstract void BuildColumns();

        #region DataTable Helpers

        /// <summary>
        /// Sets an array of columns that function as primary keys for the data table
        /// </summary>
        /// <param name="dataColumn">Data column to add as a primary key.</param>
        protected void AddPrimaryKeys(DataColumn dataColumn)
        {
            if (dataColumn == null)
                throw new ArgumentNullException(nameof(dataColumn));

            if (!CheckIfColumnExist(dataColumn.ColumnName))
                throw new InvalidOperationException(Resources.ExceptionDataColumnsDoNotExist);

            AddPrimaryKeys(new[] { dataColumn });
        }

        /// <summary>
        /// Sets an array of columns that function as primary keys for the data table
        /// </summary>
        /// <param name="dataColumns">An array of primary key data columns.</param>
        protected void AddPrimaryKeys(DataColumn[] dataColumns)
        {
            if (dataColumns == null)
                throw new ArgumentNullException(nameof(dataColumns));

            if (dataColumns.Any(dataColumn => !CheckIfColumnExist(dataColumn.ColumnName)))
                throw new InvalidOperationException(Resources.ExceptionDataColumnsDoNotExist);

            InternalDataTable.PrimaryKey = dataColumns;
        }

        /// <summary>
        /// Adds a unique constraint to the data table instance.
        /// </summary>
        /// <param name="dataColumn">The DataColumn to constrain.</param>
        protected void AddUniqueConstraint(DataColumn dataColumn)
        {
            if (dataColumn == null) throw new ArgumentNullException(nameof(dataColumn));

            if (!CheckIfColumnExist(dataColumn.ColumnName))
                throw new InvalidOperationException(Resources.ExceptionDataColumnsDoNotExist);

            AddUniqueConstraint(new[] { dataColumn });
        }

        /// <summary>
        /// Adds a unique constraint to the data table instance.
        /// </summary>
        /// <param name="dataColumns">A collection of <see cref="DataColumn"/>s.</param>
        protected void AddUniqueConstraint(DataColumn[] dataColumns)
        {
            if (dataColumns == null) throw new ArgumentNullException(nameof(dataColumns));

            if (dataColumns.Any(dataColumn => !CheckIfColumnExist(dataColumn.ColumnName)))
                throw new InvalidOperationException(Resources.ExceptionDataColumnsDoNotExist);

            var uniqueConstraint = new UniqueConstraint(dataColumns);

            Debug.Assert(uniqueConstraint != null, "uniqueConstraint != null");
            AddConstraint(uniqueConstraint);
        }

        /// <summary>
        /// Adds a unique constraint to the data table instance.
        /// </summary>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <param name="dataColumn">The DataColumn to constrain.</param>
        protected void AddUniqueConstraint(string constraintName,
                                           DataColumn dataColumn)
        {
            AddUniqueConstraint(constraintName, new[] { dataColumn });
        }

        /// <summary>
        /// Adds a unique constraint to the data table instance.
        /// </summary>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <param name="dataColumns">The array of DataColumn objects to constrain.</param>
        protected void AddUniqueConstraint(string constraintName,
                                           DataColumn[] dataColumns)
        {
            if (string.IsNullOrEmpty(constraintName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(constraintName));
            if (dataColumns == null)
                throw new ArgumentNullException(nameof(dataColumns));

            if (dataColumns.Any(dataColumn => !CheckIfColumnExist(dataColumn.ColumnName)))
                throw new InvalidOperationException(Resources.ExceptionDataColumnsDoNotExist);

            var uniqueConstraint = new UniqueConstraint(constraintName, dataColumns);
            Debug.Assert(uniqueConstraint != null, "uniqueConstraint != null");
            AddConstraint(uniqueConstraint);
        }

        /// <summary>
        /// Adds a foreign key constraint.
        /// </summary>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <param name="parentColumn">The parent DataColumn in the constraint.</param>
        /// <param name="childColumn">The child DataColumn in the constraint.</param>
        protected void AddForeignKeyConstraint(string constraintName,
                                               DataColumn parentColumn,
                                               DataColumn childColumn)
        {
            var foreignKeyConstraint = new ForeignKeyConstraint(constraintName, parentColumn, childColumn) { DeleteRule = Rule.Cascade };
            Debug.Assert(foreignKeyConstraint != null, "foreignKeyConstraint != null");
            AddConstraint(foreignKeyConstraint);
        }

        /// <summary>
        /// Adds a <see cref="Constraint"/> to a <see cref="DataTable"/> instance.
        /// </summary>
        /// <param name="constraint">A <see cref="Constraint"/>.</param>
        private void AddConstraint(Constraint constraint)
        {
            if (constraint == null)
                throw new ArgumentNullException(nameof(constraint));

            GuardDataTableExist();
            InternalDataTable.Constraints.Add(constraint);
        }

        /// <summary>
        /// Adds a boolean column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddBooleanNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddBooleanColumn(columnName);
        }

        /// <summary>
        /// Adds a boolean column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddBooleanNullColumn(string columnName,
                                            bool defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddBooleanColumn(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a boolean column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddBooleanNotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

           AddBooleanColumn(columnName, null, false);
        }

        /// <summary>
        /// Adds a boolean column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddBooleanNotNullColumn(string columnName,
                                               bool defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddBooleanColumn(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a boolean column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        private void AddBooleanColumn(string columnName,
                                      bool? defaultValue = null,
                                      bool allowDbNull = true)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddColumn(columnName, Type.GetType("System.Boolean", false, true), defaultValue, allowDbNull);
        }

        /// <summary>
        /// Adds a <see cref="Guid"/> column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddByteNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddByteColumn(columnName);
        }

        /// <summary>
        /// Adds a <see cref="Guid"/> column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddByteNullColumn(string columnName,
                                         byte defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddByteColumn(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a <see cref="Guid"/> column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddByteNotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddByteColumn(columnName, null, false);
        }

        /// <summary>
        /// Adds a <see cref="Guid"/> column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddByteNotNullColumn(string columnName,
                                            byte defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddByteColumn(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a <see cref="Byte"/> column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        private void AddByteColumn(string columnName,
                                   byte? defaultValue = null,
                                   bool allowDbNull = true)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddColumn(columnName, Type.GetType("System.Byte", false, true), defaultValue, allowDbNull);
        }

        /// <summary>
        /// Adds a <see cref="Guid"/> column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddGuidNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddGuidColumn(columnName);
        }

        /// <summary>
        /// Adds a <see cref="Guid"/> column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddGuidNullColumn(string columnName,
                                         Guid defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddGuidColumn(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a <see cref="Guid"/> column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddGuidNotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddGuidColumn(columnName, null, false);
        }

        /// <summary>
        /// Adds a <see cref="Guid"/> column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddGuidNotNullColumn(string columnName,
                                            Guid defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddGuidColumn(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a <see cref="Guid"/> column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        private void AddGuidColumn(string columnName,
                                   Guid? defaultValue = null,
                                   bool allowDbNull = true)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddColumn(columnName, Type.GetType("System.Guid", false, true), defaultValue, allowDbNull);
        }

        /// <summary>
        /// Adds a string column that allows null values.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddStringNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddStringColumn(columnName);
        }

        /// <summary>
        /// Adds a string column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddStringNullColumn(string columnName,
                                           string defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddStringColumn(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a string column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddStringNotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddStringColumn(columnName, null, false);
        }

        /// <summary>
        /// Adds a string column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="maxLength">The maximum length of the column in characters.</param>
        protected void AddStringNotNullColumn(string columnName,
                                              int maxLength)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));
            if (maxLength < 0)
                throw new ArgumentException(Resources.ExceptionValueLessThanZero, nameof(maxLength));

            AddStringColumn(columnName, null, false, maxLength);
        }

        /// <summary>
        /// Adds a string column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddStringNotNullColumn(string columnName,
                                              string defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));
            if (defaultValue == null)
                throw new ArgumentNullException(nameof(defaultValue));

            AddStringColumn(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a string column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        /// <param name="maxLength">The maximum length of the column in characters.</param>
        protected void AddStringNotNullColumn(string columnName,
                                              string defaultValue,
                                              int maxLength)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));
            if (maxLength < 0)
                throw new ArgumentException(Resources.ExceptionValueLessThanZero, nameof(maxLength));

            AddStringColumn(columnName, defaultValue, false, maxLength);
        }

        /// <summary>
        /// Adds a string column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        /// <param name="maxLength">The maximum length of the column in characters.</param>
        private void AddStringColumn(string columnName,
                                     string defaultValue = null,
                                     bool allowDbNull = true,
                                     int? maxLength = null)
        {
            AddColumn(columnName, Type.GetType("System.String", false, true), defaultValue, allowDbNull, false, 0, 1,
                       null, false, false, maxLength);
        }

        /// <summary>
        /// Adds a <see cref="Single"/> column that allows null values.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddDoubleNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddDoubleColumn(columnName);
        }

        /// <summary>
        /// Adds a <see cref="Single"/> column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddDoubleNullColumn(string columnName,
                                           double defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddDoubleColumn(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a <see cref="Single"/> column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddDoubleNotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddDoubleColumn(columnName, null, false);
        }

        /// <summary>
        /// Adds a <see cref="Single"/> column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        /// ///
        protected void AddDoubleNotNullColumn(string columnName,
                                              double defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddDoubleColumn(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a <see cref="Single"/> column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        private void AddDoubleColumn(string columnName,
                                     double? defaultValue = null,
                                     bool allowDbNull = true)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddColumn(columnName, Type.GetType("System.Double", false, true), defaultValue, allowDbNull);
        }


        /// <summary>
        /// Adds a <see cref="Single"/> column that allows null values.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddSingleNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddSingleColumn(columnName);
        }

        /// <summary>
        /// Adds a <see cref="Single"/> column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddSingleNullColumn(string columnName,
                                           float defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddSingleColumn(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a <see cref="Single"/> column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddSingleNotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddSingleColumn(columnName, null, false);
        }

        /// <summary>
        /// Adds a <see cref="Single"/> column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        /// ///
        protected void AddSingleNotNullColumn(string columnName,
                                              float defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddSingleColumn(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a <see cref="Single"/> column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        private void AddSingleColumn(string columnName,
                                     float? defaultValue = null,
                                     bool allowDbNull = true)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddColumn(columnName, Type.GetType("System.Single", false, true), defaultValue, allowDbNull);
        }

        /// <summary>
        /// Adds a date and time column that allows null values.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddDateTimeNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddDateTimeColumn(columnName);
        }

        /// <summary>
        /// Adds a <see cref="DateTime"/> column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddDateTimeNullColumn(string columnName,
                                             DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddDateTimeColumn(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a date and time column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddDateTimeNotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddDateTimeColumn(columnName, null, false);
        }

        /// <summary>
        /// Adds a date and time column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddDateTimeNotNullColumn(string columnName,
                                                DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddDateTimeColumn(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a date and time column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        private void AddDateTimeColumn(string columnName,
                                       DateTime? defaultValue = null,
                                       bool allowDbNull = true)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddColumn(columnName, Type.GetType("System.DateTime", false, true), defaultValue, allowDbNull);
        }

        /// <summary>
        /// Adds a 16-bit signed integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddInt16NullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt16Column(columnName);
        }

        /// <summary>
        /// Adds a 16-bit signed integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddInt16NullColumn(string columnName,
                                          short defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt16Column(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a 16-bit signed integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddInt16NotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt16Column(columnName, null, false);
        }

        /// <summary>
        /// Adds a 16-bit signed integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddInt16NotNullColumn(string columnName, short defaultValue)
        {
            AddInt16Column(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a 16-bit signed integer column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="autoIncrement">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        /// <param name="readOnly">Sets a value that indicates whether the column allows for changes as soon as a row has been added to the table.</param>
        /// <param name="unique">Set a value that indicates whether the values in each row of the column must be unique.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddInt16Column(string columnName,
                                      short? defaultValue = null,
                                      bool allowDbNull = true,
                                      bool autoIncrement = false,
                                      long autoIncrementSeed = 0,
                                      long autoIncrementStep = 1,
                                      bool readOnly = false,
                                      bool unique = false)
        {
            AddColumn(columnName, Type.GetType("System.Int16", false, true), defaultValue, allowDbNull, autoIncrement,
                       autoIncrementSeed, autoIncrementStep, null, readOnly, unique);
        }

        /// <summary>
        /// Adds a 32-bit signed integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddInt32NullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt32Column(columnName);
        }

        /// <summary>
        /// Adds a 32-bit signed integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddInt32NullColumn(string columnName,
                                          int defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt32Column(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a 32-bit signed integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddInt32NotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt32Column(columnName, null, false);
        }

        /// <summary>
        /// Adds a 32-bit signed integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddInt32NotNullColumn(string columnName,
                                             int defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt32Column(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds an autoincremented 32-bit signed integer columnto a data table.  The increment seed and step values 
        /// will be set to 1 and 1, respectively, and the column will be set to read only and unique.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddInt32AutoIncrementColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt32Column(columnName, null, false, true, 1, 1, true, true);
        }

        /// <summary>
        /// Adds an autoincremented 32-bit signed integer columnto a data table.  The column will be set to
        /// read only and unique.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        protected void AddInt32AutoIncrementColumn(string columnName,
                                                   long autoIncrementSeed,
                                                   long autoIncrementStep)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt32Column(columnName, null, false, true, autoIncrementSeed, autoIncrementSeed, true, true);
        }

        /// <summary>
        /// Adds a 32-bit signed integer column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="autoIncrement">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        /// <param name="readOnly">Sets a value that indicates whether the column allows for changes as soon as a row has been added to the table.</param>
        /// <param name="unique">Set a value that indicates whether the values in each row of the column must be unique.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddInt32Column(string columnName,
                                      int? defaultValue = null,
                                      bool allowDbNull = true,
                                      bool autoIncrement = false,
                                      long autoIncrementSeed = 0,
                                      long autoIncrementStep = 1,
                                      bool readOnly = false,
                                      bool unique = false)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddColumn(columnName, Type.GetType("System.Int32", false, true), defaultValue, allowDbNull, autoIncrement,
                       autoIncrementSeed, autoIncrementStep, null, readOnly, unique);
        }

        /// <summary>
        /// Adds a 64-bit signed integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddInt64NullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt64Column(columnName);
        }

        /// <summary>
        /// Adds a 64-bit signed integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddInt64NullColumn(string columnName,
                                          long defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt64Column(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a 64-bit signed integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddInt64NotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt64Column(columnName, null, false);
        }

        /// <summary>
        /// Adds a 64-bit signed integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        public void AddInt64NotNullColumn(string columnName,
                                          long defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddInt64Column(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a 64-bit signed integer column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="autoIncrement">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        /// <param name="readOnly">Sets a value that indicates whether the column allows for changes as soon as a row has been added to the table.</param>
        /// <param name="unique">Set a value that indicates whether the values in each row of the column must be unique.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        public void AddInt64Column(string columnName,
                                   long? defaultValue = null,
                                   bool allowDbNull = true,
                                   bool autoIncrement = false,
                                   long autoIncrementSeed = 0,
                                   long autoIncrementStep = 1,
                                   bool readOnly = false,
                                   bool unique = false)
        {
            AddColumn(columnName, Type.GetType("System.Int64", false, true), defaultValue, allowDbNull,
                       autoIncrement, autoIncrementSeed, autoIncrementStep, null, readOnly, unique);
        }

        /// <summary>
        /// Adds a 16-bit unsigned integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddUInt16NullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt16Column(columnName);
        }

        /// <summary>
        /// Adds a 16-bit unsigned integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddUInt16NullColumn(string columnName,
                                           ushort defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt16Column(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a 16-bit unsigned integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddUInt16NotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt16Column(columnName, null, false);
        }

        /// <summary>
        /// Adds a 16-bit unsigned integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddUInt16NotNullColumn(string columnName,
                                              ushort defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt16Column(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a 16-bit unsigned integer column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="autoIncrement">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        /// <param name="readOnly">Sets a value that indicates whether the column allows for changes as soon as a row has been added to the table.</param>
        /// <param name="unique">Set a value that indicates whether the values in each row of the column must be unique.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddUInt16Column(string columnName,
                                       ushort? defaultValue = null,
                                       bool allowDbNull = true,
                                       bool autoIncrement = false,
                                       long autoIncrementSeed = 0,
                                       long autoIncrementStep = 1,
                                       bool readOnly = false,
                                       bool unique = false)
        {
            AddColumn(columnName, Type.GetType("System.UInt16", false, true), defaultValue, allowDbNull,
                       autoIncrement, autoIncrementSeed, autoIncrementStep, null, readOnly, unique);
        }

        /// <summary>
        /// Adds a 32-bit unsigned integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddUInt32NullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt32Column(columnName);
        }

        /// <summary>
        /// Adds a 32-bit unsigned integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddUInt32NullColumn(string columnName,
                                           uint defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt32Column(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a 32-bit unsigned integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddUInt32NotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt32Column(columnName, null, false);
        }

        /// <summary>
        /// Adds a 32-bit unsigned integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddUInt32NotNullColumn(string columnName,
                                              uint defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt32Column(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds an auto-incremented 32-bit unsigned integer column to a data table.  The increment seed and step values 
        /// will be set to 1 and 1, respectively, and the column will be set to read only and unique.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddUInt32AutoIncrementColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt32Column(columnName, null, false, true, 1, 1, true, true);
        }

        /// <summary>
        /// Adds an auto-incremented 32-bit unsigned integer column to a data table.  The column will be set to
        /// read only and unique.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        protected void AddUInt32AutoIncrementColumn(string columnName,
                                                    long autoIncrementSeed,
                                                    long autoIncrementStep)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt32Column(columnName, null, false, true, autoIncrementSeed, autoIncrementSeed, true, true);
        }

        /// <summary>
        /// Adds a 32-bit unsigned integer column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="autoIncrement">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        /// <param name="readOnly">Sets a value that indicates whether the column allows for changes as soon as a row has been added to the table.</param>
        /// <param name="unique">Set a value that indicates whether the values in each row of the column must be unique.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddUInt32Column(string columnName,
                                       uint? defaultValue = null,
                                       bool allowDbNull = true,
                                       bool autoIncrement = false,
                                       long autoIncrementSeed = 0,
                                       long autoIncrementStep = 1,
                                       bool readOnly = false,
                                       bool unique = false)
        {
            AddColumn(columnName, Type.GetType("System.UInt32", false, true), defaultValue, allowDbNull,
                       autoIncrement, autoIncrementSeed, autoIncrementStep, null, readOnly, unique);
        }

        /// <summary>
        /// Adds a 64-bit unsigned integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddUInt64NullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt64Column(columnName);
        }

        /// <summary>
        /// Adds a 64-bit unsigned integer column that allows null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddUInt64NullColumn(string columnName,
                                           ulong defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt64Column(columnName, defaultValue);
        }

        /// <summary>
        /// Adds a 64-bit unsigned integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected void AddUInt64NotNullColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt64Column(columnName, null, false);
        }

        /// <summary>
        /// Adds a 64-bit unsigned integer column that does not allow null values to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddUInt64NotNullColumn(string columnName,
                                              ulong defaultValue)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(columnName));

            AddUInt64Column(columnName, defaultValue, false);
        }

        /// <summary>
        /// Adds a 64-bit unsigned integer column to a data table.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="autoIncrement">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        /// <param name="readOnly">Sets a value that indicates whether the column allows for changes as soon as a row has been added to the table.</param>
        /// <param name="unique">Set a value that indicates whether the values in each row of the column must be unique.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        private void AddUInt64Column(string columnName,
                                     ulong? defaultValue = null,
                                     bool allowDbNull = true,
                                     bool autoIncrement = false,
                                     long autoIncrementSeed = 0,
                                     long autoIncrementStep = 1,
                                     bool readOnly = false,
                                     bool unique = false)
        {
            AddColumn(columnName, Type.GetType("System.UInt64", false, true), defaultValue, allowDbNull,
                       autoIncrement, autoIncrementSeed, autoIncrementStep, null, readOnly, unique);
        }

        /// <summary>
        /// Adds a DataColumn to DataTable instance.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="dataType">Type of the data stored in the column.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="autoIncrement">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        /// <param name="caption">Sets the caption for the column.</param>
        /// <param name="readOnly">Sets a value that indicates whether the column allows for changes as soon as a row has been added to the table.</param>
        /// <param name="unique">Set a value that indicates whether the values in each row of the column must be unique.</param>
        /// <param name="maxLength">Sets the maximum length of a text column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected void AddColumn(string columnName,
                                 Type dataType,
                                 object defaultValue = null,
                                 bool allowDbNull = true,
                                 bool autoIncrement = false,
                                 long autoIncrementSeed = 0,
                                 long autoIncrementStep = 1,
                                 string caption = null,
                                 bool readOnly = false,
                                 bool unique = false,
                                 int? maxLength = null)
        {
            var dataColumn = CreateColumn(columnName, dataType, defaultValue, allowDbNull, autoIncrement,
                                                  autoIncrementSeed, autoIncrementStep, caption, readOnly, unique);


            Debug.Assert(dataColumn != null, "dataColumn != null");
            AddColumn(dataColumn);
        }

        /// <summary>
        /// Adds a DataColumn to DataTable instance.
        /// </summary>
        /// <param name="dataColumn">DataColumn instance.</param>
        protected virtual void AddColumn(DataColumn dataColumn)
        {
            if (dataColumn == null)
                throw new ArgumentNullException(nameof(dataColumn));
            
            if (CheckIfColumnExist(dataColumn.ColumnName))
                return;
            
            GuardDataTableExist();
            InternalDataTable.Columns.Add(dataColumn);
        }

        /// <summary>
        /// Creates a new DataColumn instance.
        /// </summary>
        /// <param name="columnName">A string that represents the name of the column to be created.</param>
        /// <param name="dataType">A supported DataType.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="autoIncrement">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        /// <param name="caption">Sets the caption for the column.</param>
        /// <param name="readOnly">Sets a value that indicates whether the column allows for changes as soon as a row has been added to the table.</param>
        /// <param name="unique">Set a value that indicates whether the values in each row of the column must be unique.</param>
        /// <param name="maxLength">Sets the maximum length of a text column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        /// <returns>DataColumn instance.</returns>
        protected DataColumn CreateColumn(string columnName,
                                          Type dataType,
                                          object defaultValue = null,
                                          bool allowDbNull = true,
                                          bool autoIncrement = false,
                                          long autoIncrementSeed = 0,
                                          long autoIncrementStep = 1,
                                          string caption = null,
                                          bool readOnly = false,
                                          bool unique = false,
                                          int? maxLength = null)
        {
            var dataColumn = new DataColumn(columnName, dataType);

            ConfigureColumn(dataColumn, defaultValue, allowDbNull, autoIncrement, autoIncrementSeed, autoIncrementStep,
                             caption, readOnly, unique, maxLength);
            return dataColumn;
        }

        /// <summary>
        /// Configures the DataColumn instance with values.
        /// </summary>
        /// <param name="dataColumn">DataColumn instance.</param>
        /// <param name="allowDbNull">Sets a value that indicates whether null values are allowed in this column for rows that belong to the table.</param>
        /// <param name="autoIncrement">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementSeed">Sets the starting value for a column that has its AutoIncrement property set to true.</param>
        /// <param name="autoIncrementStep">Sets the increment used by a column with its AutoIncrement property set to true.</param>
        /// <param name="caption">Sets the caption for the column.</param>
        /// <param name="readOnly">Sets a value that indicates whether the column allows for changes as soon as a row has been added to the table.</param>
        /// <param name="unique">Set a value that indicates whether the values in each row of the column must be unique.</param>
        /// <param name="maxLength">Sets the maximum length of a text column.</param>
        /// <param name="defaultValue">Sets the default value for the column when you are creating new rows.</param>
        protected virtual void ConfigureColumn(DataColumn dataColumn,
                                               object defaultValue = null,
                                               bool allowDbNull = true,
                                               bool autoIncrement = false,
                                               long autoIncrementSeed = 0,
                                               long autoIncrementStep = 1,
                                               string caption = null,
                                               bool readOnly = false,
                                               bool unique = false,
                                               int? maxLength = null)
        {
            dataColumn.AllowDBNull = allowDbNull;
            dataColumn.AutoIncrement = autoIncrement;
            dataColumn.AutoIncrementSeed = autoIncrementSeed;
            dataColumn.AutoIncrementStep = autoIncrementStep;
            dataColumn.ReadOnly = readOnly;
            dataColumn.Unique = unique;

            if (string.IsNullOrEmpty(caption) == false)
                dataColumn.Caption = caption;
            if (maxLength != null)
                dataColumn.MaxLength = maxLength.GetValueOrDefault();
            if (defaultValue != null)
                dataColumn.DefaultValue = defaultValue;
        }

        /// <summary>
        /// Checks whether the collection contains a column with the specified name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns><b>true</b> if a column exists with this name; otherwise, <b>false</b>.</returns>
        private bool CheckIfColumnExist(string columnName)
        {
            GuardDataTableExist();
            return InternalDataTable.Columns.Contains(columnName);
        }

        /// <summary>
        /// Determines whether the internal <see cref="DataTable"/> exists.
        /// </summary>
        private void GuardDataTableExist()
        {
            if (InternalDataTable == null)
                throw new InvalidOperationException(Resources.ExceptionDataTableInvalid);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            InternalDataTable?.Dispose();
        }

        #endregion
    }
}
