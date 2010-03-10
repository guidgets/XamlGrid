using System;
using System.ComponentModel;
using System.Windows;
using Company.DataGrid.Controllers;
using Company.DataGrid.Models;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents a cell that stays as a header for a <see cref="Controllers.Column"/>.
	/// </summary>
	public class HeaderCell : CellBase
	{
		/// <summary>
		/// Occurs when the direction, in which the data summarized by the header is sorted, is changed.
		/// </summary>
		public event EventHandler<SortDirectionEventArgs> SortDirectionChanged;


		/// <summary>
		/// Identifies the dependency property which gets or sets the direction, if any, in which the data 
		/// under the <see cref="Column"/>, to which the <see cref="HeaderCell"/> belong, is sorted.
		/// </summary>
		public static readonly DependencyProperty SortDirectionProperty =
			DependencyProperty.Register("SortDirection", typeof(ListSortDirection?), typeof(HeaderCell), new PropertyMetadata(null, OnSortDirectionChanged));

		private static void OnSortDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HeaderCell) d).OnSortDirectionChanged(new SortDirectionEventArgs((ListSortDirection?) e.NewValue));
		}


		/// <summary>
		/// Represents a cell that stays as a header for a <see cref="Controllers.Column"/>.
		/// </summary>
		public HeaderCell()
		{
			this.DefaultStyleKey = typeof(HeaderCell);

			DataGridFacade.Instance.RegisterController(new HeaderCellController(this));
		}


		/// <summary>
		/// Gets or sets the direction, if any, in which the data under the <see cref="Column"/>, 
		/// to which the <see cref="HeaderCell"/> belong, is sorted.
		/// </summary>
		/// <value>The direction in which the data summarized by the header is sorted.</value>
		public ListSortDirection? SortDirection
		{
			get
			{
				return (ListSortDirection?) this.GetValue(SortDirectionProperty);
			}
			set
			{
				this.SetValue(SortDirectionProperty, value);
			}
		}


		/// <summary>
		/// Determines whether the <see cref="Cell"/> is automatically sized according to its contents.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if the <see cref="Cell"/> is automatically sized according to its contents; otherwise, <c>false</c>.
		/// </returns>
		protected override bool IsAutoSized()
		{
			return this.Column.Width.SizeMode == SizeMode.ToHeader || base.IsAutoSized();
		}


		private void OnSortDirectionChanged(SortDirectionEventArgs e)
		{
			EventHandler<SortDirectionEventArgs> handler = this.SortDirectionChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
