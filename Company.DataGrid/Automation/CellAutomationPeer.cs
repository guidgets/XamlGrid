using System;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using Company.Widgets.Views;

namespace Company.Widgets.Automation
{
	/// <summary>
	/// Exposes a <see cref="Views.Cell"/> object to UI automation.
	/// </summary>
	public class CellAutomationPeer : ItemAutomationPeer, ITableItemProvider, IValueProvider
	{
		/// <summary>
		/// Exposes a <see cref="Views.Cell"/> object to UI automation.
		/// </summary>
		/// <param name="item">The <see cref="Views.Cell"/> to associate with this <see cref="CellAutomationPeer"/>.</param>
		public CellAutomationPeer(Cell item) : base(item)
		{

		}

		/// <summary>
		/// Exposes a <see cref="Views.Cell"/> object to UI automation.
		/// </summary>
		/// <param name="item">The data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/>.</param>
		/// <param name="itemsControlAutomationPeer">The <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer"/> that is associated with the <see cref="T:System.Windows.Controls.ItemsControl"/> that holds the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> collection.</param>
		public CellAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer) : base(item, itemsControlAutomationPeer)
		{

		}
		

		private Cell Cell
		{
			get
			{
				return (Cell) this.Owner;
			}
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
				return this.Cell.Column.Index;
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
				return this.Cell.RowIndex;
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
			return new IRawElementProviderSimple[0];
		}

		/// <summary>
		/// Retrieves an array of UI automation providers representing all the row headers associated with a table item or cell.
		/// </summary>
		/// <returns>An array of UI automation providers.</returns>
		public IRawElementProviderSimple[] GetRowHeaderItems()
		{
			return new IRawElementProviderSimple[0];
		}

		/// <summary>
		/// Sets the value of a control.
		/// </summary>
		/// <param name="value">The value to set. The provider is responsible for converting the value to the appropriate data type.</param>
		public void SetValue(string value)
		{
			if (!this.Cell.IsEditable)
			{
				throw new InvalidOperationException("Cannot change the value of a non-editable (read-only) cell.");
			}
			this.Cell.Value = value;
		}

		/// <summary>
		/// Gets a value that indicates whether the value of a control is read-only.
		/// </summary>
		/// <value></value>
		/// <returns><c>true</c> if the value is read-only; <c>false</c> if it can be modified. </returns>
		public bool IsReadOnly
		{
			get
			{
				return !this.Cell.IsEditable;
			}
		}

		/// <summary>
		/// Gets the value of the control.
		/// </summary>
		/// <value></value>
		/// <returns>The value of the control.</returns>
		public string Value
		{
			get
			{
				return this.Cell.Value.ToString();
			}
		}
	}
}
