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
// File:	HeaderCellAutomationPeer.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using XamlGrid.Views;

namespace XamlGrid.Automation
{
	/// <summary>
	/// Exposes a <see cref="Views.HeaderCell"/> object to UI automation.
	/// </summary>
	public class HeaderCellAutomationPeer : ItemAutomationPeer, ITableItemProvider
	{
		/// <summary>
		/// Exposes a <see cref="Views.HeaderCell"/> object to UI automation.
		/// </summary>
		/// <param name="item">The <see cref="Views.HeaderCell"/> to associate with this <see cref="HeaderCellAutomationPeer"/>.</param>
		public HeaderCellAutomationPeer(UIElement item) : base(item)
		{
		}

		/// <summary>
		/// Exposes a <see cref="Views.HeaderCell"/> object to UI automation.
		/// </summary>
		/// <param name="item">The data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/>.</param>
		/// <param name="itemsControlAutomationPeer">The <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer"/> that is associated with the <see cref="T:System.Windows.Controls.ItemsControl"/> that holds the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> collection.</param>
		public HeaderCellAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer) : base(item, itemsControlAutomationPeer)
		{
		}


		private HeaderCell HeaderCell
		{
			get
			{
				return (HeaderCell) this.Owner;
			}
		}


		/// <summary>
		/// Returns the control type for the item that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType"/>.
		/// </summary>
		/// <returns>
		/// A value of the enumeration.
		/// </returns>
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.HeaderItem;
		}

		/// <summary>
		/// Returns the class name of the item that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName"/>.
		/// </summary>
		/// <returns>
		/// The name of the owner type that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer"/>. See Remarks.
		/// </returns>
		protected override string GetClassNameCore()
		{
			return this.Owner.GetType().Name;
		}

		/// <summary>
		/// Gets the ordinal number of the column that contains the cell or item.
		/// </summary>
		/// <value></value>
		/// <returns>A zero-based ordinal number that identifies the column that contains the cell or item.</returns>
		public int Column
		{
			get
			{
				return this.HeaderCell.Column.Index;
			}
		}

		/// <summary>
		/// Gets the number of columns that are spanned by a cell or item.
		/// </summary>
		/// <value></value>
		/// <returns>The number of columns. </returns>
		public int ColumnSpan
		{
			get
			{
				return 1;
			}
		}

		/// <summary>
		/// Gets a UI automation provider that implements <see cref="T:System.Windows.Automation.Provider.IGridProvider"/> and that represents the container of the cell or item.
		/// </summary>
		/// <value></value>
		/// <returns>A UI automation provider that implements the <see cref="F:System.Windows.Automation.Peers.PatternInterface.Grid"/> control pattern and that represents the cell or item container. </returns>
		public IRawElementProviderSimple ContainingGrid
		{
			get
			{
				return this.ProviderFromPeer(this.ItemsControlAutomationPeer);
			}
		}

		/// <summary>
		/// Gets the ordinal number of the row that contains the cell or item.
		/// </summary>
		/// <value></value>
		/// <returns>A zero-based ordinal number that identifies the row that contains the cell or item. </returns>
		public int Row
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Gets the number of rows spanned by a cell or item.
		/// </summary>
		/// <value></value>
		/// <returns>The number of rows. </returns>
		public int RowSpan
		{
			get
			{
				return 1;
			}
		}

		/// <summary>
		/// Retrieves an array of UI automation providers representing all the column headers associated with a table item or cell.
		/// </summary>
		/// <returns>An array of UI automation providers.</returns>
		public IRawElementProviderSimple[] GetColumnHeaderItems()
		{
			return new[] { this.ProviderFromPeer(this) };
		}

		/// <summary>
		/// Retrieves an array of UI automation providers representing all the row headers associated with a table item or cell.
		/// </summary>
		/// <returns>An array of UI automation providers.</returns>
		public IRawElementProviderSimple[] GetRowHeaderItems()
		{
			return new IRawElementProviderSimple[0];
		}
	}
}
