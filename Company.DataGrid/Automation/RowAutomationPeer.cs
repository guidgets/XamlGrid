using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;
using Company.Widgets.Core;
using Company.Widgets.Views;

namespace Company.Widgets.Automation
{
	/// <summary>
	/// Exposes a <see cref="Views.Row"/> object to UI automation.
	/// </summary>
	public class RowAutomationPeer : ItemAutomationPeer, ITableProvider, ITableItemProvider, ISelectionItemProvider
	{
		/// <summary>
		/// Exposes a <see cref="Views.Row"/> object to UI automation.
		/// </summary>
		/// <param name="item">The <see cref="DataGrid"/> to associate with this <see cref="RowAutomationPeer"/>.</param>
		public RowAutomationPeer(Row item) : base(item)
		{

		}

		/// <summary>
		/// Exposes a <see cref="Views.Row"/> object to UI automation.
		/// </summary>
		/// <param name="item">The data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/>.</param>
		/// <param name="itemsControlAutomationPeer">The <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer"/> that is associated with the <see cref="T:System.Windows.Controls.ItemsControl"/> that holds the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> collection.</param>
		public RowAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer) : base(item, itemsControlAutomationPeer)
		{

		}


		private Row RowElement
		{
			get
			{
				return (Row) this.Owner;
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
			return AutomationControlType.DataItem;
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
				return 0;
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
			get { return this.RowElement.Index; }
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
		/// Adds the current element to the collection of selected items.
		/// </summary>
		public void AddToSelection()
		{
			if (this.RowElement.IsSelected)
			{
				return;
			}
			this.RowElement.IsSelected = true;
			this.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, false, true);
			this.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
		}

		/// <summary>
		/// Removes the current element from the collection of selected items.
		/// </summary>
		public void RemoveFromSelection()
		{
			if (this.RowElement.IsSelected)
			{
				return;
			}
			this.RowElement.IsSelected = false;
			this.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, true, false);
			this.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
		}

		/// <summary>
		/// Clears any existing selection and then selects the current element.
		/// </summary>
		public void Select()
		{
			IController rowController = DataGridFacade.Instance.RetrieveController(this.RowElement.GetHashCode().ToString());
			if (rowController == null)
			{
				return;
			}
			((INotifier) rowController).SendNotification(Notifications.SelectingItems, this.RowElement.DataContext, NotificationTypes.ClearSelection);
			this.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, false, true);
			this.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
		}

		/// <summary>
		/// Gets a value that indicates whether an item is selected.
		/// </summary>
		/// <value></value>
		/// <returns><c>true</c> if the element is selected; otherwise, <c>false</c>.</returns>
		public bool IsSelected
		{
			get
			{
				return this.RowElement.IsSelected;
			}
		}

		/// <summary>
		/// Gets the UI automation provider that implements <see cref="T:System.Windows.Automation.Provider.ISelectionProvider"/> and acts as the container for the calling object.
		/// </summary>
		/// <value></value>
		/// <returns>The UI automation provider.</returns>
		public IRawElementProviderSimple SelectionContainer
		{
			get
			{
				return this.ProviderFromPeer(this.ItemsControlAutomationPeer);
			}
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
			if (row == 0 && column < this.RowElement.Items.Count)
			{
				return this.ProviderFromPeer((from keyValuePair in itemPeerStorage 
											  where keyValuePair.Key == this.RowElement.Items[column] 
											  select keyValuePair.Value).First());
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
				return this.RowElement.Items.Count;
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
			return new IRawElementProviderSimple[0];
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


		private IList<KeyValuePair<object, ItemAutomationPeer>> itemPeerStorage;

		private ItemAutomationPeer CreatePeerForIndex(int index)
		{
			ItemsControl owner = (ItemsControl) this.Owner;
			UIElement element = owner.ItemContainerGenerator.ContainerFromIndex(index) as UIElement;
			ItemAutomationPeer peer = null;
			if (element != null)
			{
				peer = CreatePeerForElement(element) as ItemAutomationPeer;
			}
			return peer;
		}

		/// <summary>
		/// Gets the collection of child elements of the <see cref="T:System.Windows.Controls.ItemsControl"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer"/>.
		/// </summary>
		/// <returns>The collection of child elements.</returns>
		protected override List<AutomationPeer> GetChildrenCore()
		{
			if (this.Owner == null)
			{
				return null;
			}
			List<AutomationPeer> list = null;
			ItemsControl owner = (ItemsControl) this.Owner;
			ItemCollection items = owner.Items;
			int count = items.Count;
			if (count > 0)
			{
				if (this.itemPeerStorage == null)
				{
					this.itemPeerStorage = new List<KeyValuePair<object, ItemAutomationPeer>>(count);
				}
				list = new List<AutomationPeer>(count);
				for (int i = 0; i < items.Count; i++)
				{
					object item = items[i];
					if (this.itemPeerStorage.Count > i)
					{
						KeyValuePair<object, ItemAutomationPeer> pair = this.itemPeerStorage[i];
						ItemAutomationPeer peer = this.CreatePeerForIndex(i);
						if (peer != pair.Value)
						{
							if (((peer != null) && (pair.Value != null)) && (peer.Owner == pair.Value.Owner))
							{
								list.Add(pair.Value);
							}
							else
							{
								this.itemPeerStorage[i] = new KeyValuePair<object, ItemAutomationPeer>(item, peer);
								if (peer != null)
								{
									list.Add(peer);
								}
							}
						}
						else if (peer != null)
						{
							list.Add(peer);
						}
					}
					else
					{
						ItemAutomationPeer peer2 = this.CreatePeerForIndex(i);
						KeyValuePair<object, ItemAutomationPeer> pair2 =
							new KeyValuePair<object, ItemAutomationPeer>(item, peer2);
						this.itemPeerStorage.Add(pair2);
						if (peer2 != null)
						{
							list.Add(peer2);
						}
					}
				}
			}
			return list;
		}

		/// <summary>
		/// Gets a control pattern for the <see cref="T:System.Windows.Controls.ItemsControl"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer"/>.
		/// </summary>
		/// <param name="patternInterface">One of the enumeration values that indicates the control pattern.</param>
		/// <returns>
		/// The object that implements the pattern interface, or null if the specified pattern interface is not implemented by this peer.
		/// </returns>
		public override object GetPattern(PatternInterface patternInterface)
		{
			switch (patternInterface)
			{
				case PatternInterface.Scroll:
					{
						ItemsControl owner = (ItemsControl) this.Owner;
						UIElement itemsHost = owner.GetItemsHost();
						ScrollViewer viewer = null;
						while ((itemsHost != null) && (itemsHost != owner))
						{
							itemsHost = VisualTreeHelper.GetParent(itemsHost) as UIElement;
							viewer = itemsHost as ScrollViewer;
							if (viewer != null)
							{
								break;
							}
						}
						if (viewer != null)
						{
							AutomationPeer automationPeer = CreatePeerForElement(viewer);
							if ((automationPeer != null) && (automationPeer is IScrollProvider))
							{
								return (IScrollProvider) automationPeer;
							}
						}
					}
					break;
				case PatternInterface.SelectionItem:
					return this;
			}
			return base.GetPattern(patternInterface);
		}
	}
}
