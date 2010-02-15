using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace Company.DataGrid.Automation
{
	public class CellAutomationPeer : ItemAutomationPeer, ITableItemProvider, ISelectionItemProvider, IValueProvider
	{
		public CellAutomationPeer(UIElement item) : base(item)
		{
		}

		public CellAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer) : base(item, itemsControlAutomationPeer)
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

		public void AddToSelection()
		{
			throw new NotImplementedException();
		}

		public void RemoveFromSelection()
		{
			throw new NotImplementedException();
		}

		public void Select()
		{
			throw new NotImplementedException();
		}

		public bool IsSelected
		{
			get { throw new NotImplementedException(); }
		}

		public IRawElementProviderSimple SelectionContainer
		{
			get { throw new NotImplementedException(); }
		}

		public void SetValue(string value)
		{
			throw new NotImplementedException();
		}

		public bool IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public string Value
		{
			get { throw new NotImplementedException(); }
		}
	}
}
