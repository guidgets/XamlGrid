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
		public event EventHandler<SortDirectionEventArgs> SortDirectionChanged;


		public static readonly DependencyProperty SortDirectionProperty =
			DependencyProperty.Register("SortDirection", typeof(ListSortDirection?), typeof(HeaderCell), new PropertyMetadata(null, OnSortDirectionChanged));

		private static void OnSortDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HeaderCell) d).OnSortDirectionChanged(new SortDirectionEventArgs((ListSortDirection?) e.NewValue));
		}


		public HeaderCell()
		{
			this.DefaultStyleKey = typeof(HeaderCell);

			DataGridFacade.Instance.RegisterController(new HeaderCellController(this));
		}


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
