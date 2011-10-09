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
// File:	DataWrapper.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Collections;
using System.ComponentModel;
using System.Windows.Data;

namespace XamlGrid.Models
{
	/// <summary>
	/// Contains functionality to wrap a data source with the purpose of enabling it with advanced functionality.
	/// </summary>
	public static class DataWrapper
	{
		/// <summary>
		/// Wraps the specified source to expose it for manipulation.
		/// </summary>
		/// <param name="source">The source to wrap.</param>
		/// <returns></returns>
		public static ICollectionView Wrap(IEnumerable source)
		{
			return source as ICollectionView ?? new PagedCollectionView(source);
		}
	}
}
