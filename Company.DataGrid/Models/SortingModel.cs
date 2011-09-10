using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Company.Widgets.Core;

namespace Company.Widgets.Models
{
	/// <summary>
	/// Represents a <see cref="Model"/> that executes all sorting logic over a given set of data.
	/// </summary>
	public class SortingModel : Model
	{
		public new static readonly string NAME = typeof(SortingModel).Name;

		private ICollectionView collectionView;
		private readonly SortDescriptionCollection sortDescriptionsStore;

		/// <summary>
		/// Represents a <see cref="Model"/> that executes all sorting logic over a given set of data.
		/// </summary>
		public SortingModel() : base(NAME)
		{
			this.sortDescriptionsStore = new SortDescriptionCollection();
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
		/// Checks the state of the sorting (if any) at the specified property path.
		/// </summary>
		/// <param name="propertyPath">The property path to check sorting at.</param>
		public virtual void CheckSortingState(string propertyPath)
		{
			IEnumerable<SortDescription> sortDescriptions = from sortDescription in this.SortDescriptions
															where sortDescription.PropertyName == propertyPath
															select sortDescription;
			if (sortDescriptions.Any())
			{
				this.SendNotification(Notifications.Sorted, sortDescriptions.First());
			}
			else
			{
				this.SendNotification(Notifications.Sorted, new SortDescription(string.Empty, ListSortDirection.Ascending),
				                      NotificationTypes.NoSorting);
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
			IEnumerable<SortDescription> sortDescriptions = from sortDescription in this.SortDescriptions 
															where sortDescription.PropertyName == propertyName 
															select sortDescription;
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


		private void SortingModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (object sortDescription in e.NewItems)
					{
						this.SendNotification(Notifications.Sorted, sortDescription, null);
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					for (int index = e.OldItems.Count - 1; index >= 0; index--)
					{
						this.SendNotification(Notifications.Sorted, e.OldItems[index], NotificationTypes.NoSorting);
					}
					break;
				case NotifyCollectionChangedAction.Replace:
					for (int index = e.OldItems.Count - 1; index >= 0; index--)
					{
						this.SendNotification(Notifications.Sorted, e.OldItems[index], NotificationTypes.NoSorting);
					}
					foreach (object sortDescription in e.NewItems)
					{
						this.SendNotification(Notifications.Sorted, sortDescription, null);
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					this.SendNotification(Notifications.Sorted, new SortDescription(), NotificationTypes.NoSorting);
					break;
			}
		}
	}
}
