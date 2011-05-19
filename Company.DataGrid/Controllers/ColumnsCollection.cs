using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Company.Widgets.Models;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Represents an <see cref="ObservableCollection{T}"/> of <see cref="Column"/>s.
	/// </summary>
	public class ColumnsCollection : ObservableCollection<Column>
	{
		private double currentWholeWidth;

		/// <summary>
		/// Calculates the relative widths of columns in this <see cref="ColumnsCollection"/> using the last specified width.
		/// </summary>
		public void CalculateRelativeWidths()
		{
			this.CalculateRelativeWidths(this.currentWholeWidth);
		}

		/// <summary>
		/// Calculates the relative widths of columns in this <see cref="ColumnsCollection"/> using the specified width.
		/// </summary>
		/// <param name="wholeWidth">The whole available width.</param>
		public void CalculateRelativeWidths(double wholeWidth)
		{
			if (this.currentWholeWidth == wholeWidth)
			{
				return;
			}
			this.currentWholeWidth = wholeWidth;

			if (this.Any(column => double.IsNaN(column.ActualWidth)))
			{
				return;
			}
			IEnumerable<Column> relativeColumns = from column in this
												  where column.Visibility == Visibility.Visible &&
														column.Width.SizeMode == SizeMode.Fill
												  select column;
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
	}
}
