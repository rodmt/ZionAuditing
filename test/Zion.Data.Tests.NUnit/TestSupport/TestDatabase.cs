// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System;
using System.Data.Common;

namespace Zion.Data.Tests.NUnit.TestSupport
{
    public class TestDatabase : Database
    {
        public TestDatabase(string connectionString,
                            DbProviderFactory dbProviderFactory,
                            int commandTimeout = 60)
                            : base(dbProviderFactory, GetDbConnectionStringBuilder(connectionString), commandTimeout)
        {
        }

        private static DbConnectionStringBuilder GetDbConnectionStringBuilder(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            return new DbConnectionStringBuilder() { ConnectionString = connectionString };
        }

        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
            throw new NotImplementedException();
        }
    }
}
