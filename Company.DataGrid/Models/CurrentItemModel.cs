using System;
using System.ComponentModel;
using Company.DataGrid.Core;

namespace Company.DataGrid.Models
{
	public class CurrentItemModel : Model
	{
		public new const string NAME = "currentItemModel";

		private ICollectionView collectionView;

		public CurrentItemModel() : base(NAME)
		{

		}

		public object CurrentItem
		{
			get
			{
				return this.collectionView.CurrentItem;
			}
		}

		public void SetCollectionView(ICollectionView newCollectionView)
		{
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
			this.SendNotification(Notifications.CURRENT_ITEM_CHANGED, this.collectionView.CurrentItem);
		}

		public bool MoveCurrentToFirst()
		{
			return this.collectionView.MoveCurrentToFirst();
		}

		public bool MoveCurrentToPrevious()
		{
			return this.collectionView.MoveCurrentToPrevious();
		}

		public bool MoveCurrentToNext()
		{
			return this.collectionView.MoveCurrentToNext();
		}

		public bool MoveCurrenttoLast()
		{
			return this.collectionView.MoveCurrentToLast();
		}

		public bool MoveCurrentTo(object item)
		{
			return this.collectionView.MoveCurrentTo(item);
		}

		public bool MoveCurrentToPosition(int position)
		{
			return this.collectionView.MoveCurrentToPosition(position);
		}
	}
}
