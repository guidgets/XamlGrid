using System;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Contains methods that extend the functionality of <see cref="Type"/>.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Determines whether the specified type is simple, that is, it is not built upon other types.
		/// These are the numeric types (including <see cref="char"/>) and <see cref="string"/>.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> to check.</param>
		/// <returns>
		/// 	<c>true</c> if the specified <see cref="Type"/> is simple; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsSimple(this Type type)
		{
			Type typeToCheck = Nullable.GetUnderlyingType(type) ?? type;
			return typeToCheck.IsPrimitive || typeToCheck == typeof(decimal) || typeToCheck == typeof(string);
		}

		/// <summary>
		/// Determines whether the <see cref="Type"/> is a numeric type.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> to check.</param>
		/// <returns>
		/// 	<c>true</c> if the <see cref="Type"/> is a numeric type; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNumeric(this Type type)
		{
			Type typeToCheck = Nullable.GetUnderlyingType(type) ?? type;
			TypeCode typeCode = Type.GetTypeCode(typeToCheck);
			return (TypeCode.Char <= typeCode && typeCode <= TypeCode.Decimal);
		}
	}
}