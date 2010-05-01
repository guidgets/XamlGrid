using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Company.DataGrid.Core;

namespace Company.DataGrid.Models
{
	/// <summary>
	/// Represents a <see cref="Model"/> that executes all sorting logic over a given set of data.
	/// </summary>
	public class SortingModel : Model
	{
		public new const string NAME = "sortingModel";

		private bool notificationsSuspended;
		private ICollectionView collectionView;
		private readonly SortDescriptionCollection sortDescriptionsStore;
		private List<Notification> notificationsStore;

		/// <summary>
		/// Represents a <see cref="Model"/> that executes all sorting logic over a given set of data.
		/// </summary>
		public SortingModel() : base(NAME)
		{
			this.sortDescriptionsStore = new SortDescriptionCollection();
			this.notificationsSuspended = true;
			this.notificationsStore = new List<Notification>();
		}

		/// <summary>
		/// Gets the sort descriptions which the <see cref="SelectionModel"/> currently uses to sort its items.
		/// </summary>
		/// <value>The sort descriptions.</value>
		public virtual SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this.collectionView != null ? this.collectionView.SortDescriptions : this.sortDescriptionsStore;
			}
		}

		/// <summary>
		/// Sets the collection view which items the <see cref="SortingModel"/> sorts.
		/// </summary>
		/// <param name="newCollectionView">The new collection view to be sorted by the <see cref="SelectionModel"/>.</param>
		public virtual void SetCollectionView(ICollectionView newCollectionView)
		{
			if (this.collectionView == newCollectionView)
			{
				return;
			}
			if (this.collectionView != null)
			{
				((INotifyCollectionChanged) this.collectionView.SortDescriptions).CollectionChanged -= this.SortingModel_CollectionChanged;				
			}
			this.collectionView = newCollectionView;
			if (this.collectionView == null)
			{
				return;
			}
			((INotifyCollectionChanged) this.collectionView.SortDescriptions).CollectionChanged += this.SortingModel_CollectionChanged;
			foreach (SortDescription sortDescription in this.sortDescriptionsStore)
			{
				this.collectionView.SortDescriptions.Add(sortDescription);
			}
			this.sortDescriptionsStore.Clear();
		}

		/// <summary>
		/// Starts the notifications sent when sorting occurs.
		/// </summary>
		public virtual void StartNotifications()
		{
			if (this.notificationsStore == null)
			{
				return;
			}
			this.notificationsSuspended = false;
			foreach (Notification notification in this.notificationsStore)
			{
				this.SendNotification(notification.Name, notification.Body, notification.Type);
			}
			this.notificationsStore.Clear();
			this.notificationsStore = null;
		}

		private void SortingModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (object sortDescription in e.NewItems)
					{
						this.SendNotificationForSorting(sortDescription, null);
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					for (int index = e.OldItems.Count - 1; index >= 0; index--)
					{
						this.SendNotificationForSorting(e.OldItems[index], NotificationTypes.REMOVED_SORTING);
					}
					break;
				case NotifyCollectionChangedAction.Replace:
					for (int index = e.OldItems.Count - 1; index >= 0; index--)
					{
						this.SendNotificationForSorting(e.OldItems[index], NotificationTypes.REMOVED_SORTING);
					}
					foreach (object sortDescription in e.NewItems)
					{
						this.SendNotificationForSorting(sortDescription, null);
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					this.SendNotificationForSorting(new SortDescription(), NotificationTypes.REMOVED_SORTING);
					break;
			}
		}

		/// <summary>
		/// Sorts the items of the <see cref="SortingModel"/> by the specified property name in the specified direction.
		/// </summary>
		/// <param name="sortDescription">The sort description to use for sorting.</param>
		public virtual void Sort(ExtendedSortDescription sortDescription)
		{
			this.Sort(sortDescription.Property, sortDescription.SortDirection, sortDescription.ClearPreviousSorting);
		}

		/// <summary>
		/// Sorts the items of the <see cref="SortingModel"/> by the specified property name in the specified direction.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="sortDirection">The sort direction.</param>
		/// <param name="clearPreviousSorting"></param>
		public virtual void Sort(string propertyName, ListSortDirection? sortDirection, bool clearPreviousSorting)
		{
			if (clearPreviousSorting)
			{
				for (int i = this.SortDescriptions.Count - 1; i >= 0; i--)
				{
					if (this.SortDescriptions[i].PropertyName != propertyName)
					{
						this.SortDescriptions.RemoveAt(i);
					}
				}
			}
			IEnumerable<SortDescription> sortDescriptions = (from sortDescription in this.SortDescriptions
															 where sortDescription.PropertyName == propertyName
															 select sortDescription);
			bool isUnsorted = !sortDescriptions.Any();
			if (sortDirection == null && isUnsorted)
			{
				return;
			}
			if (isUnsorted)
			{
				this.SortDescriptions.Add(new SortDescription(propertyName, sortDirection.Value));
			}
			else
			{
				if (sortDirection == null)
				{
					this.SortDescriptions.Remove(sortDescriptions.First());
				}
				else
				{
					this.SortDescriptions[this.SortDescriptions.IndexOf(sortDescriptions.First())] = new SortDescription(propertyName, sortDirection.Value);
				}
			}
		}

		/// <summary>
		/// Refreshes the sorting, if any, by the specified property path.
		/// </summary>
		/// <param name="propertyPath">The property path to sort by.</param>
		public virtual void RefreshIfSorted(string propertyPath)
		{
			if (this.collectionView == null)
			{
				return;
			}
			if ((from sortDescription in this.SortDescriptions
				 where sortDescription.PropertyName == propertyPath
				 select sortDescription).Any())
			{
				this.collectionView.Refresh();
			}
		}

		private void SendNotificationForSorting(object sortDescription, string notificationType)
		{
			// TODO: remove the store for notifications; make the header cells ask for their sorted state instead
			if (this.notificationsSuspended)
			{
				notificationsStore.Add(new Notification(Notifications.SORTED, sortDescription, notificationType));
			}
			else
			{
				this.SendNotification(Notifications.SORTED, sortDescription, notificationType);				
			}
		}
	}
}
