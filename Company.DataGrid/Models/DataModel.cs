using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Company.DataGrid.Core;

namespace Company.DataGrid.Models
{
	public class DataModel : Model
	{
		public new const string NAME = "dataModel";

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
			this.SendNotification(Notifications.REFRESH_SORTING, e.PropertyPath);
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
				this.collectionView = enumerable as ICollectionView ?? new PagedCollectionView(enumerable);
			}
			this.SendNotification(Notifications.DATA_WRAPPED, this.collectionView);
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
