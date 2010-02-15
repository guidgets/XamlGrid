using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace Company.DataGrid.Automation
{
	public class HeaderCellAutomationPeer : ItemAutomationPeer, ITableItemProvider
	{
		public HeaderCellAutomationPeer(UIElement item) : base(item)
		{
		}

		public HeaderCellAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer) : base(item, itemsControlAutomationPeer)
		{
		}

		public int Column
		{
			get { throw new NotImplementedException(); }
		}

		public int ColumnSpan
		{
			get { throw new NotImplementedException(); }
		}

		public IRawElementProviderSimple ContainingGrid
		{
			get { throw new NotImplementedException(); }
		}

		public int Row
		{
			get { throw new NotImplementedException(); }
		}

		public int RowSpan
		{
			get { throw new NotImplementedException(); }
		}

		public IRawElementProviderSimple[] GetColumnHeaderItems()
		{
			throw new NotImplementedException();
		}

		public IRawElementProviderSimple[] GetRowHeaderItems()
		{
			throw new NotImplementedException();
		}
	}
}
