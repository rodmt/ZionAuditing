// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System.Collections.Generic;
using System.Data;

namespace Zion.Data
{
    /// <summary>
    /// Makes a <typeparam name="TResult"></typeparam> from a database object.
    /// </summary>
    public interface IRowMapper<out TResult>
    {
        /// <summary>
        /// Maps a <see cref="IDataRecord"/> to a <typeparam name="TResult"/>.
        /// </summary>
        /// <param name="dataRecord">A <see cref="IDataRecord"/> to map.</param>
        /// <returns>A <typeparam name="TResult"/> instance based on <paramref name="dataRecord"/>.</returns>
        TResult MapRow(IDataRecord dataRecord);

        /// <summary>
        /// Maps a <see cref="IDataReader"/> to a 
        /// collection of <typeparam name="TResult"/>s.
        /// </summary>
        /// <param name="dataReader">A <see cref="IDataReader"/> instance.</param>
        /// <returns>A collection of <typeparam name="TResult"/>s.</returns>
        IEnumerable<TResult> MapRows(IDataReader dataReader);

        /// <summary>
        /// Maps a <see cref="DataRow"/> to a <typeparam name="TResult"/>.
        /// </summary>
        /// <param name="dataRow">A <see cref="DataRow"/> to map.</param>
        /// <returns>A <typeparam name="TResult"/> instance based on <paramref name="dataRow"/>.</returns>
        TResult MapRow(DataRow dataRow);

        /// <summary>
        /// Maps a collection of <see cref="DataRow"/>s to a 
        /// collection of <typeparam name="TResult"/>s.
        /// </summary>
        /// <param name="dataRows">A collection of <see cref="DataRow"/>.</param>
        /// <returns>A collection of <typeparam name="TResult"/>s.</returns>
        IEnumerable<TResult> MapRows(IEnumerable<DataRow> dataRows);
    }
}
