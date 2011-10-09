// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
// 
// File:	TypeExtensions.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using XamlGrid.Aspects;

namespace XamlGrid.Controllers
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
		[Validate]
		public static bool IsSimple([NotNull] this Type type)
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
		[Validate]
		public static bool IsNumeric([NotNull] this Type type)
		{
			Type typeToCheck = Nullable.GetUnderlyingType(type) ?? type;
			TypeCode typeCode = Type.GetTypeCode(typeToCheck);
			return (TypeCode.Char <= typeCode && typeCode <= TypeCode.Decimal);
		}

		/// <summary>
		/// Gets the type of the elements (assuming all elements have the same type) of the specified <see cref="IEnumerable"/>.
		/// </summary>
		/// <param name="enumerable">The <see cref="IEnumerable"/> to get the element type of.</param>
		/// <returns>The type in the generic placeholder of the specified <see cref="IEnumerable"/> if the latter is generic;
		/// otherwise, the type of the first non-<c>null</c> element, if any.</returns>
		[Validate]
		public static Type GetElementType([NotNull] this IEnumerable enumerable)
		{
			Type enumerableType = null;
			ICollectionView collectionView = enumerable as ICollectionView;
			if (collectionView != null && collectionView.SourceCollection != null)
			{
				enumerableType = collectionView.SourceCollection.GetType();
			}
			if (enumerableType == null)
			{
				enumerableType = enumerable.GetType();
			}
			Type @interface = enumerableType.GetInterface(typeof(IEnumerable<>).FullName, false);
			if (@interface != null)
			{
				return @interface.GetGenericArguments()[0];
			}
			IEnumerator enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current != null)
				{
					return enumerator.Current.GetType();
				}
			}
			return typeof(object);
		}
	}
}
