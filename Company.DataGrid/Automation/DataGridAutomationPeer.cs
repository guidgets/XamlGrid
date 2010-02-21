using System;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Company.DataGrid.Automation
{
	/// <summary>
	/// Exposes a <see cref="DataGrid"/> object to UI automation.
	/// </summary>
	public class DataGridAutomationPeer : ItemsControlAutomationPeer, ITableProvider, ISelectionProvider, IScrollProvider
	{
		/// <summary>
		/// Exposes a <see cref="DataGrid"/> object to UI automation.
		/// </summary>
		/// <param name="owner">The <see cref="DataGrid"/> to associate with this <see cref="DataGridAutomationPeer"/>.</param>
		public DataGridAutomationPeer(Views.DataGrid owner) : base(owner)
		{

		}


		private Views.DataGrid DataGrid
		{
			get
			{
				return (Views.DataGrid) this.Owner;
			}
		}


		/// <summary>
		/// Returns the control type for the <see cref="T:System.Windows.UIElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType"/>.
		/// </summary>
		/// <returns>A value of the enumeration.</returns>
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataGrid;
		}

		/// <summary>
		/// Returns the name of the <see cref="T:System.Windows.UIElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName"/>.
		/// </summary>
		/// <returns>
		/// The name of the owner type that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer"/>. See Remarks.
		/// </returns>
		protected override string GetClassNameCore()
		{
			return this.Owner.GetType().Name;
		}

		/// <summary>
		/// Returns the text label of the <see cref="T:System.Windows.FrameworkElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName"/>.
		/// </summary>
		/// <returns>
		/// The text label of the element that is associated with this automation peer.
		/// </returns>
		protected override string GetNameCore()
		{
			if (!string.IsNullOrEmpty(this.DataGrid.Name))
			{
				return this.DataGrid.Name;
			}
			string name = base.GetNameCore();
			return string.IsNullOrEmpty(name) ? this.GetClassName() : name;
		}

		/// <summary>
		/// Gets a control pattern for the <see cref="T:System.Windows.Controls.ItemsControl"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer"/>.
		/// </summary>
		/// <param name="patternInterface">One of the enumeration values that indicates the control pattern.</param>
		/// <returns>
		/// The object that implements the pattern interface, or <c>null</c> if the specified pattern interface is not implemented by this peer.
		/// </returns>
		public override object GetPattern(PatternInterface patternInterface)
		{
			switch (patternInterface)
			{
				case PatternInterface.Selection:
				case PatternInterface.Scroll:
				case PatternInterface.Grid:
				case PatternInterface.Table:
					return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/> for a data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> collection of this <see cref="T:System.Windows.Controls.ItemsControl"/>.
		/// </summary>
		/// <param name="item">The data item that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/>.</param>
		/// <returns>
		/// An object that exposes the data item to UI automation.
		/// </returns>
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new RowAutomationPeer(item, this);
		}

		/// <summary>
		/// Retrieves the UI automation provider for the specified cell.
		/// </summary>
		/// <param name="row">The ordinal number of the row that contains the cell.</param>
		/// <param name="column">The ordinal number of the column that contains the cell.</param>
		/// <returns>
		/// The UI automation provider for the specified cell.
		/// </returns>
		public IRawElementProviderSimple GetItem(int row, int column)
		{
			if (row < this.DataGrid.Items.Count && column < this.DataGrid.Columns.Count)
			{
				DependencyObject rowElement = this.DataGrid.ItemContainerGenerator.ContainerFromIndex(row);
				if (rowElement is ItemsControl)
				{
					DependencyObject cell = ((ItemsControl) rowElement).ItemContainerGenerator.ContainerFromIndex(column);
					if (cell is UIElement)
					{
						return this.ProviderFromPeer(CreatePeerForElement((UIElement) cell));
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the total number of columns in a grid.
		/// </summary>
		/// <value></value>
		/// <returns>The total number of columns in a grid.</returns>
		public int ColumnCount
		{
			get
			{
				return this.DataGrid.Columns.Count;
			}
		}

		/// <summary>
		/// Gets the total number of rows in a grid.
		/// </summary>
		/// <value></value>
		/// <returns>The total number of rows in a grid.</returns>
		public int RowCount
		{
			get
			{
				return this.DataGrid.Items.Count;
			}
		}

		/// <summary>
		/// Returns a collection of UI Automation providers that represents all the column headers in a table.
		/// </summary>
		/// <returns>An array of UI automation providers.</returns>
		public IRawElementProviderSimple[] GetColumnHeaders()
		{
			return (from column in this.DataGrid.Columns
			    select this.ProviderFromPeer(CreatePeerForElement(column.HeaderCell))).ToArray();
		}

		/// <summary>
		/// Returns a collection of UI Automation providers that represents all row headers in the table.
		/// </summary>
		/// <returns>An array of UI automation providers.</returns>
		public IRawElementProviderSimple[] GetRowHeaders()
		{
			return new IRawElementProviderSimple[0];
		}

		/// <summary>
		/// Gets the primary direction of traversal for the table.
		/// </summary>
		/// <value></value>
		/// <returns>The primary direction of traversal, as a value of the enumeration. </returns>
		public RowOrColumnMajor RowOrColumnMajor
		{
			get
			{
				return RowOrColumnMajor.RowMajor;
			}
		}

		/// <summary>
		/// Retrieves a UI automation provider for each child element that is selected.
		/// </summary>
		/// <returns>An array of UI automation providers.</returns>
		public IRawElementProviderSimple[] GetSelection()
		{
			return (from item in this.DataGrid.SelectedItems
			    let row = this.DataGrid.ItemContainerGenerator.ContainerFromItem(item)
			    let peer = CreatePeerForElement((UIElement) row)
			    select this.ProviderFromPeer(peer)).ToArray();
		}

		/// <summary>
		/// Gets a value that indicates whether the UI automation provider allows more than one child element to be selected concurrently.
		/// </summary>
		/// <value></value>
		/// <returns><c>true</c> if multiple selection is allowed; otherwise, <c>false</c>.</returns>
		public bool CanSelectMultiple
		{
			get
			{
				return this.DataGrid.SelectionMode == SelectionMode.Multiple ||
				    this.DataGrid.SelectionMode == SelectionMode.Extended;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the UI automation provider requires at least one child element to be selected.
		/// </summary>
		/// <value></value>
		/// <returns><c>true</c> if selection is required; otherwise, <c>false</c>.</returns>
		public bool IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Scrolls the visible region of the content area horizontally, vertically, or both.
		/// </summary>
		/// <param name="horizontalAmount">The horizontal increment that is specific to the control. Pass <see cref="F:System.Windows.Automation.ScrollPatternIdentifiers.NoScroll"/> if the control cannot be scrolled in this direction.</param>
		/// <param name="verticalAmount">The vertical increment that is specific to the control. Pass <see cref="F:System.Windows.Automation.ScrollPatternIdentifiers.NoScroll"/> if the control cannot be scrolled in this direction.</param>
		public void Scroll(ScrollAmount horizontalAmount, ScrollAmount verticalAmount)
		{
			IScrollInfo scrollInfo = this.DataGrid.GetItemsHost() as IScrollInfo;
			if (scrollInfo == null)
			{
				return;
			}
			if (horizontalAmount != ScrollAmount.NoAmount && scrollInfo.ExtentWidth <= scrollInfo.ViewportWidth)
			{
				throw new InvalidOperationException("Trying to scroll horizontally with horizontal scrolling disabled.");
			}
			if (horizontalAmount != ScrollAmount.NoAmount && scrollInfo.ExtentHeight <= scrollInfo.ViewportHeight)
			{
				throw new InvalidOperationException("Trying to scroll vertically with vertical scrolling disabled.");
			}
			switch (horizontalAmount)
			{
				case ScrollAmount.LargeDecrement:
					scrollInfo.PageLeft();
					break;
				case ScrollAmount.SmallDecrement:
					scrollInfo.LineLeft();
					break;
				case ScrollAmount.LargeIncrement:
					scrollInfo.PageRight();
					break;
				case ScrollAmount.SmallIncrement:
					scrollInfo.LineRight();
					break;
			}
			switch (verticalAmount)
			{
				case ScrollAmount.LargeDecrement:
					scrollInfo.PageUp();
					break;
				case ScrollAmount.SmallDecrement:
					scrollInfo.LineUp();
					break;
				case ScrollAmount.LargeIncrement:
					scrollInfo.PageDown();
					break;
				case ScrollAmount.SmallIncrement:
					scrollInfo.LineDown();
					break;
			}
			ScrollViewer scroll = this.DataGrid.GetScrollHost();
			if (scroll != null)
			{
				scrollInfo.SetVerticalOffset(Math.Min(scroll.ScrollableHeight, scroll.VerticalOffset));
			}
		}

		/// <summary>
		/// Sets the horizontal and vertical scroll position as a percentage of the total content area within the control.
		/// </summary>
		/// <param name="horizontalPercent">The horizontal position as a percentage of the content area's total range. Pass <see cref="F:System.Windows.Automation.ScrollPatternIdentifiers.NoScroll"/> if the control cannot be scrolled in this direction.</param>
		/// <param name="verticalPercent">The vertical position as a percentage of the content area's total range. Pass <see cref="F:System.Windows.Automation.ScrollPatternIdentifiers.NoScroll"/> if the control cannot be scrolled in this direction.</param>
		public void SetScrollPercent(double horizontalPercent, double verticalPercent)
		{
			ScrollViewer scroll = this.DataGrid.GetScrollHost();
			if (scroll == null)
			{
				return;
			}
			if (horizontalPercent != -1)
			{
				if (scroll.ExtentWidth <= scroll.ViewportWidth)
				{
					throw new InvalidOperationException("Trying to scroll horizontally with horizontal scrolling disabled.");
				}
				if (horizontalPercent < 0 || horizontalPercent > 100)
				{
					throw new ArgumentOutOfRangeException("horizontalPercent", "The percentage for the horizontal offset must be between 0 and 100.");
				}
			}
			if (verticalPercent != -1)
			{
				if (scroll.ExtentHeight <= scroll.ViewportHeight)
				{
					throw new InvalidOperationException("Trying to scroll vertically with vertical scrolling disabled.");
				}
				if (verticalPercent < 0 || verticalPercent > 100)
				{
					throw new ArgumentOutOfRangeException("verticalPercent", "The percentage for vertical offset must be between 0 and 100.");
				}
			}
			if (horizontalPercent != -1)
			{
				scroll.ScrollToHorizontalOffset((scroll.ExtentWidth - scroll.ViewportWidth) * horizontalPercent / 100);
			}
			if (verticalPercent != -1)
			{
				scroll.ScrollToVerticalOffset((scroll.ExtentHeight - scroll.ViewportHeight) * verticalPercent / 100);
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the control can scroll horizontally.
		/// </summary>
		/// <value></value>
		/// <returns><c>true</c> if the control can scroll horizontally; otherwise, <c>false</c>.</returns>
		public bool HorizontallyScrollable
		{
			get
			{
				Panel itemsHost = this.DataGrid.GetItemsHost();
				if (itemsHost is IScrollInfo)
				{
					return ((IScrollInfo) itemsHost).CanHorizontallyScroll;
				}
				return false;
			}
		}

		/// <summary>
		/// Gets the current horizontal scroll position.
		/// </summary>
		/// <value></value>
		/// <returns>The horizontal scroll position as a percentage of the total content area within the control.</returns>
		public double HorizontalScrollPercent
		{
			get
			{
				ScrollViewer scroll = this.DataGrid.GetScrollHost();
				if (scroll == null || scroll.ExtentWidth <= scroll.ViewportWidth)
				{
					return -1;
				}
				return 100 * scroll.HorizontalOffset / (scroll.ExtentWidth - scroll.ViewportWidth);
			}
		}

		/// <summary>
		/// Gets the current horizontal view size.
		/// </summary>
		/// <value></value>
		/// <returns>The horizontal size of the viewable region as a percentage of the total content area within the control. </returns>
		public double HorizontalViewSize
		{
			get
			{
				ScrollViewer scroll = this.DataGrid.GetScrollHost();
				if (scroll != null)
				{
					return Math.Min(100, 100 * scroll.ViewportWidth / scroll.ExtentWidth);
				}
				return 100;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the control can scroll vertically.
		/// </summary>
		/// <value></value>
		/// <returns><c>true</c> if the control can scroll vertically; otherwise, <c>false</c>. </returns>
		public bool VerticallyScrollable
		{
			get
			{
				Panel itemsHost = this.DataGrid.GetItemsHost();
				if (itemsHost is IScrollInfo)
				{
					return ((IScrollInfo) itemsHost).CanVerticallyScroll;
				}
				return false;
			}
		}

		/// <summary>
		/// Gets the current vertical scroll position.
		/// </summary>
		/// <value></value>
		/// <returns>The vertical scroll position as a percentage of the total content area within the control. </returns>
		public double VerticalScrollPercent
		{
			get
			{
				ScrollViewer scroll = this.DataGrid.GetScrollHost();
				if (scroll == null || scroll.ExtentHeight <= scroll.ViewportHeight)
				{
					return -1;
				}
				return 100 * scroll.VerticalOffset / (scroll.ExtentHeight - scroll.ViewportHeight);
			}
		}

		/// <summary>
		/// Gets the vertical view size.
		/// </summary>
		/// <value></value>
		/// <returns>The vertical size of the viewable region as a percentage of the total content area within the control. </returns>
		public double VerticalViewSize
		{
			get
			{
				ScrollViewer scroll = this.DataGrid.GetScrollHost();
				if (scroll != null)
				{
					return Math.Min(100, 100 * scroll.ViewportHeight / scroll.ExtentHeight);
				}
				return 100;
			}
		}
	}
}
