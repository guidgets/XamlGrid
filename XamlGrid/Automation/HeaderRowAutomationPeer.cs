using System.Linq;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using XamlGrid.Views;

namespace XamlGrid.Automation
{
	/// <summary>
	/// Exposes a <see cref="Views.HeaderRow"/> object to UI automation.
	/// </summary>
	public class HeaderRowAutomationPeer : ItemsControlAutomationPeer, ITableProvider
	{
		/// <summary>
		/// Exposes a <see cref="Views.HeaderRow"/> object to UI automation.
		/// </summary>
		/// <param name="owner">The <see cref="Views.HeaderRow"/> to associate with this <see cref="HeaderCellAutomationPeer"/>.</param>
		public HeaderRowAutomationPeer(HeaderRow owner) : base(owner)
		{

		}


		private HeaderRow HeaderRow
		{
			get
			{
				return (HeaderRow) this.Owner;
			}
		}


		/// <summary>
		/// Returns the control type for the <see cref="T:System.Windows.UIElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType"/>.
		/// </summary>
		/// <returns>
		/// A value of the enumeration.
		/// </returns>
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Header;
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
		/// Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/> for a data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> collection of this <see cref="T:System.Windows.Controls.ItemsControl"/>.
		/// </summary>
		/// <param name="item">The data item that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/>.</param>
		/// <returns>
		/// An object that exposes the data item to UI automation.
		/// </returns>
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new HeaderCellAutomationPeer(item, this);
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
			if (row == 0 && column < this.HeaderRow.Items.Count)
			{
				return this.ProviderFromPeer(this.CreateItemAutomationPeer(this.HeaderRow.Items[column]));
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
				return this.HeaderRow.Items.Count;
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
				return 1;
			}
		}

		/// <summary>
		/// Returns a collection of UI Automation providers that represents all the column headers in a table.
		/// </summary>
		/// <returns>An array of UI automation providers.</returns>
		public IRawElementProviderSimple[] GetColumnHeaders()
		{
			return (from item in this.HeaderRow.Items 
					select this.ProviderFromPeer(new HeaderCellAutomationPeer(item, this))).ToArray();
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
				return RowOrColumnMajor.ColumnMajor;
			}
		}
	}
}
