// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System.Data;

namespace Zion.Data.Tests.NUnit.TestSupport
{
	internal class TestDataTableBuilder : DataTableBuilder
	{
		public TestDataTableBuilder()
		{
		}

		public TestDataTableBuilder(string tableName)
			: base(tableName)
		{
		}

		#region Overrides of DataTableBuilder

		/// <summary>
		/// Adds columns to the <see cref="DataTable"/> instance.
		/// </summary>
		protected override void BuildColumns()
		{
		}

		#endregion

		public void CreateBooleanNullColumn(string columnName)
		{
			AddBooleanNullColumn(columnName);
		}

		public void CreateBooleanNotNullColumn(string columnName)
		{
			AddBooleanNotNullColumn(columnName);
		}

		public void CreateByteNullColumn(string columnName)
		{
			AddByteNullColumn(columnName);
		}

		public void CreateByteNotNullColumn(string columnName)
		{
			AddByteNotNullColumn(columnName);
		}

		public void CreateDateTimeNullColumn(string columnName)
		{
			AddDateTimeNullColumn(columnName);
		}

		public void CreateDateTimeNotNullColumn(string columnName)
		{
			AddDateTimeNotNullColumn(columnName);
		}

		public void CreateDoubleNullColumn(string columnName)
		{
			AddDoubleNullColumn(columnName);
		}

		public void CreateDoubleNotNullColumn(string columnName)
		{
			AddDoubleNotNullColumn(columnName);
		}

		public void CreateGuidNullColumn(string columnName)
		{
			AddGuidNullColumn(columnName);
		}

		public void CreateGuidNotNullColumn(string columnName)
		{
			AddGuidNotNullColumn(columnName);
		}

		public void CreateInt16NullColumn(string columnName)
		{
			AddInt16NullColumn(columnName);
		}

		public void CreateInt16NotNullColumn(string columnName)
		{
			AddInt16NotNullColumn(columnName);
		}

		public void CreateInt32NullColumn(string columnName)
		{
			AddInt32NullColumn(columnName);
		}

		public void CreateInt32NotNullColumn(string columnName)
		{
			AddInt32NotNullColumn(columnName);
		}

		public void CreateInt64NullColumn(string columnName)
		{
			AddInt64NullColumn(columnName);
		}

		public void CreateInt64NotNullColumn(string columnName)
		{
			AddInt64NotNullColumn(columnName);
		}

		public void CreateSingleNullColumn(string columnName)
		{
			AddSingleNullColumn(columnName);
		}

		public void CreateSingleNullColumn(string columnName, float defaultValue)
		{
			AddSingleNullColumn(columnName, defaultValue);
		}

		public void CreateSingleNotNullColumn(string columnName)
		{
			AddSingleNotNullColumn(columnName);
		}

		public void CreateSingleNotNullColumn(string columnName, float defaultValue)
		{
			AddSingleNotNullColumn(columnName, defaultValue);
		}

		public void CreateStringNullColumn(string columnName)
		{
			AddStringNullColumn(columnName);
		}

		public void CreateStringNotNullColumn(string columnName)
		{
			AddStringNotNullColumn(columnName);
		}

		public void CreateStringNotNullColumn(string columnName, string defaultValue)
		{
			AddStringNotNullColumn(columnName, defaultValue);
		}

		public void CreateUInt16NullColumn(string columnName)
		{
			AddUInt16NullColumn(columnName);
		}

		public void CreateUInt16NotNullColumn(string columnName)
		{
			AddUInt16NotNullColumn(columnName);
		}

		public void CreateUInt32NullColumn(string columnName)
		{
			AddUInt32NullColumn(columnName);
		}

		public void CreateUInt32NotNullColumn(string columnName)
		{
			AddUInt32NotNullColumn(columnName);
		}

		public void CreateUInt64NullColumn(string columnName)
		{
			AddUInt64NullColumn(columnName);
		}

		public void CreateUInt64NotNullColumn(string columnName)
		{
			AddUInt64NotNullColumn(columnName);
		}
	}
}