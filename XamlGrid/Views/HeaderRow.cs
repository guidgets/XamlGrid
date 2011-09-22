using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using XamlGrid.Automation;
using XamlGrid.Controllers;
using XamlGrid.Models;

namespace XamlGrid.Views
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
		}

		/// <summary>
		/// When implemented in a derived class, returns class-specific <see cref="AutomationPeer"/> implementations for the Silverlight automation infrastructure.
		/// </summary>
		/// <returns>
		/// The class-specific <see cref="AutomationPeer"/> subclass to return.
		/// </returns>
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new HeaderRowAutomationPeer(this);
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
			((CellBase) element).Column = (Column) item;
			base.PrepareContainerForItemOverride(element, item);
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
