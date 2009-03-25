using System;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Contains methods that supply additional functionality when working with types.
	/// </summary>
	public static class TypeController
	{
		/// <summary>
		/// Determines whether the type of the specified object is numeric.
		/// </summary>
		/// <param name="objectOfTypeToCheck">An object of the type to check.</param>
		/// <returns>
		/// 	<c>true</c> if the type of the specified object is numeric; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNumeric(object objectOfTypeToCheck)
		{
			if (objectOfTypeToCheck is IConvertible)
			{
				TypeCode typeCode = ((IConvertible) objectOfTypeToCheck).GetTypeCode();
				return (TypeCode.Char <= typeCode && typeCode <= TypeCode.Decimal);
			}
			return false;
		}
	}
}