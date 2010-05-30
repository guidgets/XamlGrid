using System;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Contains methods that extend the functionality of <see cref="Type"/>.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Determines whether the <see cref="Type"/> is a numeric type.
		/// </summary>
		/// <param name="typeToCheck">The <see cref="Type"/> to check.</param>
		/// <returns>
		/// 	<c>true</c> if the <see cref="Type"/> is a numeric type; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNumeric(this Type typeToCheck)
		{
			Type nulledType = Nullable.GetUnderlyingType(typeToCheck);
			TypeCode typeCode = nulledType != null ? Type.GetTypeCode(nulledType) : Type.GetTypeCode(typeToCheck);
			return (TypeCode.Char <= typeCode && typeCode <= TypeCode.Decimal);
		}
	}
}