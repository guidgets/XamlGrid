using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Company.DataGrid.Automation
{
	public class DataGridAutomationPeer : ItemsControlAutomationPeer, ITableProvider, ISelectionProvider, IScrollProvider, IMultipleViewProvider
	{
		public DataGridAutomationPeer(ItemsControl owner) : base(owner)
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

		public void Scroll(ScrollAmount horizontalAmount, ScrollAmount verticalAmount)
		{
			throw new NotImplementedException();
		}

		public void SetScrollPercent(double horizontalPercent, double verticalPercent)
		{
			throw new NotImplementedException();
		}

		public bool HorizontallyScrollable
		{
			get { throw new NotImplementedException(); }
		}

		public double HorizontalScrollPercent
		{
			get { throw new NotImplementedException(); }
		}

		public double HorizontalViewSize
		{
			get { throw new NotImplementedException(); }
		}

		public bool VerticallyScrollable
		{
			get { throw new NotImplementedException(); }
		}

		public double VerticalScrollPercent
		{
			get { throw new NotImplementedException(); }
		}

		public double VerticalViewSize
		{
			get { throw new NotImplementedException(); }
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
	}
}
