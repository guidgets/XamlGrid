﻿using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using Company.DataGrid.Views;

namespace Company.DataGrid.Automation
{
	/// <summary>
	/// Exposes the <see cref="DataGrid"/> to UI automation.
	/// </summary>
	public class DataGridAutomationPeer : ItemsControlAutomationPeer, ITableProvider, ISelectionProvider, IScrollProvider, IMultipleViewProvider
	{
		private readonly Views.DataGrid dataGrid;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataGridAutomationPeer"/> class.
		/// </summary>
		/// <param name="owner">The <see cref="DataGrid"/> to associate with this <see cref="DataGridAutomationPeer"/>.</param>
		public DataGridAutomationPeer(Views.DataGrid owner) : base(owner)
		{
			this.dataGrid = owner;
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
			if (!string.IsNullOrEmpty(this.dataGrid.Name))
			{
				return this.dataGrid.Name;
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
				case PatternInterface.MultipleView:
				case PatternInterface.Table:
					return this;
			}
			return base.GetPattern(patternInterface);
		}

		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return base.CreateItemAutomationPeer(item);
		}


		public IRawElementProviderSimple GetItem(int row, int column)
		{
			throw new NotImplementedException();
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
				return this.dataGrid.Columns.Count;
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
				return this.dataGrid.Items.Count;
			}
		}

		public IRawElementProviderSimple[] GetColumnHeaders()
		{
			throw new NotImplementedException();
		}

		public IRawElementProviderSimple[] GetRowHeaders()
		{
			throw new NotImplementedException();
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

		public IRawElementProviderSimple[] GetSelection()
		{
			throw new NotImplementedException();
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
				return this.dataGrid.SelectionMode == SelectionMode.Multiple ||
				       this.dataGrid.SelectionMode == SelectionMode.Extended;
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
			get
			{
				throw new NotImplementedException();
			}
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