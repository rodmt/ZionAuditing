// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System;
using System.Data;

namespace Zion.Data
{
    /// <summary>
    /// A wrapper around a database connection instance.
    /// </summary>
    public sealed class DatabaseConnection : IDisposable
    {
        /// <summary>
        /// Gets a <see cref="IDbConnection"/>.
        /// </summary>
        public IDbConnection Connection { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether the 
        /// <see cref="IDbConnection"/> is open.
        /// </summary>
        public bool IsOpen => Connection.State == ConnectionState.Open;

	    /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="dbConnection">A <see cref="IDbConnection"/>.</param>
        public DatabaseConnection(IDbConnection dbConnection)
        {
            Connection = dbConnection;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }

        #endregion
    }
}
