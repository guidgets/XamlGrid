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
// File:	ColumnsCollection.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using XamlGrid.Models;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Represents an <see cref="ObservableCollection{T}"/> of <see cref="Column"/>s.
	/// </summary>
	public class ColumnsCollection : ObservableCollection<Column>
	{
		/// <summary>
		/// Inserts the item.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		protected override void InsertItem(int index, Column item)
		{
			base.InsertItem(index, item);
			item.IndexChanged -= this.Item_IndexChanged;
			item.Index = index;
			item.IndexChanged += this.Item_IndexChanged;
		}

		/// <summary>
		/// Sets the item.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		protected override void SetItem(int index, Column item)
		{
			base.SetItem(index, item);
			item.IndexChanged -= this.Item_IndexChanged;
			item.Index = index;
			item.IndexChanged += this.Item_IndexChanged;
		}

		/// <summary>
		/// Removes the item.
		/// </summary>
		/// <param name="index">The index.</param>
		protected override void RemoveItem(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				this[index].IndexChanged -= this.Item_IndexChanged;
				this[index].Index = -1;
			}
			base.RemoveItem(index);
		}

		/// <summary>
		/// Clears the items.
		/// </summary>
		protected override void ClearItems()
		{
			foreach (Column column in this.Items)
			{
				column.IndexChanged -= this.Item_IndexChanged;
				column.Index = -1;
			}
			base.ClearItems();
		}


		/// <summary>
		/// Calculates the relative widths of columns in this <see cref="ColumnsCollection"/> using the specified width.
		/// </summary>
		/// <param name="wholeWidth">The whole available width.</param>
		public void CalculateRelativeWidths(double wholeWidth)
		{
			if (this.Any(column => double.IsNaN(column.ActualWidth)))
			{
				return;
			}
			IEnumerable<Column> relativeColumns = (from column in this
												   where column.Visibility == Visibility.Visible &&
														 column.Width.SizeMode == SizeMode.Fill
												   select column).ToList();
			double stars = relativeColumns.Sum(column => column.Width.Value);
			double availableWidth = wholeWidth - (from column in this
			                                      where column.Visibility == Visibility.Visible &&
			                                            column.Width.SizeMode != SizeMode.Fill
			                                      select column.ActualWidth).Sum();
			foreach (Column column in relativeColumns.Skip(1))
			{
				double width = Math.Floor(column.Width.Value * availableWidth / stars);
				column.ActualWidth = Math.Max(width, 1);
			}
			Column firstColumn = relativeColumns.FirstOrDefault();
			if (firstColumn != null)
			{
				double width = wholeWidth - (from column in this
				                             where column.Visibility == Visibility.Visible &&
				                                   column != firstColumn
				                             select column.ActualWidth).Sum();
				firstColumn.ActualWidth = Math.Max(width, 1);
			}
		}


		private void Item_IndexChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			Column column = (Column) sender;
			int index = column.Index;
			this.Remove(column);
			this.Insert(index, column);
		}
	}
}
