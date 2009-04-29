using System;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Contains methods that supply additional functionality when working with types.
	/// </summary>
	public static class TypeController
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