// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System;
using System.Globalization;
using NUnit.Framework;

namespace Zion.Data.Tests.NUnit
{
	[TestFixture]
	public class DatabaseExtensionsFixture
	{
		private readonly DBNull _dbNull = DBNull.Value;
		private readonly object _nullObject = null;
		private readonly object _guidObject = "01234567-89AB-CDEF-0123-456789ABCDEF";

		#region AsBoolean Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultBoolean()
		{
			// Arrange
			const bool expectedValue = default(bool);

			// Act - Convert to Boolean.
			var actualValue = _dbNull.AsBoolean();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultBoolean()
		{
			// Arrange
			const bool expectedValue = true;

			// Act - Convert to Boolean.
			var actualValue = _dbNull.AsBoolean(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultBoolean()
		{
			// Arrange
			const bool expectedValue = default(bool);

			// Act - Convert to Boolean.
			var actualValue = _nullObject.AsBoolean();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultBoolean()
		{
			// Arrange
			const bool expectedValue = true;

			// Act - Convert to Boolean.
			var actualValue = _nullObject.AsBoolean(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultBoolean()
		{
			// Arrange
			const bool expectedValue = default(bool);

			// Act - Convert to Boolean.
			var actualValue = "Invalid Object".AsBoolean();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultBoolean()
		{
			// Arrange
			const bool expectedValue = true;

			// Act - Convert to Boolean.
			var actualValue = "Invalid Object".AsBoolean(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidBooleanObjectShouldReturnBoolean()
		{
			// Arrange
			object booleanObject = true;
			var expectedValue = bool.Parse(booleanObject.ToString());

			// Act - Convert to Boolean.
			var actualValue = booleanObject.AsBoolean();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsByte Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultByte()
		{
			// Arrange
			const byte expectedValue = default(byte);

			// Act - Convert to Byte.
			var actualValue = _dbNull.AsByte();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultByte()
		{
			// Arrange
			const byte expectedValue = byte.MaxValue - 1;

			// Act - Convert to Byte.
			var actualValue = _dbNull.AsByte(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultByte()
		{
			// Arrange
			const byte expectedValue = default(byte);

			// Act - Convert to Byte.
			var actualValue = _nullObject.AsByte();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultByte()
		{
			// Arrange
			const byte expectedValue = byte.MaxValue - 1;

			// Act - Convert to Byte.
			var actualValue = _nullObject.AsByte(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultByte()
		{
			// Arrange
			const byte expectedValue = default(byte);

			// Act - Convert to Byte.
			var actualValue = (byte.MaxValue + 1).ToString(CultureInfo.InvariantCulture)
												 .AsByte();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultByte()
		{
			// Arrange
			const byte expectedValue = byte.MaxValue - 1;

			// Act - Convert to Byte.
			var actualValue = (byte.MaxValue + 1).ToString(CultureInfo.InvariantCulture)
												 .AsByte(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidByteObjectShouldReturnByte()
		{
			// Arrange
			object byteObject = (byte.MaxValue / 2);
			var expectedValue = byte.Parse(byteObject.ToString());

			// Act - Convert to Byte.
			var actualValue = byteObject.AsByte();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsByteArray Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultByteArray()
		{
			// Arrange
			const byte[] expectedValue = default(byte[]);

			// Act - Convert to ByteArray.
			var actualValue = _dbNull.AsBytes();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultByteArray()
		{
			// Arrange
			var expectedValue = new byte[] { 0x1, 0x2, 0x3, 0x4, 0xAA, 0xAF };

			// Act - Convert to ByteArray.
			var actualValue = _dbNull.AsBytes(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultByteArray()
		{
			// Arrange
			const byte[] expectedValue = default(byte[]);

			// Act - Convert to ByteArray.
			var actualValue = _nullObject.AsBytes();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultByteArray()
		{
			// Arrange
			var expectedValue = new byte[] { 0x1, 0x2, 0x3, 0x4, 0xAA, 0xAF };

			// Act - Convert to ByteArray.
			var actualValue = _nullObject.AsBytes(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		[Ignore("Need to refactor the test.")]
		public void AnInvalidObjectShouldReturnDefaultByteArray()
		{
			// Arrange
			const byte[] expectedValue = default(byte[]);

			// Act - Convert to ByteArray.
			var actualValue = (byte.MaxValue + 1).ToString(CultureInfo.InvariantCulture)
												   .AsBytes();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		[Ignore("Need to refactor the test.")]
		public void AnInvalidObjectShouldReturnGivenDefaultByteArray()
		{
			// Arrange
			var expectedValue = new byte[] { 0x1, 0x2, 0x3, 0x4, 0xAA, 0xAF };

			// Act - Convert to ByteArray.
			var actualValue = (byte.MaxValue + 1).ToString(CultureInfo.InvariantCulture)
												   .AsBytes(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		[Ignore("Need to refactor the test.")]
		public void AValidByteArrayObjectShouldReturnByteArray()
		{
			// Arrange
			object byteArrayObject = long.MaxValue;
			var expectedValue = BitConverter.GetBytes(long.MaxValue);

			// Act - Convert to ByteArray.
			var actualValue = byteArrayObject.AsBytes();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsChar Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultChar()
		{
			// Arrange
			const char expectedValue = default(char);

			// Act - Convert to Char.
			var actualValue = _dbNull.AsChar();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultChar()
		{
			// Arrange
			const char expectedValue = 'A';

			// Act - Convert to Char.
			var actualValue = _dbNull.AsChar(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultChar()
		{
			// Arrange
			const char expectedValue = default(char);

			// Act - Convert to Char.
			var actualValue = _nullObject.AsChar();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultChar()
		{
			// Arrange
			const char expectedValue = 'A';

			// Act - Convert to Char.
			var actualValue = _nullObject.AsChar(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultChar()
		{
			// Arrange
			const char expectedValue = default(char);

			// Act - Convert to Char.
			var actualValue = (char.MaxValue + 1).ToString(CultureInfo.InvariantCulture)
												   .AsChar();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultChar()
		{
			// Arrange
			const char expectedValue = 'A';

			// Act - Convert to Char.
			var actualValue = (char.MaxValue + 1).ToString(CultureInfo.InvariantCulture)
												   .AsChar(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidCharObjectShouldReturnChar()
		{
			// Arrange
			object charObject = char.MaxValue;
			var expectedValue = char.Parse(charObject.ToString());

			// Act - Convert to Char.
			var actualValue = charObject.AsChar();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsDateTime Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultDateTime()
		{
			// Arrange
			var expectedValue = default(DateTime);

			// Act - Convert to DateTime.
			var actualValue = _dbNull.AsDateTime();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultDateTime()
		{
			// Arrange
			var expectedValue = DateTime.UtcNow;

			// Act - Convert to DateTime.
			var actualValue = _dbNull.AsDateTime(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultDateTime()
		{
			// Arrange
			var expectedValue = default(DateTime);

			// Act - Convert to DateTime.
			var actualValue = _nullObject.AsDateTime();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultDateTime()
		{
			// Arrange
			var expectedValue = DateTime.UtcNow;

			// Act - Convert to DateTime.
			var actualValue = _nullObject.AsDateTime(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultDateTime()
		{
			// Arrange
			var expectedValue = default(DateTime);

			// Act - Convert to DateTime.
			var actualValue = "Invalid Object".AsDateTime();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultDateTime()
		{
			// Arrange
			var expectedValue = DateTime.UtcNow;

			// Act - Convert to DateTime.
			var actualValue = "Invalid Object".AsDateTime(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidDateTimeObjectShouldReturnDateTime()
		{
			// Arrange
			object dateTimeObject = new DateTime(1976, 7, 14);
			var expectedValue = Convert.ToDateTime(dateTimeObject);

			// Act - Convert to DateTime.
			var actualValue = dateTimeObject.AsDateTime();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsDecimal Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultDecimal()
		{
			// Arrange
			const decimal expectedValue = default(decimal);

			// Act - Convert to Decimal.
			var actualValue = _dbNull.AsDecimal();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultDecimal()
		{
			// Arrange
			var expectedValue = new decimal(7.2398);

			// Act - Convert to Decimal.
			var actualValue = _dbNull.AsDecimal(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultDecimal()
		{
			// Arrange
			const decimal expectedValue = default(decimal);

			// Act - Convert to Decimal.
			var actualValue = _nullObject.AsDecimal();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultDecimal()
		{
			// Arrange
			var expectedValue = new decimal(7.2398);

			// Act - Convert to Decimal.
			var actualValue = _nullObject.AsDecimal(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultDecimal()
		{
			// Arrange
			const decimal expectedValue = default(decimal);

			// Act - Convert to Decimal.
			var actualValue = "Invalid Object".AsDecimal();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultDecimal()
		{
			// Arrange
			var expectedValue = new decimal(7.2398);

			// Act - Convert to Decimal.
			var actualValue = "Invalid Object".AsDecimal(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidDecimalObjectShouldReturnDecimal()
		{
			// Arrange
			object decimalObject = (decimal.MaxValue / 2);
			var expectedValue = decimal.Parse(decimalObject.ToString());

			// Act - Convert to Decimal.
			var actualValue = decimalObject.AsDecimal();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsGuid Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultGuid()
		{
			// Arrange
			var expectedValue = default(Guid);

			// Act - Convert to GUID.
			var actualValue = _dbNull.AsGuid();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultGuid()
		{
			// Arrange
			var expectedValue = new Guid();

			// Act - Convert to GUID.
			var actualValue = _dbNull.AsGuid(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultGuid()
		{
			// Arrange
			var expectedValue = default(Guid);

			// Act - Convert to GUID.
			var actualValue = _nullObject.AsGuid();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultGuid()
		{
			// Arrange
			var expectedValue = new Guid();

			// Act - Convert to GUID.
			var actualValue = _nullObject.AsGuid(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultGuid()
		{
			// Arrange
			var expectedValue = default(Guid);

			// Act - Convert to GUID.
			var actualValue = "01234567-89AB-CDEF-0123-ZZZ789ABCDEF".AsGuid();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultGuid()
		{
			// Arrange
			var expectedValue = new Guid();

			// Act - Convert to GUID.
			var actualValue = "01234567-89AB-CDEF-0123-ZZZ789ABCDEF".AsGuid(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidGuidObjectShouldReturnGuid()
		{
			// Arrange
			object guidObject = "01234567-89AB-CDEF-0123-456789ABCDEF";
			var expectedValue = new Guid(guidObject.ToString());

			// Act - Convert to GUID.
			var actualValue = guidObject.AsGuid();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsInt16 Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultInt16()
		{
			// Arrange
			const short expectedValue = default(short);

			// Act - Convert to Int16.
			var actualValue = _dbNull.AsInt16();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultInt16()
		{
			// Arrange
			const short expectedValue = short.MaxValue;

			// Act - Convert to Int16.
			var actualValue = _dbNull.AsInt16(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultInt16()
		{
			// Arrange
			const short expectedValue = default(short);

			// Act - Convert to Int16.
			var actualValue = _nullObject.AsInt16();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultInt16()
		{
			// Arrange
			const short expectedValue = short.MaxValue;

			// Act - Convert to Int16.
			var actualValue = _nullObject.AsInt16(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultInt16()
		{
			// Arrange
			const short expectedValue = default(short);

			// Act - Convert to Int16.
			var actualValue = "Invalid Object".AsInt16();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultInt16()
		{
			// Arrange
			const short expectedValue = short.MaxValue;

			// Act - Convert to Int16.
			var actualValue = "Invalid Object".AsInt16(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidInt16ObjectShouldReturnInt16()
		{
			// Arrange
			const short expectedValue = short.MaxValue;

			// Act - Convert to Int16.
			var actualValue = short.MaxValue.ToString(CultureInfo.InvariantCulture).AsInt16();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsInt32 Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultInt32()
		{
			// Arrange
			const int expectedValue = default(int);

			// Act - Convert to Int32.
			var actualValue = _dbNull.AsInt32();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultInt32()
		{
			// Arrange
			const int expectedValue = int.MaxValue;

			// Act - Convert to Int32.
			var actualValue = _dbNull.AsInt32(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultInt32()
		{
			// Arrange
			const int expectedValue = default(int);

			// Act - Convert to Int32.
			var actualValue = _nullObject.AsInt32();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultInt32()
		{
			// Arrange
			const int expectedValue = int.MaxValue;

			// Act - Convert to Int32.
			var actualValue = _nullObject.AsInt32(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultInt32()
		{
			// Arrange
			const int expectedValue = default(int);

			// Act - Convert to Int32.
			var actualValue = "Invalid Object".AsInt32();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultInt32()
		{
			// Arrange
			const int expectedValue = int.MaxValue;

			// Act - Convert to Int32.
			var actualValue = "Invalid Object".AsInt32(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidInt32ObjectShouldReturnInt32()
		{
			// Arrange
			const int expectedValue = int.MaxValue;

			// Act - Convert to Int32.
			var actualValue = int.MaxValue.ToString(CultureInfo.InvariantCulture).AsInt32();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsInt64 Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultInt64()
		{
			// Arrange
			const long expectedValue = default(long);

			// Act - Convert to Int64.
			var actualValue = _dbNull.AsInt64();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultInt64()
		{
			// Arrange
			const long expectedValue = long.MaxValue;

			// Act - Convert to Int64.
			var actualValue = _dbNull.AsInt64(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultInt64()
		{
			// Arrange
			const long expectedValue = default(long);

			// Act - Convert to Int64.
			var actualValue = _nullObject.AsInt64();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultInt64()
		{
			// Arrange
			const long expectedValue = long.MaxValue;

			// Act - Convert to Int64.
			var actualValue = _nullObject.AsInt64(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultInt64()
		{
			// Arrange
			const long expectedValue = default(long);

			// Act - Convert to Int64.
			var actualValue = "Invalid Object".AsInt64();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultInt64()
		{
			// Arrange
			const long expectedValue = long.MaxValue;

			// Act - Convert to Int64.
			var actualValue = "Invalid Object".AsInt64(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidInt64ObjectShouldReturnInt64()
		{
			// Arrange
			const long expectedValue = long.MaxValue;

			// Act - Convert to Int64.
			var actualValue = long.MaxValue.ToString(CultureInfo.InvariantCulture).AsInt64();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsSByte Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultSByte()
		{
			// Arrange
			const sbyte expectedValue = default(sbyte);

			// Act - Convert to SByte.
			var actualValue = _dbNull.AsSByte();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultSByte()
		{
			// Arrange
			const sbyte expectedValue = sbyte.MaxValue - 1;

			// Act - Convert to SByte.
			var actualValue = _dbNull.AsSByte(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultSByte()
		{
			// Arrange
			const sbyte expectedValue = default(sbyte);

			// Act - Convert to SByte.
			var actualValue = _nullObject.AsSByte();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultSByte()
		{
			// Arrange
			const sbyte expectedValue = sbyte.MaxValue - 1;

			// Act - Convert to SByte.
			var actualValue = _nullObject.AsSByte(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultSByte()
		{
			// Arrange
			const sbyte expectedValue = default(sbyte);

			// Act - Convert to SByte.
			var actualValue = (sbyte.MaxValue + 1).ToString(CultureInfo.InvariantCulture)
													.AsSByte();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultSByte()
		{
			// Arrange
			const sbyte expectedValue = sbyte.MaxValue - 1;

			// Act - Convert to SByte.
			var actualValue = (sbyte.MaxValue + 1).ToString(CultureInfo.InvariantCulture)
													.AsSByte(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidSByteObjectShouldReturnSByte()
		{
			// Arrange
			object sbyteObject = (sbyte.MaxValue / 2);
			var expectedValue = sbyte.Parse(sbyteObject.ToString());

			// Act - Convert to SByte.
			var actualValue = sbyteObject.AsSByte();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsSingle Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultSingle()
		{
			// Arrange
			const float expectedValue = default(float);

			// Act - Convert to Single.
			var actualValue = _dbNull.AsSingle();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultSingle()
		{
			// Arrange
			var expectedValue = Convert.ToSingle(7.2398);

			// Act - Convert to Single.
			var actualValue = _dbNull.AsSingle(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultSingle()
		{
			// Arrange
			const float expectedValue = default(float);

			// Act - Convert to Single.
			var actualValue = _nullObject.AsSingle();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultSingle()
		{
			// Arrange
			var expectedValue = Convert.ToSingle(7.2398);

			// Act - Convert to Single.
			var actualValue = _nullObject.AsSingle(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultSingle()
		{
			// Arrange
			const float expectedValue = default(float);

			// Act - Convert to Single.
			var actualValue = "Invalid Object".AsSingle();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultSingle()
		{
			// Arrange
			var expectedValue = Convert.ToSingle(7.2398);

			// Act - Convert to Single.
			var actualValue = "Invalid Object".AsSingle(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidSingleObjectShouldReturnSingle()
		{
			// Arrange
			object singleObject = float.Epsilon;
			var expectedValue = float.Parse(singleObject.ToString());

			// Act - Convert to Single.
			var actualValue = singleObject.AsSingle();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsString Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultString()
		{
			// Arrange
			const string expectedValue = default(string);

			// Act - Convert to String.
			var actualValue = DatabaseExtensions.AsString(_dbNull);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultString()
		{
			// Arrange
			const string expectedValue = "Given Default String";

			// Act - Convert to String.
			var actualValue = _dbNull.AsString(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultString()
		{
			// Arrange
			const string expectedValue = default(string);

			// Act - Convert to String.
			var actualValue = DatabaseExtensions.AsString(_nullObject);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultString()
		{
			// Arrange
			const string expectedValue = "Given Default String";

			// Act - Convert to String.
			var actualValue = _nullObject.AsString(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsUInt16 Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultUInt16()
		{
			// Arrange
			const ushort expectedValue = default(ushort);

			// Act - Convert to UInt16.
			var actualValue = _dbNull.AsUInt16();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultUInt16()
		{
			// Arrange
			const ushort expectedValue = ushort.MaxValue;

			// Act - Convert to UInt16.
			var actualValue = _dbNull.AsUInt16(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultUInt16()
		{
			// Arrange
			const ushort expectedValue = default(ushort);

			// Act - Convert to UInt16.
			var actualValue = _nullObject.AsUInt16();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultUInt16()
		{
			// Arrange
			const ushort expectedValue = ushort.MaxValue;

			// Act - Convert to UInt16.
			var actualValue = _nullObject.AsUInt16(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultUInt16()
		{
			// Arrange
			const ushort expectedValue = default(ushort);

			// Act - Convert to UInt16.
			var actualValue = "Invalid Object".AsUInt16();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultUInt16()
		{
			// Arrange
			const ushort expectedValue = ushort.MaxValue;

			// Act - Convert to UInt16.
			var actualValue = "Invalid Object".AsUInt16(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidUInt16ObjectShouldReturnUInt16()
		{
			// Arrange
			const ushort expectedValue = ushort.MaxValue;

			// Act - Convert to UInt16.
			var actualValue = ushort.MaxValue.ToString(CultureInfo.InvariantCulture).AsUInt16();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsUInt32 Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultUInt32()
		{
			// Arrange
			const uint expectedValue = default(uint);

			// Act - Convert to UInt32.
			var actualValue = _dbNull.AsUInt32();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultUInt32()
		{
			// Arrange
			const uint expectedValue = uint.MaxValue;

			// Act - Convert to UInt32.
			var actualValue = _dbNull.AsUInt32(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultUInt32()
		{
			// Arrange
			const uint expectedValue = default(uint);

			// Act - Convert to UInt32.
			var actualValue = _nullObject.AsUInt32();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultUInt32()
		{
			// Arrange
			const uint expectedValue = uint.MaxValue;

			// Act - Convert to UInt32.
			var actualValue = _nullObject.AsUInt32(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultUInt32()
		{
			// Arrange
			const uint expectedValue = default(uint);

			// Act - Convert to UInt32.
			var actualValue = "Invalid Object".AsUInt32();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultUInt32()
		{
			// Arrange
			const uint expectedValue = uint.MaxValue;

			// Act - Convert to UInt32.
			var actualValue = "Invalid Object".AsUInt32(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidUInt32ObjectShouldReturnUInt32()
		{
			// Arrange
			const uint expectedValue = uint.MaxValue;

			// Act - Convert to UInt32.
			var actualValue = uint.MaxValue.ToString(CultureInfo.InvariantCulture).AsUInt32();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion

		#region AsUInt64 Tests

		[Test]
		public void ADbNullObjectShouldReturnDefaultUInt64()
		{
			// Arrange
			const ulong expectedValue = default(ulong);

			// Act - Convert to UInt64.
			var actualValue = _dbNull.AsUInt64();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ADbNullObjectShouldReturnGivenDefaultUInt64()
		{
			// Arrange
			const ulong expectedValue = ulong.MaxValue;

			// Act - Convert to UInt64.
			var actualValue = _dbNull.AsUInt64(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnDefaultUInt64()
		{
			// Arrange
			const ulong expectedValue = default(ulong);

			// Act - Convert to UInt64.
			var actualValue = _nullObject.AsUInt64();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void ANullObjectShouldReturnGivenDefaultUInt64()
		{
			// Arrange
			const ulong expectedValue = ulong.MaxValue;

			// Act - Convert to UInt64.
			var actualValue = _nullObject.AsUInt64(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnDefaultUInt64()
		{
			// Arrange
			const ulong expectedValue = default(ulong);

			// Act - Convert to UInt64.
			var actualValue = "Invalid Object".AsUInt64();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AnInvalidObjectShouldReturnGivenDefaultUInt64()
		{
			// Arrange
			const ulong expectedValue = ulong.MaxValue;

			// Act - Convert to UInt64.
			var actualValue = "Invalid Object".AsUInt64(expectedValue);

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		[Test]
		public void AValidUInt64ObjectShouldReturnUInt64()
		{
			// Arrange
			const ulong expectedValue = ulong.MaxValue;

			// Act - Convert to UInt64.
			var actualValue = ulong.MaxValue.ToString(CultureInfo.InvariantCulture).AsUInt64();

			// Assert
			Assert.That(actualValue, Is.EqualTo(expectedValue));
		}

		#endregion
	}
}
