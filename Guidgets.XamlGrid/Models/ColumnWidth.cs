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
// File:	ColumnWidth.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.ComponentModel;
using Guidgets.XamlGrid.Controllers;

namespace Guidgets.XamlGrid.Models
{
	/// <summary>
	/// Represents different ways of specifying the width of a <see cref="Column"/>.
	/// </summary>
	[TypeConverter(typeof(StringToColumnWidthConverter))]
	public struct ColumnWidth
	{
		/// <summary>
		/// Represents different ways of specifying the width of a <see cref="Column"/>.
		/// </summary>
		/// <param name="value">The absolute value of the width of the <see cref="Column"/>.</param>
		public ColumnWidth(double value) : this()
		{
			this.Value = value;
		}

		/// <summary>
		/// Represents different ways of specifying the width of a <see cref="Column"/>.
		/// </summary>
		/// <param name="sizeMode">The size mode according to which a <see cref="Column"/> gets its width.</param>
		public ColumnWidth(SizeMode sizeMode) : this()
		{
			this.Value = 1;
			this.SizeMode = sizeMode;
		}

		/// <summary>
		/// Represents different ways of specifying the width of a <see cref="Column"/>.
		/// </summary>
		/// <param name="value">The number of "star" units the width of a <see cref="Column"/> uses.</param>
		/// <param name="sizeMode">The size mode according to which a <see cref="Column"/> gets its width.</param>
		public ColumnWidth(double value, SizeMode sizeMode) : this()
		{
			this.Value = value;
			this.SizeMode = sizeMode;
		}

		/// <summary>
		/// Gets or sets the absolute value of the wight of the <see cref="Column"/>; 
		/// relevant only when the size mode is <see cref="Models.SizeMode.Absolute"/> or <see cref="Models.SizeMode.Fill"/>.
		/// </summary>
		/// <value>The absolute value of the wight of the <see cref="Column"/>.</value>
		public double Value
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets the size mode according to which a <see cref="Column"/> gets its width.
		/// </summary>
		/// <value>The size mode according to which a <see cref="Column"/> gets its width.</value>
		public SizeMode SizeMode
		{
			get; 
			set;
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="columnWidthLeft">The column width left.</param>
		/// <param name="columnWidthRight">The column width right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(ColumnWidth columnWidthLeft, ColumnWidth columnWidthRight)
		{
			return columnWidthLeft.SizeMode == columnWidthRight.SizeMode &&
			       columnWidthLeft.Value == columnWidthRight.Value;
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="columnWidthLeft">The column width left.</param>
		/// <param name="columnWidthRight">The column width right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(ColumnWidth columnWidthLeft, ColumnWidth columnWidthRight)
		{
			return !(columnWidthLeft == columnWidthRight);
		}

		/// <summary>
		/// Determines whether this <see cref="ColumnWidth"/> objects is equal to the specified other <see cref="ColumnWidth"/>.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns></returns>
		public bool Equals(ColumnWidth other)
		{
			base.Equals(other);
			return other.Value == this.Value && Equals(other.SizeMode, this.SizeMode);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is ColumnWidth && this.Equals((ColumnWidth) obj);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (this.Value.GetHashCode() * 397) ^ this.SizeMode.GetHashCode();
			}
		}

		/// <summary>
		/// Determines whether the <see cref="ColumnWidth"/> specifies an automatic size.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if the <see cref="ColumnWidth"/> specifies an automatic size; otherwise, <c>false</c>.
		/// </returns>
		public bool IsAuto()
		{
			return this.SizeMode == SizeMode.Auto || this.SizeMode == SizeMode.ToHeader || this.SizeMode == SizeMode.ToData;
		}
	}
}
