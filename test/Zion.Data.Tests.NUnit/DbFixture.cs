// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using Zion.Data.Tests.NUnit.TestSupport;

namespace Zion.Data.Tests.NUnit
{
	[TestFixture]
	public class DbFixture
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
		public void ADbReadShouldTransformDataRowsToSingleObject()
		{
			//// Arrange
			//const string sqlText = "select * from [Northwind].[dbo].[Products] where [SupplierId] = @param1 and [ProductName] = @param2;";
			//IDbCommand dbCommand = _database.GetSqlTextCommand(sqlText);
			//_database.AddInParameter(dbCommand, "@param1", DbType.Int32, 1);
			//_database.AddInParameter(dbCommand, "@param2", DbType.String, "Chang");

			//// Act
			//var dataSet = _database.ExecuteDataSet(dbCommand);
			//var products = Db.Read(dataSet.Tables[0].AsEnumerable(), DataRowMake).ToList();

			//// Assert
			//Assert.That(products, Is.Not.Null);
			//Assert.That(products.Count(), Is.EqualTo(1));
		}

		[Test]
		public void ADbReadShouldTransformDataRowsToObjectCollection()
		{
			//// Arrange
			//const string sqlText = "select * from [Northwind].[dbo].[Products];";
			//IDbCommand dbCommand = _database.GetSqlTextCommand(sqlText);

			//// Act
			//var dataSet = _database.ExecuteDataSet(dbCommand);
			//var products = Db.Read(dataSet.Tables[0].AsEnumerable(), DataRowMake).ToList();

			//// Assert
			//Assert.That(products, Is.Not.Null);
			//Assert.That(products.Count(), Is.EqualTo(77));
		}

		[Test]
		public void ADbReadShouldTransformDataRecordToSingleObject()
		{
			// Arrange
			const string sqlText = "select * from [Northwind].[dbo].[Products] where [SupplierId] = @param1 and [ProductName] = @param2;";
			IDbCommand dbCommand = _database.GetSqlTextCommand(sqlText);
			_database.AddInParameter(dbCommand, "@param1", DbType.Int32, 1);
			_database.AddInParameter(dbCommand, "@param2", DbType.String, "Chang");
			Product product = null;
			IDataReader dataReader = null;

			// Act
			try
			{
				dataReader = _database.ExecuteReader(dbCommand, CommandBehavior.SingleRow);
				product = Db.Read(dataReader, DataRecordMake, true);
			}
			finally
			{
				dataReader?.Close();
				dataReader?.Dispose();
			}

			// Assert
			Assert.That(product, Is.Not.Null);
		}

		[Test]
		public void ADbReadShouldTransformDataRecordToDefaultObject()
		{
			// Arrange
			const string sqlText = "select * from [Northwind].[dbo].[Products] where [SupplierId] = @param1 and [ProductName] = @param2;";
			IDbCommand dbCommand = _database.GetSqlTextCommand(sqlText);
			_database.AddInParameter(dbCommand, "@param1", DbType.Int32, 11111);
			_database.AddInParameter(dbCommand, "@param2", DbType.String, "Chang2");
			Product product;
			IDataReader dataReader = null;

			// Act
			try
			{
				dataReader = _database.ExecuteReader(dbCommand, CommandBehavior.SingleRow);
				product = Db.Read(dataReader, DataRecordMake, true);
			}
			finally
			{
				dataReader?.Close();
				dataReader?.Dispose();
			}

			// Assert
			Assert.That(product, Is.EqualTo(default(Product)));
		}

		[Test]
		public void ADbReadShouldTransformDataRecordTosToObjectCollection()
		{
			// Arrange
			const string sqlText = "select * from [Northwind].[dbo].[Products];";
			IDbCommand dbCommand = _database.GetSqlTextCommand(sqlText);

			// Act
			var products = Db.Read(_database.ExecuteReader(dbCommand), DataRecordMake).ToList();

			// Assert
			Assert.That(products, Is.Not.Null);
			Assert.That(products.Count(), Is.EqualTo(77));
		}

		[Test]
		public void ADbReadWithNullDbRecordShouldReturnDefaultObject()
		{
			// Arrange
			// Act
			var product = Db.Read(null, DataRecordMake, true);

			// Assert
			Assert.That(product, Is.EqualTo(default(Product)));
		}

		[Test]
		public void ADbReadWithClosedDbRecordShouldReturnDefaultObject()
		{
			// Arrange
			const string sqlText = "select * from [Northwind].[dbo].[Products] where [SupplierId] = @param1 and [ProductName] = @param2;";
			IDbCommand dbCommand = _database.GetSqlTextCommand(sqlText);
			_database.AddInParameter(dbCommand, "@param1", DbType.Int32, 1);
			_database.AddInParameter(dbCommand, "@param2", DbType.String, "Chang");
			Product product = null;
			IDataReader dataReader = null;

			// Act
			try
			{
				dataReader = _database.ExecuteReader(dbCommand, CommandBehavior.SingleRow | CommandBehavior.CloseConnection);
				dataReader.Close();
				product = Db.Read(dataReader, DataRecordMake, true);
			}
			finally
			{
				dataReader?.Close();
				dataReader?.Dispose();
			}

			// Assert
			Assert.That(product, Is.EqualTo(default(Product)));
		}

		protected class Product
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public int SupplierId { get; set; }
			public int CategoryId { get; set; }
			public string QuantityPerUnit { get; set; }
			public decimal UnitPrice { get; set; }
			public short UnitsInStock { get; set; }
			public short UnitsOnOrder { get; set; }
			public short ReorderLevel { get; set; }
			public bool Discontinued { get; set; }
		}

		// Makes a user business object from a data row.
		private static readonly Func<DataRow, Product> DataRowMake =
			dataRow => new Product()
			{
				Id = dataRow["ProductId"].AsInt32(),
				Name = DatabaseExtensions.AsString(dataRow["ProductName"]),
				SupplierId = dataRow["SupplierId"].AsInt32(),
				CategoryId = dataRow["CategoryId"].AsInt32(),
				QuantityPerUnit = DatabaseExtensions.AsString(dataRow["QuantityPerUnit"]),
				UnitPrice = dataRow["UnitPrice"].AsDecimal(),
				UnitsInStock = dataRow["UnitsInStock"].AsInt16(),
				UnitsOnOrder = dataRow["UnitsOnOrder"].AsInt16(),
				ReorderLevel = dataRow["ReorderLevel"].AsInt16(),
				Discontinued = dataRow["Discontinued"].AsBoolean()
			};

		// Makes a user business object from a data record.
		private static readonly Func<IDataRecord, Product> DataRecordMake =
			dataRecord => new Product()
			{
				Id = dataRecord["ProductId"].AsInt32(),
				Name = DatabaseExtensions.AsString(dataRecord["ProductName"]),
				SupplierId = dataRecord["SupplierId"].AsInt32(),
				CategoryId = dataRecord["CategoryId"].AsInt32(),
				QuantityPerUnit = DatabaseExtensions.AsString(dataRecord["QuantityPerUnit"]),
				UnitPrice = dataRecord["UnitPrice"].AsDecimal(),
				UnitsInStock = dataRecord["UnitsInStock"].AsInt16(),
				UnitsOnOrder = dataRecord["UnitsOnOrder"].AsInt16(),
				ReorderLevel = dataRecord["ReorderLevel"].AsInt16(),
				Discontinued = dataRecord["Discontinued"].AsBoolean()
			};
	}
}
