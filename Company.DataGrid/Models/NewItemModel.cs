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

		}

		/// <summary>
		/// Gets or sets the type of the items in the underlying data source; this type is used when creating new items.
		/// </summary>
		/// <value>The type of the items in the underlying data source.</value>
		public Type ItemType
		{
			get; 
			set;
		}

		/// <summary>
		/// Creates a new item to be edited.
		/// </summary>
		/// <returns>The newly created item.</returns>
		public object CreateItem()
		{
			// a new row may be requested before any data source is specified
			if (this.ItemType == null)
			{
				return null;
			}
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

		/// <summary>
		/// Adds the specified item to the underlying data source.
		/// </summary>
		/// <param name="item">The item to add.</param>
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
			throw new NotSupportedException("The source collection does not support adding elements.");
		}

		/// <summary>
		/// Sets the underlying data source to add items to.
		/// </summary>
		/// <param name="dataSource">The data source to add items to.</param>
		public void SetSource(ICollectionView dataSource)
		{
			// TODO: whenever the source is changed the data context of the new row must be updated, if it's visible
			// a few questions:
			// 1. Bind the grid to an addable source, show the new row, then bind to an unaddable source - should an exception be thrown here as well? Hide the new row (people will wonder)?
			this.collectionView = dataSource;
		}
	}
}
