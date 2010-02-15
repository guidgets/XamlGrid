using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Company.DataGrid.Automation
{
	public class HeaderRowAutomationPeer : ItemsControlAutomationPeer, ITableProvider
	{
		public HeaderRowAutomationPeer(ItemsControl owner) : base(owner)
		{
		}

		public IRawElementProviderSimple GetItem(int row, int column)
		{
			throw new NotImplementedException();
		}

		public int ColumnCount
		{
			get { throw new NotImplementedException(); }
		}

		public int RowCount
		{
			get { throw new NotImplementedException(); }
		}

		public IRawElementProviderSimple[] GetColumnHeaders()
		{
			throw new NotImplementedException();
		}

		public IRawElementProviderSimple[] GetRowHeaders()
		{
			throw new NotImplementedException();
		}

		public RowOrColumnMajor RowOrColumnMajor
		{
			get { throw new NotImplementedException(); }
		}
	}
}
