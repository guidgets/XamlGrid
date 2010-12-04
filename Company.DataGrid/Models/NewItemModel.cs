using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Company.Widgets.Core;

namespace Company.Widgets.Models
{
	public class NewItemModel : Model
	{
		public new static readonly string NAME = typeof(NewItemModel).Name;
		private ICollectionView collectionView;


		public NewItemModel() : base(NAME)
		{
			this.ItemType = typeof(object);
		}

		public Type ItemType
		{
			get; 
			set;
		}

		public object CreateItem()
		{
			// TODO: signal the data grid to raise an event for a new item
			object newItem = (from constructor in this.ItemType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
							  where constructor.GetParameters().Length == 0
							  select constructor.Invoke(null)).FirstOrDefault();
			if (newItem == null)
			{
				const string error = "A new item cannot be created because the type of {0} " +
				                     "does not have a parameterless constructor.";
				throw new MissingMemberException(string.Format(error, this.ItemType.FullName));
			}
			this.SendNotification(Notifications.NEW_ITEM_ADDED, newItem);
			return newItem;
		}

		public void AddItem(object item)
		{
			if (this.collectionView == null)
			{
				return;
			}
			Type enumerableType = this.collectionView.SourceCollection.GetType();
			if (this.collectionView.SourceCollection is IList || enumerableType.GetInterface(typeof(ICollection<>).FullName, false) != null)
			{
				enumerableType.GetMethod("Add").Invoke(this.collectionView.SourceCollection, new[] { item });
				if (!(this.collectionView.SourceCollection is INotifyCollectionChanged))
				{
					this.collectionView.Refresh();
				}
				this.CreateItem();
			}
			// TODO: where and what is the best way to check whether the enumerable supports adding items?
			// throw new NotSupportedException("The source collection does not support adding elements.");
		}

		public void SetSource(ICollectionView newCollectionView)
		{
			// TODO: whenever the source is changed the data context of the new row must be updated
			// a few questions:
			// 1. Should a model for a new item be created at all if the underlying collection does not support adding? - the new row may request its creation when it's loaded and visible
			// 2. Bind the grid to an addable source, show the new row, then bind to an unaddable source - should an exception be thrown here as well? Hide the new row (people will wonder)?
			this.collectionView = newCollectionView;
		}
	}
}
