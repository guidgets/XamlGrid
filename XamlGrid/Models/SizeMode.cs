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
// File:	SizeMode.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>
// 
namespace XamlGrid.Models
{
	/// <summary>
	/// Provides values for different types of sizing.
	/// </summary>
	public enum SizeMode
	{
		/// <summary>
		/// The size is an absolute value.
		/// </summary>
		Absolute,
		/// <summary>
		/// The size fills all available space.
		/// </summary>
		Fill,
		/// <summary>
		/// The size is automatically calculated.
		/// </summary>
		Auto,
		/// <summary>
		/// The size is automatically calculated according to a header element.
		/// </summary>
		ToHeader,
		/// <summary>
		/// The size is automatically calculated according to the displayed data.
		/// </summary>
		ToData
	}
}
