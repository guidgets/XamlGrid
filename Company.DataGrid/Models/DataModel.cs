using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Company.Widgets.Core;

namespace Company.Widgets.Models
{
	public class DataModel : Model
	{
		public new static readonly string NAME = typeof(DataModel).Name;

		private readonly ObservableItemCollection<object> observableItemCollection;
		private ICollectionView collectionView;

		public DataModel() : base(NAME)
		{
			this.observableItemCollection = new ObservableItemCollection<object>();
			this.observableItemCollection.ThrowExceptionOnInvalidPath = false;
			this.observableItemCollection.ItemPropertyChanged += this.ObservableItemCollection_ItemPropertyChanged;
		}

		private void ObservableItemCollection_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.ItemPropertyChanged, new[] { e.Item, e.PropertyPath });
		}

		public virtual ICollectionView Create(IEnumerable enumerable)
		{
			if (this.collectionView != null && this.collectionView.SourceCollection == enumerable)
			{
				return this.collectionView;
			}
			this.observableItemCollection.Clear();
			if (enumerable == null)
			{
				this.collectionView = null;
			}
			else
			{
				if (enumerable is ICollection || enumerable.GetType().GetInterface(typeof(ICollection<>).FullName, false) != null)
				{
					this.observableItemCollection.SetSource(from object item in enumerable
					                                        select item);
				}
				this.collectionView = DataWrapper.Wrap(enumerable);
			}
			this.SendNotification(Notifications.DataWrapped, this.collectionView);
			return this.collectionView;
		}

		public virtual void UpdatePropertyPaths(NotifyCollectionChangedAction action, IEnumerable<string> oldPropertyPaths,
												IEnumerable<string> newPropertyPaths)
		{
			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					this.observableItemCollection.AddPropertyPaths(newPropertyPaths);
					break;
				case NotifyCollectionChangedAction.Remove:
					this.observableItemCollection.RemovePropertyPaths(oldPropertyPaths);
					break;
				case NotifyCollectionChangedAction.Replace:
					this.observableItemCollection.ReplacePropertyPaths(oldPropertyPaths, newPropertyPaths);
					break;
				case NotifyCollectionChangedAction.Reset:
					this.observableItemCollection.ClearPropertyPaths();
					break;
			}
		}
	}
}
