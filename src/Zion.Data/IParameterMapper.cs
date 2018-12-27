// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System.Data;

namespace Zion.Data
{
    public interface IParameterMapper<in T>
    {
        /// <summary>
        /// Adds <see cref="IDataParameter"/>s to a <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="parameterValues">A collection of parameter values.</param>
        void AssignParameters(IDbCommand dbCommand, object[] parameterValues);

        /// <summary>
        /// Adds <see cref="IDataParameter"/>s to a <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="dbCommand">A <see cref="IDbCommand"/> instance.</param>
        /// <param name="entity">A <typeparam name="T"/>.</param>
        void AssignParameters(IDbCommand dbCommand, T entity);
    }
}
