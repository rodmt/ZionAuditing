// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using Zion.Data.Tests.NUnit.TestSupport;

namespace Zion.Data.Tests.NUnit
{
	[TestFixture]
	public class DatabaseParameterizedSqlTextFixture
	{
		private Database _database;

		[SetUp]
		public void Setup()
		{
			var connectionStringName = ConfigurationManager.AppSettings.Get("ConnectionStringName");
			var connectionData = ConfigurationManager.ConnectionStrings[connectionStringName];
			_database = new TestDatabase(connectionData.ConnectionString, SqlClientFactory.Instance);
		}

		[Test]
		public void ASqlTextCommandWithParametersShouldReturnDataSet()
		{
			// Arrange
			const string sqlText = "select * from [Northwind].[dbo].[Products] where [SupplierId] = @param1 and [ProductName] = @param2;";
			IDbCommand dbCommand = _database.GetSqlTextCommand(sqlText);
			_database.AddInParameter(dbCommand, "@param1", DbType.Int32, 1);
			_database.AddInParameter(dbCommand, "@param2", DbType.String, "Chang");

			// Act
			var dataSet = _database.ExecuteDataSet(dbCommand);

			// Assert
			Assert.That(1, Is.EqualTo(dataSet.Tables[0].Rows.Count));
		}

		[Test]
		//[ExpectedException(typeof(SqlException))]
		public void ASqlTextCommandWithNotEnoughParameterValuesShouldThrow()
		{
			// Arrange
			const string sqlText = "select * from [Northwind].[dbo].[Products] where [SupplierId] = @param1 and [ProductName] = @param2;";
			IDbCommand dbCommand = _database.GetSqlTextCommand(sqlText);
			_database.AddInParameter(dbCommand, "@param1", DbType.Int32, 1);

            // Act
            // Assert
            Assert.That((TestDelegate) (() => _database.ExecuteDataSet(dbCommand)), Throws.TypeOf<SqlException>());
		}

		[Test]
		//[ExpectedException(typeof(SqlException))]
		public void ASqlTextCommandWithTooManyParameterValuesShouldThrow()
		{
			// Arrange
			const string sqlText = "select * from [Northwind].[dbo].[Products] where [SupplierId] = @param1 and [ProductName] = @param2;";
			IDbCommand dbCommand = _database.GetSqlTextCommand(sqlText);
			_database.AddInParameter(dbCommand, "@param1", DbType.Int32, 1);
			_database.AddInParameter(dbCommand, "@param2", DbType.String, "Chang");
			_database.AddInParameter(dbCommand, "@param2", DbType.String, "Chang2");

            // Act
            // Assert
            Assert.That((TestDelegate) (() => _database.ExecuteDataSet(dbCommand)), Throws.TypeOf<SqlException>());
        }
	}
}
