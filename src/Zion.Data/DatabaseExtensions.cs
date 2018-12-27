// Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
//
// Unpublished copyright. This material contains proprietary information
// that shall be used or copied only within Zion Software Solutions, 
// except with written permission of Zion Software Solutions.

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Zion.Data
{
	/// <summary>
	/// Implements additional functionality for the database class.
	/// </summary>
	public static class DatabaseExtensions
	{
		/// <summary>
		/// Converts the object to a <see cref="Boolean"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="Boolean"/> or a default value.</returns>
		public static bool AsBoolean(this object value,
										bool defaultValue = default(bool))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			bool result;
			return !bool.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="Byte"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="Byte"/> or a default value.</returns>
		public static byte AsByte(this object value,
								  byte defaultValue = default(byte))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			byte result;
			return !byte.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Convert the object to an array <see cref="Byte"/>s.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="Byte"/> or a default value.</returns>
		public static byte[] AsBytes(this object value,
									 byte[] defaultValue = default(byte[]))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			// Memory Stream instance.
			MemoryStream memoryStream = null;

			try
			{
				// Create a new memory stream instance.
				memoryStream = new MemoryStream();

				// Convert to an array.
				var binaryWriter = new BinaryFormatter();
				binaryWriter.Serialize(memoryStream, value);
				return memoryStream.ToArray();
			}
			finally
			{
				memoryStream?.Close();
				memoryStream?.Dispose();
			}
		}

		/// <summary>
		/// Converts the object to a <see cref="Char"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="Char"/> or a default value.</returns>
		public static char AsChar(this object value,
								  char defaultValue = default(char))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			char result;
			return !char.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="Decimal"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="Decimal"/> or a default value.</returns>
		public static decimal AsDecimal(this object value,
										decimal defaultValue = default(decimal))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			decimal result;
			return !decimal.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="DateTime"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="DateTime"/> or a default value.</returns>
		public static DateTime AsDateTime(this object value,
										  DateTime defaultValue = default(DateTime))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			DateTime result;
			return !DateTime.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="Guid"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="Guid"/> or a default value.</returns>
		public static Guid AsGuid(this object value,
								  Guid defaultValue = default(Guid))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			Guid result;
			return !Guid.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="Int16"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="Int16"/> or a default value.</returns>
		public static short AsInt16(this object value,
									short defaultValue = default(short))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			short result;
			return !short.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="Int32"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="Int32"/> or a default value.</returns>
		public static int AsInt32(this object value,
									int defaultValue = default(int))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			int result;
			return !int.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="Int64"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="Int64"/> or a default value.</returns>
		public static long AsInt64(this object value,
									long defaultValue = default(long))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			long result;
			return !long.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="SByte"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="SByte"/> or a default value.</returns>
		public static sbyte AsSByte(this object value,
									sbyte defaultValue = default(sbyte))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			sbyte result;
			return !sbyte.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="Single"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="Single"/> or a default value.</returns>
		public static float AsSingle(this object value,
									  float defaultValue = default(float))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			float result;
			return !float.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="String"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">A default value.</param>
		/// <returns>The value converted to <see cref="String"/> or a default value.</returns>
		public static string AsString(this object value,
									  string defaultValue = default(string))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;
			return value.ToString();
		}

		/// <summary>
		/// Converts the object to a <see cref="UInt16"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="UInt16"/> or a default value.</returns>
		public static ushort AsUInt16(this object value,
									  ushort defaultValue = default(ushort))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			ushort result;
			return !ushort.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="UInt32"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="UInt32"/> or a default value.</returns>
		public static uint AsUInt32(this object value,
									  uint defaultValue = default(uint))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			uint result;
			return !uint.TryParse(value.ToString(), out result) ? defaultValue : result;
		}

		/// <summary>
		/// Converts the object to a <see cref="UInt64"/>.
		/// </summary>
		/// <param name="value">The value from the database.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value converted to <see cref="UInt64"/> or a default value.</returns>
		public static ulong AsUInt64(this object value,
									  ulong defaultValue = default(ulong))
		{
			if (value == null || Convert.IsDBNull(value))
				return defaultValue;

			ulong result;
			return !ulong.TryParse(value.ToString(), out result) ? defaultValue : result;
		}
	}
}
