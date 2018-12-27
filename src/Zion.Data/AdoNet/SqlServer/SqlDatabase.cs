// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Zion.Data.AdoNet.SqlServer
{
    public class SqlDatabase : Database
    {
        private const char _ParameterToken = '@';

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="connectionString">A connection string.</param>
        /// <param name="commandTimeout">A command timeout.</param>
        public SqlDatabase(string connectionString,
                           int commandTimeout = 60)
            : base(SqlClientFactory.Instance, new SqlConnectionStringBuilder(connectionString), commandTimeout)
        {
        }

        /// <summary>
        /// Adds a <see cref="IDataParameter"/> to a <see cref="IDbCommand"/> instance.
        /// </summary>
        /// <param name="command">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="parameterName">The name of a <see cref="IDataParameter"/>.</param>
        /// <param name="dbType">The <see cref="DbType"/> of a <see cref="IDataParameter"/>.</param>
        /// <param name="value">The value of a <see cref="IDataParameter"/>.</param>
        public void AddInParameter(IDbCommand command,
                                   string parameterName,
                                   SqlDbType dbType,
                                   object value)
        {
            // Validate the argument(s)
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            // Create the parameter.
            AddParameter(command, parameterName, dbType, 0, ParameterDirection.Input, true,
                          string.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a <see cref="IDataParameter"/> to a <see cref="IDbCommand"/> instance.
        /// </summary>
        /// <param name="command">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="parameterName">The name of a <see cref="IDataParameter"/>.</param>
        /// <param name="dbType">The <see cref="DbType"/> of a <see cref="IDataParameter"/>.</param>
        /// <param name="size">The size of a <see cref="IDataParameter"/>.</param>
        /// <param name="value">The value of a <see cref="IDataParameter"/>.</param>
        public void AddInParameter(IDbCommand command,
                                   string parameterName,
                                   SqlDbType dbType,
                                   int size,
                                   object value)
        {
            // Validate the argument(s)
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(parameterName));

            // Create the parameter.
            AddParameter(command, parameterName, dbType, size, ParameterDirection.Input, true,
                          null, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a <see cref="IDataParameter"/> to a <see cref="IDbCommand"/> instance.
        /// </summary>
        /// <param name="command">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="parameterName">The name of a <see cref="IDataParameter"/>.</param>
        /// <param name="dbType">The <see cref="DbType"/> of a <see cref="IDataParameter"/>.</param>
        /// <param name="value">The value of a <see cref="IDataParameter"/>.</param>
        public void AddOutParameter(IDbCommand command,
                                    string parameterName,
                                    SqlDbType dbType,
                                    object value)
        {
            // Validate the argument(s)
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(parameterName));

            // Create the parameter.
            AddParameter(command, parameterName, dbType, 0, ParameterDirection.Output, true,
                          string.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a <see cref="IDataParameter"/> to a <see cref="IDbCommand"/> instance.
        /// </summary>
        /// <param name="command">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="parameterName">The name of a <see cref="IDataParameter"/>.</param>
        /// <param name="dbType">The <see cref="DbType"/> of a <see cref="IDataParameter"/>.</param>
        public void AddOutParameter(IDbCommand command,
                                    string parameterName,
                                    SqlDbType dbType)
        {
            // Validate the argument(s)
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(parameterName));

            // Create the parameter.
            AddParameter(command, parameterName, dbType, 0, ParameterDirection.Output, true,
                          null, DataRowVersion.Default, null);
        }

        /// <summary>
        /// Adds a <see cref="IDataParameter"/> to a <see cref="IDbCommand"/> instance.
        /// </summary>
        /// <param name="command">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="parameterName">The name of a <see cref="IDataParameter"/>.</param>
        /// <param name="dbType">The <see cref="DbType"/> of a <see cref="IDataParameter"/>.</param>
        /// <param name="size">The size of a <see cref="IDataParameter"/>.</param>
        public void AddOutParameter(IDbCommand command,
                                    string parameterName,
                                    SqlDbType dbType,
                                    int size)
        {
            // Validate the argument(s)
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(parameterName));

            // Create the parameter.
            AddParameter(command, parameterName, dbType, size, ParameterDirection.Output, true,
                          null, DataRowVersion.Default, null);
        }

        /// <summary>
        /// Adds a <see cref="IDataParameter"/> to a <see cref="IDbCommand"/> instance.
        /// </summary>
        /// <param name="command">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="parameterName">The name of a <see cref="IDataParameter"/>.</param>
        /// <param name="dbType">The <see cref="DbType"/> of a <see cref="IDataParameter"/>.</param>
        /// <param name="size">The size of a <see cref="IDataParameter"/>.</param>
        /// <param name="parameterDirection">The <see cref="ParameterDirection"/> of a <see cref="IDataParameter"/>.</param>
        /// <param name="isNullable">Indicates whether a parameter accepts null values.</param>
        /// <param name="sourceColumn">Name of the source column.</param>
        /// <param name="dataRowVersion">A <see cref="DataRowVersion"/>.</param>
        /// <param name="value">The value of a <see cref="IDataParameter"/>.</param>
        public virtual void AddParameter(IDbCommand command,
                                         string parameterName,
                                         SqlDbType dbType,
                                         int size,
                                         ParameterDirection parameterDirection,
                                         bool isNullable,
                                         string sourceColumn,
                                         DataRowVersion dataRowVersion,
                                         object value)
        {
            // Validate the argument(s)
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            // Create the parameter.
            IDataParameter dataParameter = CreateParameter(parameterName, dbType, size, parameterDirection, isNullable,
                                                            sourceColumn, dataRowVersion, value);
            // Add the parameter.
            command.Parameters.Add(dataParameter);
        }

        /// <summary>
        /// Creates a <see cref="IDbDataParameter"/> instance.
        /// </summary>
        /// <param name="parameterName">Parameter name of a <see cref="IDbDataParameter"/>.</param>
        /// <param name="dbType">A <see cref="DbType"/>.</param>
        /// <param name="size">Maximum size, in bytes, of the data within the column.</param>
        /// <param name="parameterDirection">A <see cref="ParameterDirection"/>.</param>
        /// <param name="isNullable">Indicates whether a parameter accepts null values.</param>
        /// <param name="sourceColumn">Name of the source column.</param>
        /// <param name="dataRowVersion">A <see cref="DataRowVersion"/>.</param>
        /// <param name="value">The value of parameter.</param>
        /// <returns>A <see cref="IDbDataParameter"/>.</returns>
        protected virtual IDbDataParameter CreateParameter(string parameterName,
                                                           SqlDbType dbType,
                                                           int size,
                                                           ParameterDirection parameterDirection,
                                                           bool isNullable,
                                                           string sourceColumn,
                                                           DataRowVersion dataRowVersion,
                                                           object value)
        {
            // Create the parameter.
            var dbParameter = CreateParameter(parameterName) as SqlParameter;

            // Check if the parameter is null.
            if (dbParameter == null)
                throw new InvalidOperationException();

            // Configure the parameter.
            ConfigureParameter(dbParameter, dbType, size, parameterDirection, isNullable,
                                sourceColumn, dataRowVersion, value);
            return dbParameter;
        }

        /// <summary>
        /// Configures a <see cref="DbParameter"/>.
        /// </summary>
        /// <param name="parameter">A <see cref="DbParameter"/> instance.</param>
        /// <param name="dbType">A <see cref="DbType"/>.</param>
        /// <param name="size">Maximum size, in bytes, of the data within the column.</param>
        /// <param name="parameterDirection">A <see cref="ParameterDirection"/>.</param>
        /// <param name="isNullable">Indicates whether a parameter accepts null values.</param>
        /// <param name="sourceColumn">Name of the source column.</param>
        /// <param name="dataRowVersion">A <see cref="DataRowVersion"/>.</param>
        /// <param name="value">The value of parameter.</param>
        /// <param name="precision">The maximum number of digits used to represent the Value property of a data provider Parameter object.</param>
        /// <param name="scale">The number of decimal places to which Value is resolved.</param>
        protected virtual void ConfigureParameter(SqlParameter parameter,
                                                  SqlDbType dbType,
                                                  int size,
                                                  ParameterDirection parameterDirection,
                                                  bool isNullable,
                                                  string sourceColumn,
                                                  DataRowVersion dataRowVersion,
                                                  object value,
                                                  byte precision = 0,
                                                  byte scale = 0)
        {
            // Validate the argument(s)
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            // Set the properties.
            parameter.SqlDbType = dbType;
            parameter.Direction = parameterDirection;
            parameter.IsNullable = isNullable;
            parameter.Precision = precision;
            parameter.Scale = scale;
            parameter.Size = size;
            parameter.SourceColumn = sourceColumn;
            parameter.SourceVersion = dataRowVersion;
            parameter.Value = value ?? DBNull.Value;
        }

        /// <summary>
        /// Formats the parameter name.
        /// </summary>
        /// <param name="parameterName">A parameter name.</param>
        /// <returns>A Formatted parameter name.</returns>
        protected override string FormatParameterName(string parameterName)
        {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));

            // Check if the parameter has valid format token.
            return parameterName[0] != _ParameterToken ? parameterName.Insert(0, new string(_ParameterToken, 1)) : parameterName;
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
        /// </summary>
        /// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
        /// <remarks>The <see cref="DbCommand"/> must be a <see cref="SqlCommand"/> instance.</remarks>
        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
            SqlCommandBuilder.DeriveParameters((SqlCommand)discoveryCommand);
        }
    }
}
