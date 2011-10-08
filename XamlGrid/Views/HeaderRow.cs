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
// File:	HeaderRow.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

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
