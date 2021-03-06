// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
// 
// File:	NewItemModel.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Guidgets.XamlGrid.Controllers;
using Guidgets.XamlGrid.Core;

namespace Guidgets.XamlGrid.Models
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
		public void AddItem()
		{
			// a new row may be requested before any data source is specified
			if (this.ItemType == null)
			{
				return;
			}
			NewItemEventArgs newItemEventArgs = new NewItemEventArgs();
			this.SendNotification(Notifications.NewItemCustom, newItemEventArgs);
			object newItem = newItemEventArgs.NewItem ??
			                 (from constructor in this.ItemType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
			                  where constructor.GetParameters().Length == 0
			                  select constructor.Invoke(null)).FirstOrDefault();
			if (newItem == null)
			{
				const string error = "A new item cannot be created because the type of {0} " +
									 "does not have a parameterless constructor.";
				throw new MissingMemberException(string.Format(error, this.ItemType.FullName));
			}
			// not checking of the underlying source can be added to because there may be no source at at all yet
			this.SendNotification(Notifications.NewItemAdded, newItem);
			this.Data = newItem;
		}

		/// <summary>
		/// Adds the specified item to the underlying data source.
		/// </summary>
		public void CommitItem()
		{
			if (this.Data == null)
			{
				throw new InvalidOperationException("A new item must be added before being committed.");
			}
			// simply return because a commit may be requested when there is no data source
			if (this.collectionView == null)
			{
				return;
			}
			Type enumerableType = this.collectionView.SourceCollection.GetType();
			if (this.collectionView.SourceCollection is IList || enumerableType.GetInterface(typeof(ICollection<>).FullName, false) != null)
			{
				enumerableType.GetMethod("Add").Invoke(this.collectionView.SourceCollection, new[] { this.Data });
				if (!(this.collectionView.SourceCollection is INotifyCollectionChanged))
				{
					this.collectionView.Refresh();
				}
				this.AddItem();
				return;
			}
			throw new NotSupportedException("The addition of a new item was requested but " +
			                                "the source collection does not support adding elements.");
		}

		/// <summary>
		/// Sets the underlying data source to add items to.
		/// </summary>
		/// <param name="dataSource">The data source to add items to.</param>
		public void SetSource(ICollectionView dataSource)
		{
			// TODO: what to do if there already is a new item?
			this.collectionView = dataSource;
			if (this.collectionView != null)
			{
				this.ItemType = this.collectionView.GetElementType();
			}
		}
	}
}
