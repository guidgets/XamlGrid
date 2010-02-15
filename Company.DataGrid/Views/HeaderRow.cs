﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Company.DataGrid.Controllers;
using Company.DataGrid.Models;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents a header that contains explanatory information about the data in a <see cref="DataGrid"/>.
	/// </summary>
	public class HeaderRow : ItemsControl
	{
		private static readonly DependencyProperty visibilityListenerProperty =
			DependencyProperty.Register("visibilityListener", typeof(Visibility), typeof(HeaderRow),
										new PropertyMetadata(Visibility.Visible, OnVisibilityListenerChanged));

		private static readonly Binding visibilityBinding = new Binding("Visibility")
	                                                    	{
	                                                    		RelativeSource = new RelativeSource(RelativeSourceMode.Self),
	                                                    		Mode = BindingMode.OneWay
	                                                    	};


		/// <summary>
		/// Represents a header that contains explanatory information about the data in a <see cref="DataGrid"/>.
		/// </summary>
		public HeaderRow()
		{
			this.DefaultStyleKey = typeof(HeaderRow);

			this.SetBinding(visibilityListenerProperty, visibilityBinding);

			DataGridFacade.Instance.RegisterController(new HeaderRowController(this));
		}


		/// <summary>
		/// Creates or identifies the element that is used to display the given item.
		/// </summary>
		/// <returns>
		/// The element that is used to display the given item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new HeaderCell();
		}

		/// <summary>
		/// Prepares the specified element to display the specified item.
		/// </summary>
		/// <param name="element">The element used to display the specified item.</param>
		/// <param name="item">The item to display.</param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			HeaderCell headerCell = (HeaderCell) element;
			Column column = (Column) item;
			headerCell.Column = column;
		}

		/// <summary>
		/// Undoes the effects of the <see cref="M:System.Windows.Controls.ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)"/> method.
		/// </summary>
		/// <param name="element">The container element.</param>
		/// <param name="item">The item.</param>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			DataGridFacade.Instance.RemoveController(element.GetHashCode().ToString());
		}


		private static void OnVisibilityListenerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderRow headerRow = (HeaderRow) d;
			foreach (Column column in from Column item in headerRow.Items
									  where (item.Width.SizeMode == SizeMode.Auto || item.Width.SizeMode == SizeMode.ToHeader)
									  select item)
			{
				column.AutoSize();
			}
		}
	}
}
