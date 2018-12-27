// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using Zion.Data.AdoNet.SqlServer;

namespace Zion.Data.Tests.NUnit.Sql
{
	[TestFixture]
	public class SqlDatabaseParameterizedSqlTextFixture
	{
		private SqlDatabase _sqlDatabase;

		[SetUp]
		public void Setup()
		{
			var connectionStringName = ConfigurationManager.AppSettings.Get("ConnectionStringName");
			var connectionData = ConfigurationManager.ConnectionStrings[connectionStringName];
			_sqlDatabase = new SqlDatabase(connectionData.ConnectionString);
		}

        [Test]
	    public void CanDiscoverParameters()
	    {
            using (var connection = _sqlDatabase.CreateConnection())
            {
                connection.Open();
                using (var command = _sqlDatabase.GetStoredProcCommand("dbo.Employee Sales by Country"))
                {
                    _sqlDatabase.DiscoverParameters(command);

                    Assert.That(command.Parameters.Count, Is.EqualTo(3));
                }
            }
	    }

		[Test]
		public void ASqlTextCommandWithParametersShouldReturnDataSet()
		{
			// Arrange
			const string sqlText = "select * from [Northwind].[dbo].[Products] where [SupplierId] = @param1 and [ProductName] = @param2;";
			IDbCommand dbCommand = _sqlDatabase.GetSqlTextCommand(sqlText);
			_sqlDatabase.AddInParameter(dbCommand, "@param1", SqlDbType.Int, 1);
			_sqlDatabase.AddInParameter(dbCommand, "@param2", SqlDbType.NVarChar, "Chang");

			// Act
			var dataSet = _sqlDatabase.ExecuteDataSet(dbCommand);

			// Assert
			Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1));
		}

		[Test]
		//[ExpectedException(typeof(SqlException))]
		public void ASqlTextCommandWithNotEnoughParameterValuesShouldThrow()
		{
			// Arrange
			const string sqlText = "select * from [Northwind].[dbo].[Products] where [SupplierId] = @param1 and [ProductName] = @param2;";
			IDbCommand dbCommand = _sqlDatabase.GetSqlTextCommand(sqlText);
			_sqlDatabase.AddInParameter(dbCommand, "@param1", SqlDbType.Int, 1);

            // Act
            // Assert
            Assert.That(() => _sqlDatabase.ExecuteDataSet(dbCommand), Throws.TypeOf<SqlException>());
        }

		[Test]
		//[ExpectedException(typeof(SqlException))]
		public void ASqlTextCommandWithTooManyParameterValuesShouldThrow()
		{
			// Arrange
			const string sqlText = "select * from [Northwind].[dbo].[Products] where [SupplierId] = @param1 and [ProductName] = @param2;";
			IDbCommand dbCommand = _sqlDatabase.GetSqlTextCommand(sqlText);
			_sqlDatabase.AddInParameter(dbCommand, "@param1", SqlDbType.Int, 1);
			_sqlDatabase.AddInParameter(dbCommand, "@param2", SqlDbType.NVarChar, "Chang");
			_sqlDatabase.AddInParameter(dbCommand, "@param2", SqlDbType.NVarChar, "Chang2");

            // Act
            // Assert
            Assert.That(() => _sqlDatabase.ExecuteDataSet(dbCommand), Throws.TypeOf<SqlException>());
        }
	}
}
