// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Zion.Data
{
	/// <summary>
	/// Manages all lower level data base access.
	/// </summary>
	public static class Db
	{
		/// <summary>
		/// Makes a collection of <typeparam name="TResult"/> from a collection
		/// of <see cref="DataRow"/>s.
		/// </summary>
		/// <typeparam name="TResult">The type of object.</typeparam>
		/// <param name="dataRows">A collection of <see cref="DataRow"/>s.</param>
		/// <param name="make">Delegate to make a <typeparam name="TResult"/>.</param>
		/// <returns>A collection of <typeparam name="TResult"/>.</returns>
		public static IEnumerable<TResult> Read<TResult>(IEnumerable<DataRow> dataRows,
														 Func<DataRow, TResult> make)
		{
			// Check if have a collection.
			if (dataRows == null || make == null)
				return null;

			// Make the type.
			return dataRows.Select(dataRow => Read(dataRow, make));
		}

		/// <summary>
		/// Makes a <typeparam name="TResult"/> from a <see cref="DataRow"/>.
		/// </summary>
		/// <typeparam name="TResult">The type of object.</typeparam>
		/// <param name="dataRow">A <see cref="DataRow"/>.</param>
		/// <param name="make">Delegate to make a <typeparam name="TResult"/>.</param>
		/// <returns>A <typeparam name="TResult"/>.</returns>
		public static TResult Read<TResult>(DataRow dataRow,
											Func<DataRow, TResult> make)
		{
			// Check if we have a data row.
			return dataRow == null || make == null ? default(TResult) : make(dataRow);
		}

		/// <summary>
		/// Makes a <see cref="T"/> from a <see cref="IDataReader"/>.
		/// </summary>
		/// <typeparam name="T">The type of object.</typeparam>
		/// <param name="dataReader">A <see cref="IDataReader"/>.</param>
		/// <param name="make">Delegate to make a <see cref="T"/>.</param>
		/// <param name="advanceReader">Indicates whether to advance the reader.</param>
		/// <returns></returns>
		public static T Read<T>(IDataReader dataReader,
								Func<IDataRecord, T> make,
								bool advanceReader)
		{
			// Check if the reader is closed and if we 
			// are able advance to the next record.
			return dataReader == null || dataReader.IsClosed || (advanceReader && !dataReader.Read())
				? default(T)
				: make(dataReader);
		}

		/// <summary>
		/// Makes a <see cref="T"/> from a <see cref="IDataReader"/>.
		/// </summary>
		/// <typeparam name="T">The type of object.</typeparam>
		/// <param name="dataRecord">A <see cref="IDataReader"/>.</param>
		/// <param name="make">Delegate to make a <see cref="T"/>.</param>
		/// <returns>A collection of <see cref="T"/>.</returns>
		public static T Read<T>(IDataRecord dataRecord,
								Func<IDataRecord, T> make)
		{
			return dataRecord == null ? default(T) : make(dataRecord);
		}

		/// <summary>
		/// Makes a <see cref="T"/> from a <see cref="IDataReader"/>.
		/// </summary>
		/// <typeparam name="T">The type of object.</typeparam>
		/// <param name="dataReader">A <see cref="IDataReader"/>.</param>
		/// <param name="make">Delegate to make a <see cref="T"/>.</param>
		/// <returns>A collection of <see cref="T"/>.</returns>
		public static IEnumerable<T> Read<T>(IDataReader dataReader,
											 Func<IDataRecord, T> make)
		{
			while (dataReader.Read())
				yield return make(dataReader);
		}
	}
}
