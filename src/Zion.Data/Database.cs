// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace Zion.Data
{
    /// <summary>
    /// Handles the low-level access to the database.
    /// </summary>
    public abstract class Database
    {
        private readonly DbProviderFactory _dbProvider;
        private readonly DbConnectionStringBuilder _connectionStringBuilder;
        private readonly int _commandTimeout;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="dbProvider">A <see cref="DbProviderFactory"/></param>
        /// <param name="connectionStringBuilder">A <see cref="DbConnectionStringBuilder"/>.</param>
        /// <param name="commandTimeout">The timeout value for a <see cref="IDbCommand"/>.</param>
        protected Database(DbProviderFactory dbProvider,
                           // String connectionString,
                           DbConnectionStringBuilder connectionStringBuilder,
                           int commandTimeout = 30)
        {
            if (commandTimeout < 0)
                throw new ArgumentException(Resources.ExceptionValueLessThanZero, nameof(commandTimeout));

            _dbProvider = dbProvider ?? throw new ArgumentNullException(nameof(dbProvider));
            _connectionStringBuilder = connectionStringBuilder ?? throw new ArgumentNullException(nameof(connectionStringBuilder));
            _commandTimeout = commandTimeout;
        }

        /// <summary>
        /// Gets a SQL text <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="sqlText">SQL text.</param>
        /// <returns>A SQL text <see cref="IDbCommand"/>.</returns>
        public DbCommand GetSqlTextCommand(string sqlText)
        {
            if (string.IsNullOrEmpty(sqlText))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(sqlText));

            return CreateCommandByType(sqlText, CommandType.Text);
        }

        /// <summary>
        /// Gets a stored procedure <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="storedProcName">Name of the stored procedure.</param>
        /// <returns>A stored procedure <see cref="IDbCommand"/>.</returns>
        public DbCommand GetStoredProcCommand(string storedProcName)
        {
            if (string.IsNullOrEmpty(storedProcName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(storedProcName));

            return CreateCommandByType(storedProcName, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Creates a <see cref="IDbCommand"/> instance by
        /// a <see cref="CommandType"/>.
        /// </summary>
        /// <param name="commandText">The command to run against the data source.</param>
        /// <param name="commandType">The <see cref="CommandType"/>.</param>
        /// <returns>A <see cref="IDbCommand"/>.</returns>
        private DbCommand CreateCommandByType(string commandText,
                                              CommandType commandType)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(commandText));

            Debug.Assert(_dbProvider != null, "m_dbProvider != null");
            var command = _dbProvider.CreateCommand();

            Debug.Assert(command != null, "command != null");
            Debug.Assert(_commandTimeout >= 0, "m_commandTimeout >= 0");

            command.CommandType = commandType;
            command.CommandText = commandText;
            command.CommandTimeout = _commandTimeout;
            return command;
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
                                   DbType dbType,
                                   object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

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
                                   DbType dbType,
                                   int size,
                                   object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(parameterName));

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
                                    DbType dbType,
                                    object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(parameterName));

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
                                    DbType dbType)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(parameterName));

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
                                    DbType dbType,
                                    int size)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(parameterName));

            AddParameter(command, parameterName, dbType, size, ParameterDirection.Output, true,
                          null, DataRowVersion.Default, null);
        }

        /// <summary>
        /// Adds a <see cref="IDataParameter"/> to a <see cref="IDbCommand"/> instance.
        /// </summary>
        /// <param name="command">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="parameterName">The name of a <see cref="IDataParameter"/>.</param>
        /// <param name="dbType">The <see cref="DbType"/> of a <see cref="IDataParameter"/>.</param>
        /// <param name="value">The value of a <see cref="IDataParameter"/>.</param>
        public void AddInOutParameter(IDbCommand command,
                                      string parameterName,
                                      DbType dbType,
                                      object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            AddParameter(command, parameterName, dbType, 0, ParameterDirection.InputOutput, true,
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
        public void AddInOutParameter(IDbCommand command,
                                      string parameterName,
                                      DbType dbType,
                                      int size,
                                      object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(parameterName));

            AddParameter(command, parameterName, dbType, size, ParameterDirection.InputOutput, true,
                          null, DataRowVersion.Default, value);
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
                                         DbType dbType,
                                         int size,
                                         ParameterDirection parameterDirection,
                                         bool isNullable,
                                         string sourceColumn,
                                         DataRowVersion dataRowVersion,
                                         object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            IDataParameter dataParameter = CreateParameter(parameterName, dbType, size, parameterDirection, isNullable,
                                                            sourceColumn, dataRowVersion, value);
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
                                                           DbType dbType,
                                                           int size,
                                                           ParameterDirection parameterDirection,
                                                           bool isNullable,
                                                           string sourceColumn,
                                                           DataRowVersion dataRowVersion,
                                                           object value)
        {
            if (!(CreateParameter(parameterName) is DbParameter dbParameter))
                throw new InvalidOperationException();

            ConfigureParameter(dbParameter, dbType, size, parameterDirection, isNullable,
                                sourceColumn, dataRowVersion, value);
            return dbParameter;
        }

        /// <summary>
        /// Creates a <see cref="IDbDataParameter"/> instance.
        /// </summary>
        /// <param name="parameterName">Parameter name of a <see cref="IDbDataParameter"/>.</param>
        /// <returns>A <see cref="IDbDataParameter"/>.</returns>
        protected virtual IDbDataParameter CreateParameter(string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, nameof(parameterName));

            IDbDataParameter parameter = _dbProvider.CreateParameter();

            if (parameter == null)
                return null;

            parameter.ParameterName = parameterName;
            return parameter;
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
        protected virtual void ConfigureParameter(DbParameter parameter,
                                                  DbType dbType,
                                                  int size,
                                                  ParameterDirection parameterDirection,
                                                  bool isNullable,
                                                  string sourceColumn,
                                                  DataRowVersion dataRowVersion,
                                                  object value,
                                                  byte precision = 0,
                                                  byte scale = 0)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            parameter.DbType = dbType;
            parameter.Direction = parameterDirection;
            parameter.IsNullable = isNullable;
            ((IDbDataParameter)parameter).Precision = precision;
            ((IDbDataParameter)parameter).Scale = scale;
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
        protected virtual string FormatParameterName(string parameterName)
        {
            return parameterName;
        }

        /// <summary>
        /// Gets a parameter value.
        /// </summary>
        /// <param name="dbCommand">The command that contains the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        public virtual object GetParameterValue(DbCommand dbCommand,
                                                string name)
        {
            if (dbCommand == null)
                throw new ArgumentNullException(nameof(dbCommand));

            return dbCommand.Parameters[FormatParameterName(name)].Value;
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
        /// </summary>
        /// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
        protected abstract void DeriveParameters(DbCommand discoveryCommand);

        /// <summary>
        /// Discovers the <see cref="IDataParameter"/>s for a <see cref="DbCommand"/>.
        /// </summary>
        /// <param name="command"></param>
        public void DiscoverParameters(DbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            using (var wrapper = GetOpenConnection())
            {
                using (var discoveryCommand = CreateCommandByType(command.CommandText, command.CommandType))
                {
                    discoveryCommand.Connection = wrapper.Connection as DbConnection;
                    DeriveParameters(discoveryCommand);

                    var parameters = from IDataParameter parameter in discoveryCommand.Parameters select (IDataParameter)((ICloneable)parameter).Clone();
                    command.Parameters.AddRange(parameters.ToArray());

                    //foreach (IDataParameter parameter in discoveryCommand.Parameters)
                    //{
                    //    var cloneParameter = (IDataParameter)((ICloneable)parameter).Clone();
                    //    command.Parameters.Add(cloneParameter);
                    //}
                }
            }
        }


        /// <summary>
        /// Executes a sql statement.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(IDbCommand dbCommand)
        {
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));

            using (var wrapper = GetOpenConnection())
            {
                PrepareCommand(dbCommand, wrapper.Connection);
                return DoExecuteNonQuery(dbCommand);
            }
        }

        /// <summary>
        /// Executes a sql statement.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="dbConnection">A <see cref="IDbConnection"/>.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(IDbCommand dbCommand,
                                     IDbConnection dbConnection)
        {
            // Validate the argument(s).
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));

            // Add the transaction.
            PrepareCommand(dbCommand, dbConnection);
            return DoExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Executes a sql statement.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="dbTransaction">A <see cref="IDbTransaction"/>.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(IDbCommand dbCommand,
                                     IDbTransaction dbTransaction)
        {
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));
            if (dbTransaction == null) throw new ArgumentNullException(nameof(dbTransaction));
            
            PrepareCommand(dbCommand, dbTransaction);
            return DoExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Executes a sql statement.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <returns>The number of rows affected.</returns>
        protected virtual int DoExecuteNonQuery(IDbCommand dbCommand)
        {
            if (dbCommand == null)
                throw new ArgumentNullException(nameof(dbCommand));

            return dbCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets a <see cref="IDataReader"/>/
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="connection">A <see cref="IDbConnection"/> instance.</param>
        /// <param name="commandBehavior">A <see cref="CommandBehavior"/>.</param>
        /// <returns>A <see cref="IDataReader"/> instance.</returns>
        public IDataReader ExecuteReader(IDbCommand dbCommand,
                                         IDbConnection connection,
                                         CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            PrepareCommand(dbCommand, connection);
            return ExecuteReader(dbCommand, commandBehavior);
        }

        /// <summary>
        /// Gets a <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="commandBehavior">A <see cref="CommandBehavior"/>.</param>
        /// <returns>A <see cref="IDataReader"/> instance.</returns>
        public IDataReader ExecuteReader(IDbCommand dbCommand,
                                         CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            if (dbCommand == null)
                throw new ArgumentNullException(nameof(dbCommand));

            //// Gets a new and open connection.
            //using ( DatabaseConnection dbConnection = GetOpenConnection() )
            //{
            //    // Add the connection.
            //    PrepareCommand( dbCommand, dbConnection.Connection );
            //    return GetExecuteReader( dbCommand, commandBehavior );
            //}

            var dbConnection = GetOpenConnection();
            PrepareCommand(dbCommand, dbConnection.Connection);
            return GetExecuteReader(dbCommand, commandBehavior);
        }

        /// <summary>
        /// Gets a <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="commandBehavior">A <see cref="CommandBehavior"/>.</param>
        /// <returns>A <see cref="IDataReader"/> instance.</returns>
        protected virtual IDataReader GetExecuteReader(IDbCommand dbCommand,
                                                       CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            if (dbCommand == null)
                throw new ArgumentNullException(nameof(dbCommand));

            return dbCommand.ExecuteReader(commandBehavior);
        }

        /// <summary>
        /// Get a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/>.</param>
        /// <returns>A <see cref="DataSet"/>.</returns>
        public DataSet ExecuteDataSet(IDbCommand dbCommand)
        {
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));

            using (var dbConnection = GetOpenConnection())
            {
                Debug.Assert(dbConnection != null, "dbConnection != null");
                PrepareCommand(dbCommand, dbConnection.Connection);
                return GetDataSet(dbCommand);
            }
        }

        /// <summary>
        /// Get a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/>.</param>
        /// <returns>A <see cref="DataSet"/>.</returns>
        protected virtual DataSet GetDataSet(IDbCommand dbCommand)
        {
            if (dbCommand == null)
                throw new ArgumentNullException(nameof(dbCommand));

            Debug.Assert(_dbProvider != null, "m_dbProvider != null");
            using (DataAdapter dataAdapter = _dbProvider.CreateDataAdapter())
            {
                Debug.Assert(dataAdapter != null, "dataAdapter != null");
                ((IDbDataAdapter)dataAdapter).SelectCommand = dbCommand;

                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }

        /// <summary>
        /// Prepares a <see cref="IDbCommand"/> instance.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="transaction">A <see cref="IDbTransaction"/> instance.</param>
        protected static void PrepareCommand(IDbCommand dbCommand,
                                             IDbTransaction transaction)
        {
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            PrepareCommand(dbCommand, transaction.Connection);
            dbCommand.Transaction = transaction;
        }

        /// <summary>
        /// Prepares a <see cref="IDbCommand"/> instance.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="connection">A <see cref="IDbConnection"/> instance.</param>
        protected static void PrepareCommand(IDbCommand dbCommand,
                                             IDbConnection connection)
        {
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            dbCommand.Connection = connection;
        }

        /// <summary>
        /// Gets a new and open <see cref="IDbConnection"/> instance.
        /// </summary>
        /// <returns>A <see cref="IDbConnection"/> instance.</returns>
        public virtual DatabaseConnection GetOpenConnection()
        {
            var dbConnection = GetNewOpenConnection();

            if (dbConnection == null)
                throw new InvalidOperationException();

            return new DatabaseConnection(dbConnection);
        }

        /// <summary>
        /// Gets a new and open <see cref="IDbConnection"/> instance.
        /// </summary>
        /// <returns>A <see cref="IDbConnection"/> instance.</returns>
        public virtual IDbConnection GetNewOpenConnection()
        {
            var dbConnection = CreateConnection();

            if (dbConnection == null)
                throw new InvalidOperationException();

            dbConnection.Open();
            return dbConnection;
        }

        /// <summary>
        /// Gets a new <see cref="IDbConnection"/> instance.
        /// </summary>
        /// <returns>A new <see cref="IDbConnection"/>.</returns>
        public virtual IDbConnection CreateConnection()
        {
            var newConnection = _dbProvider.CreateConnection();

            if (newConnection == null)
                return null;

            newConnection.ConnectionString = _connectionStringBuilder.ConnectionString; //m_connectionString;
            return newConnection;
        }
    }
}
