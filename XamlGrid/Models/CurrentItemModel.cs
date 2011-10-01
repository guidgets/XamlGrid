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
// File:	CurrentItemModel.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System;
using System.ComponentModel;
using XamlGrid.Core;

namespace XamlGrid.Models
{
	/// <summary>
	/// Represents a <see cref="Model"/> that manages the current item of a collection of items.
	/// </summary>
	public class CurrentItemModel : Model
	{
		public new const string NAME = "currentItemModel";

		private ICollectionView collectionView;

		/// <summary>
		/// Represents a <see cref="Model"/> that manages the current item of a collection of items.
		/// </summary>
		public CurrentItemModel() : base(NAME)
		{

		}

		/// <summary>
		/// Gets the current item of the <see cref="CurrentItemModel"/>.
		/// </summary>
		/// <value>The current item.</value>
		public virtual object CurrentItem
		{
			get
			{
				return this.collectionView.CurrentItem;
			}
		}

		/// <summary>
		/// Sets the collection view which contains the items to manage for currency.
		/// </summary>
		/// <param name="newCollectionView">The new collection view to manage for currency.</param>
		public virtual void SetCollectionView(ICollectionView newCollectionView)
		{
			if (this.collectionView == newCollectionView)
			{
				return;
			}
			if (this.collectionView != null)
			{
				this.collectionView.CurrentChanged -= this.CollectionView_CurrentChanged; 
			}
			this.collectionView = newCollectionView;
			if (this.collectionView != null)
			{
				this.collectionView.CurrentChanged += this.CollectionView_CurrentChanged;
			}
		}

		private void CollectionView_CurrentChanged(object sender, EventArgs e)
		{
			this.SendNotification(Notifications.CurrentItemChanged, this.collectionView.CurrentItem);
		}

		/// <summary>
		/// Moves the current item to first item in a collection.
		/// </summary>
		/// <returns>A value indicating if the moving was successful.</returns>
		public virtual bool MoveCurrentToFirst()
		{
			return this.collectionView.MoveCurrentToFirst();
		}

		/// <summary>
		/// Moves the current item to the previous item in a collection.
		/// </summary>
		/// <returns>A value indicating if the moving was successful.</returns>
		public virtual bool MoveCurrentToPrevious()
		{
			this.collectionView.MoveCurrentToPrevious();
			return !this.collectionView.IsCurrentBeforeFirst || this.MoveCurrentToFirst();
		}

		/// <summary>
		/// Moves the current item to the next item in a collection.
		/// </summary>
		/// <returns>A value indicating if the moving was successful.</returns>
		public virtual bool MoveCurrentToNext()
		{
			this.collectionView.MoveCurrentToNext();
			return !this.collectionView.IsCurrentAfterLast || this.MoveCurrentToLast();
		}

		/// <summary>
		/// Moves the current item to the last item in a collection.
		/// </summary>
		/// <returns>A value indicating if the moving was successful.</returns>
		public virtual bool MoveCurrentToLast()
		{
			return this.collectionView.MoveCurrentToLast();
		}

		/// <summary>
		/// Moves the current item to the specified item.
		/// </summary>
		/// <param name="item">The item to become the current one.</param>
		/// <returns>A value indicating if the moving was successful.</returns>
		public virtual bool MoveCurrentTo(object item)
		{
			return this.collectionView.MoveCurrentTo(item);
		}

		/// <summary>
		/// Moves the current item to the item at the specified position.
		/// </summary>
		/// <param name="position">The position at which the item to become the current one is located.</param>
		/// <returns>A value indicating if the moving was successful.</returns>
		public virtual bool MoveCurrentToPosition(int position)
		{
			return this.collectionView.MoveCurrentToPosition(position);
		}
	}
}
