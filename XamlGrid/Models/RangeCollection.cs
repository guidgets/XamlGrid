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
// File:	RangeCollection.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XamlGrid.Models
{
	/// <summary>
	/// Represents an <see cref="ObservableCollection{T}"/> of <see cref="Range"/> objects.
	/// </summary>
	public class RangeCollection : ObservableCollection<Range>
	{
		/// <summary>
		/// Inserts the specified item (<see cref="Range"/>) if it is not already contained in another <see cref="Range"/> object.
		/// </summary>
		/// <param name="index">The index to insert the item at.</param>
		/// <param name="item">The item to insert.</param>
		protected override void InsertItem(int index, Range item)
		{
			if (this.ContainsRange(item))
			{
				return;
			}
			base.InsertItem(index, item);
		}

		/// <summary>
		/// Determines whether the <see cref="RangeCollection"/> contains the specified <see cref="Range"/> as 
		/// either an independent range or as a sub-range of a larger range.
		/// </summary>
		/// <param name="range">The <see cref="Range"/> to check for being contained in the <see cref="RangeCollection"/>.</param>
		/// <returns>
		/// 	<c>true</c> if the <see cref="RangeCollection"/> contains the specified <see cref="Range"/>; otherwise, <c>false</c>.
		/// </returns>
		public virtual bool ContainsRange(Range range)
		{
			return (from item in this
			        where Math.Min(item.Start, item.End) <= Math.Min(range.Start, range.End) && 
						  Math.Max(range.Start, range.End) <= Math.Max(item.Start, item.End)
			        select item).Any();
		}

		/// <summary>
		/// Adds the range that starts and ends at the respective specified parameters.
		/// </summary>
		/// <param name="rangeStart">The initial index of the range to add.</param>
		/// <param name="rangeEnd">The final index of the range to add.</param>
		public virtual void AddRange(int rangeStart, int rangeEnd)
		{
			this.Add(new Range(rangeStart, rangeEnd));
		}

		/// <summary>
		/// Removes the range that starts and ends at the respective specified parameters.
		/// </summary>
		/// <param name="rangeStart">The initial index of the range to remove.</param>
		/// <param name="rangeEnd">The final index of the range to remove.</param>
		/// <returns></returns>
		public virtual bool RemoveRange(int rangeStart, int rangeEnd)
		{
			Range rangeToRemove = new Range(rangeStart, rangeEnd);
			if (this.Contains(rangeToRemove))
			{
				return this.Remove(rangeToRemove);
			}
			IEnumerable<Range> ranges = from range in this
			                            where range.ContainsRange(rangeToRemove)
			                            select range;
			if (ranges.Any())
			{
				Range containingRange = ranges.First();
				int index = this.IndexOf(containingRange);
				bool result = this.Remove(containingRange);
				if (containingRange.Start < rangeToRemove.Start)
				{
					this.InsertItem(index++, new Range(containingRange.Start, rangeToRemove.Start - 1));
				}
				if (rangeToRemove.End < containingRange.End)
				{
					this.InsertItem(index, new Range(rangeToRemove.End + 1, containingRange.End));
				}
				return result;
			}
			return false;
		}
	}
}
