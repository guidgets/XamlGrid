using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Company.DataGrid.Automation
{
	public class RowAutomationPeer : ItemsControlAutomationPeer, ITableProvider, ITableItemProvider, ISelectionProvider, ISelectionItemProvider, IScrollItemProvider, IMultipleViewProvider
	{
		public RowAutomationPeer(ItemsControl owner)
			: base(owner)
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

		public void ScrollIntoView()
		{
			throw new NotImplementedException();
		}

		public int[] GetSupportedViews()
		{
			throw new NotImplementedException();
		}

		public string GetViewName(int viewId)
		{
			throw new NotImplementedException();
		}

		public void SetCurrentView(int viewId)
		{
			throw new NotImplementedException();
		}

		public int CurrentView
		{
			get { throw new NotImplementedException(); }
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

		public IRawElementProviderSimple[] GetSelection()
		{
			throw new NotImplementedException();
		}

		public bool CanSelectMultiple
		{
			get { throw new NotImplementedException(); }
		}

		public bool IsSelectionRequired
		{
			get { throw new NotImplementedException(); }
		}
	}
}
