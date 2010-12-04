using System.Linq;
using System.Windows;
using Company.Widgets.Controllers;
using Company.Widgets.Models;

namespace Company.Widgets.Views
{
	/// <summary>
	/// Represents a header that contains explanatory information about the data in a <see cref="DataGrid"/>.
	/// </summary>
	public class HeaderRow : RowBase
	{
		/// <summary>
		/// Represents a header that contains explanatory information about the data in a <see cref="DataGrid"/>.
		/// </summary>
		public HeaderRow()
		{
			this.DefaultStyleKey = typeof(HeaderRow);


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

		protected override void OnVisibilityChanged(DependencyPropertyChangedEventArgs e)
		{
			foreach (Column column in from Column item in this.Items
			                          where (item.Width.SizeMode == SizeMode.Auto || item.Width.SizeMode == SizeMode.ToHeader)
			                          select item)
			{
				column.AutoSize();
			}
			base.OnVisibilityChanged(e);
		}
	}
}
