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
	public class SqlDatabaseFixture
	{
		[Test]
		public void ConnectionTest()
		{
			// Arrange
			var connectionStringName = ConfigurationManager.AppSettings.Get("ConnectionStringName");
			var connectionData = ConfigurationManager.ConnectionStrings[connectionStringName];
			SqlDatabase sqlDatabase = null;
			IDbConnection dbConnection = null;
			try
			{
				// Act
				sqlDatabase = new SqlDatabase(connectionData.ConnectionString);
				dbConnection = sqlDatabase.CreateConnection();
				dbConnection.Open();

				// Assert
				Assert.That(dbConnection, Is.Not.Null);
				Assert.That(dbConnection, Is.TypeOf<SqlConnection>());
				Assert.That(dbConnection.State, Is.EqualTo(ConnectionState.Open));

			}
			finally
			{
				// Release resources.
				dbConnection?.Close();
				dbConnection?.Dispose();
			}
		}
	}
}
