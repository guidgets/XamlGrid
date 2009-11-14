using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;
using Company.DataGrid.Core;

namespace Company.DataGrid.Models
{
	public class SelectionModel : Model
	{
		public new const string NAME = "selectionModel";

		private SelectionMode selectionMode;

		public SelectionModel() : base (NAME)
		{
			this.SelectedItems = new SelectedItemsCollection();
		}

		public SelectedItemsCollection SelectedItems
		{
			get; 
			private set;
		}

		public SelectionMode SelectionMode
		{
			get
			{
				return this.selectionMode;
			}
			set
			{
				if (this.selectionMode != value)
				{
					this.selectionMode = value;
					this.SelectedItems.SelectionMode = this.selectionMode;
					this.SendNotification(Notifications.SELECTION_MODE_CHANGED, this.selectionMode);
				}
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();

			this.SelectedItems.CollectionChanged += this.SelectedItems_CollectionChanged;
		}

		public override void OnRemove()
		{
			base.OnRemove();

			this.SelectedItems.CollectionChanged -= this.SelectedItems_CollectionChanged;
		}

		private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					this.SendNotification(Notifications.ITEMS_SELECTED, e.NewItems);
					break;
				case NotifyCollectionChangedAction.Remove:
					this.SendNotification(Notifications.ITEMS_DESELECTED, e.OldItems);
					break;
				case NotifyCollectionChangedAction.Replace:
					this.SendNotification(Notifications.ITEMS_DESELECTED, e.OldItems);
					this.SendNotification(Notifications.ITEMS_SELECTED, e.NewItems);
					break;
				case NotifyCollectionChangedAction.Reset:
					this.SendNotification(Notifications.ITEMS_DESELECTED, SelectedItems);
					break;
			}
		}
	}
}
