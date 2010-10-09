﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Company.Widgets.Core;

namespace Company.Widgets.Models
{
	public class NewItemModel : Model
	{
		public new static readonly string NAME = typeof(NewItemModel).Name;
		private IEnumerable enumerable;


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
			this.enumerable.GetType().GetMethod("Add").Invoke(this.enumerable, new[] { item });
			// TODO: refresh the data grid if the enumerable is not an INotifyCollectionChanged
			// TODO: a thought: when wrapping a data source would it be a good idea to additionally (besides ICollectionView) wrap it in an INotifyCollectionChanged?
			// probably not because the original source is lost; but is it possible for this source to have something that valuable to the grid?
			// a possible solution would be to use an inheritor of ObservableCollection which keeps the original source as a field and synchronizes it
			// if this additional wrapper is used how to get the original source besides checking with "is"? The original must be checked whether it supports adding (or not)?
			// it may be possible to add items to the wrapper and let it throw an exception if its original cannot have items added
			this.CreateItem();
		}

		public void SetSource(IEnumerable enumerable)
		{
			// TODO: whenever the source is changed the data context of the new row must be updated
			// a few questions:
			// 1. Should a model for a new item be created at all if the underlying collection does not support adding? - the new row may request its creation when it's loaded and visible
			// 2. Bind the grid to an addable source, show the new row, then bind to an unaddable source - should an exception be thrown here as well? Hide the new row (people will wonder)?
			ICollectionView collectionView = enumerable as ICollectionView;
			this.enumerable = collectionView != null ? collectionView.SourceCollection : enumerable;
		}
	}
}
