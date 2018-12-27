// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System;
using System.Collections.Generic;
using System.Data;

namespace Zion.Data
{
    /// <summary>
    /// Interface for an entity mapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EntityRowMapper<T> : IRowMapper<T> where T : new()
    {
        private readonly Func<DataRow, T> _dataRowMapping;
        private readonly Func<IDataRecord, T> _dataRecordMapping;

        /// <summary>
        /// Initializes a <see cref="EntityRowMapper{T}"/>
        /// </summary>
        /// <param name="dataRowMapping">A data row mapper.</param>
        /// <param name="dataRecordMapping">A data reader mapper.</param>
        protected EntityRowMapper(Func<DataRow, T> dataRowMapping, 
                                  Func<IDataRecord, T> dataRecordMapping)
        {
            if (dataRowMapping == null)
                throw new ArgumentNullException(nameof(dataRowMapping));
            if (dataRecordMapping == null)
                throw new ArgumentNullException(nameof(dataRecordMapping));

            _dataRowMapping = dataRowMapping;
            _dataRecordMapping = dataRecordMapping;
        }

        #region Implementation of IRowMapper<out T>

        /// <summary>
        /// Maps a <see cref="IDataRecord"/> to a <typeparam name="TResult"/>.
        /// </summary>
        /// <param name="dataRecord">A <see cref="IDataRecord"/> to map.</param>
        /// <returns>A <typeparam name="T"/> instance based on <paramref name="dataRecord"/>.</returns>
        public T MapRow(IDataRecord dataRecord)
        {
            if (dataRecord == null)
                throw new ArgumentNullException(nameof(dataRecord));

            return Db.Read(dataRecord, _dataRecordMapping);
        }

        /// <summary>
        /// Maps a <see cref="IDataReader"/> to a 
        /// collection of <typeparam name="TResult"/>s.
        /// </summary>
        /// <param name="dataReader">A <see cref="IDataReader"/> instance.</param>
        /// <returns>A collection of <typeparam name="T"/>s.</returns>
        public IEnumerable<T> MapRows(IDataReader dataReader)
        {
            if (dataReader == null)
                throw new ArgumentNullException(nameof(dataReader));

            return Db.Read(dataReader, _dataRecordMapping);
        }

        /// <summary>
        /// Maps a <see cref="DataRow"/> to a <typeparam name="TResult"/>.
        /// </summary>
        /// <param name="dataRow">A <see cref="DataRow"/> to map.</param>
        /// <returns>A <typeparam name="T"/> instance based on <paramref name="dataRow"/>.</returns>
        public T MapRow(DataRow dataRow)
        {
            if (dataRow == null)
                throw new ArgumentNullException(nameof(dataRow));

            return Db.Read(dataRow, _dataRowMapping);
        }

        /// <summary>
        /// Maps a collection of <see cref="DataRow"/>s to a 
        /// collection of <typeparam name="TResult"/>s.
        /// </summary>
        /// <param name="dataRows">A collection of <see cref="DataRow"/>.</param>
        /// <returns>A collection of <typeparam name="TResult"/>s.</returns>
        public IEnumerable<T> MapRows(IEnumerable<DataRow> dataRows)
        {
            if (dataRows == null)
                throw new ArgumentNullException(nameof(dataRows));

            return Db.Read(dataRows, _dataRowMapping);
        }

        #endregion
    }
}
