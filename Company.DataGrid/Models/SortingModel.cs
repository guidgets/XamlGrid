﻿using System.Collections.Generic;
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
		public SortDescriptionCollection SortDescriptions
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
		public void SetCollectionView(ICollectionView newCollectionView)
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
		public void StartNotifications()
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
				case NotifyCollectionChangedAction.Remove:
					this.SendNotificationForSorting(e.OldItems[0], NotificationTypes.REMOVED_SORTING);
					break;
				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Replace:
					this.SendNotificationForSorting(e.NewItems[0], NotificationTypes.SORTED);
					break;
				case NotifyCollectionChangedAction.Reset:
					this.SendNotificationForSorting(new SortDescription(), NotificationTypes.REMOVED_SORTING);
					break;
			}
		}

		/// <summary>
		/// Refreshes the sorting, if any, by the specified property path.
		/// </summary>
		/// <param name="propertyPath">The property path to sort by.</param>
		public void RefreshIfSorted(string propertyPath)
		{
			if (this.collectionView == null)
			{
				return;
			}
			bool sortedByPropertyPath = (from sortDescription in this.SortDescriptions
										 where sortDescription.PropertyName == propertyPath
										 select sortDescription).Any();
			if (sortedByPropertyPath)
			{
				this.collectionView.Refresh();
			}
		}

		/// <summary>
		/// Sorts the items of the <see cref="SelectionModel"/> by the specified property name in the specified direction.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="sortDirection">The sort direction.</param>
		public void Sort(string propertyName, ListSortDirection? sortDirection)
		{
			IEnumerable<SortDescription> sortDescriptions = (from sortDescription in this.SortDescriptions
															 where sortDescription.PropertyName == propertyName
															 select sortDescription);
			bool isUnsorted = !sortDescriptions.Any();
			if (sortDirection == null && isUnsorted)
			{
				return;
			}
			SortDescription description;
			string notificationType = NotificationTypes.SORTED;
			if (isUnsorted)
			{
				description = new SortDescription(propertyName, sortDirection.Value);
				this.SortDescriptions.Add(description);
			}
			else
			{
				if (sortDirection == null)
				{
					this.SortDescriptions.Remove(sortDescriptions.First());
					description = new SortDescription();
					notificationType = NotificationTypes.REMOVED_SORTING;
				}
				else
				{
					description = new SortDescription(propertyName, sortDirection.Value);
					this.SortDescriptions[this.SortDescriptions.IndexOf(sortDescriptions.First())] = description;
				}
			}
			this.SendNotificationForSorting(description, notificationType);
		}

		private void SendNotificationForSorting(object sortDescription, string notificationType)
		{
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
