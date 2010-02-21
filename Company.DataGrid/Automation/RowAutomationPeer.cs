using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;

namespace Company.DataGrid.Automation
{
	public class RowAutomationPeer : ItemAutomationPeer, ITableProvider, ITableItemProvider, ISelectionProvider, ISelectionItemProvider, IScrollItemProvider, IMultipleViewProvider
	{
		public RowAutomationPeer(UIElement item) : base(item)
		{
		}

		public RowAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer) : base(item, itemsControlAutomationPeer)
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


		private IList<KeyValuePair<object, ItemAutomationPeer>> itemPeerStorage;

		private static ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			ItemAutomationPeer peer = null;
			if (item is UIElement)
			{
				peer = CreatePeerForElement((UIElement) item) as ItemAutomationPeer;
			}
			return peer;
		}

		private ItemAutomationPeer CreatePeerForIndex(object item, int index)
		{
			ItemsControl owner = (ItemsControl) this.Owner;
			UIElement element = owner.ItemContainerGenerator.ContainerFromIndex(index) as UIElement;
			ItemAutomationPeer peer;
			if (element == null)
			{
				peer = CreateItemAutomationPeer(item);
			}
			else
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
						ItemAutomationPeer peer = this.CreatePeerForIndex(item, i);
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
						ItemAutomationPeer peer2 = this.CreatePeerForIndex(item, i);
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
			if (patternInterface == PatternInterface.Scroll)
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
					AutomationPeer orCreateAutomationPeer = CreatePeerForElement(viewer);
					if ((orCreateAutomationPeer != null) && (orCreateAutomationPeer is IScrollProvider))
					{
						return (IScrollProvider) orCreateAutomationPeer;
					}
				}
			}
			return base.GetPattern(patternInterface);
		}
	}
}
