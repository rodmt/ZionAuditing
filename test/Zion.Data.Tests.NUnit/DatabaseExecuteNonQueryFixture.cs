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
    public class DatabaseExecuteNonQueryFixture
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
        public void AnInsertShouldReturnNumberOfRowsAffected()
        {
            // Arrange
            const string sqlText = "insert [Northwind].[dbo].[Region]( [RegionId], [RegionDescription] ) values ( @param1, @param2 );";
            IDbCommand dbCommand = _database.GetSqlTextCommand(sqlText);
            _database.AddInParameter(dbCommand, "@param1", DbType.Int32, 7);
            _database.AddInParameter(dbCommand, "@param2", DbType.String, "TestRegion");
            int rowsAffected;
            IDbConnection dbConnection = null;
            IDbTransaction dbTransaction = null;

            try
            {
                // Act
                dbConnection = _database.CreateConnection();
                dbConnection.Open();
                dbTransaction = dbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                rowsAffected = _database.ExecuteNonQuery(dbCommand, dbTransaction);
            }
            finally
            {
	            dbTransaction?.Rollback();
	            dbTransaction?.Dispose();

	            dbConnection?.Close();
	            dbConnection?.Dispose();
            }

            // Assert
			Assert.That(rowsAffected,Is.EqualTo(1));
        }
    }
}
