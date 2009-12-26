using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Company.DataGrid.Core;
using Company.DataGrid.Views;

namespace Company.DataGrid.Controllers
{
	public class HeaderCellContoller : Controller
	{
		public HeaderCellContoller(object viewComponent) : base(viewComponent.GetHashCode().ToString(), viewComponent)
		{

		}

		public override void OnRegister()
		{
			base.OnRegister();

			this.HeaderCell.Checked += this.HeaderCell_Checked;
			this.HeaderCell.Unchecked += this.HeaderCell_Unchecked;
			this.HeaderCell.Indeterminate += this.HeaderCell_Indeterminate;
		}

		public override void OnRemove()
		{
			base.OnRemove();

			this.HeaderCell.Checked -= this.HeaderCell_Checked;
			this.HeaderCell.Unchecked -= this.HeaderCell_Unchecked;
			this.HeaderCell.Indeterminate -= this.HeaderCell_Indeterminate;
		}

		private void HeaderCell_Checked(object sender, RoutedEventArgs e)
		{
			this.SendNotification(Notifications.SORTING_REQUESTED,
			                      new SortDescription(this.HeaderCell.Column.Binding.Path.Path, ListSortDirection.Descending));
		}

		private void HeaderCell_Unchecked(object sender, RoutedEventArgs e)
		{
			this.SendNotification(Notifications.SORTING_REQUESTED,
			                      new SortDescription(this.HeaderCell.Column.Binding.Path.Path, ListSortDirection.Ascending));
		}

		private void HeaderCell_Indeterminate(object sender, RoutedEventArgs e)
		{
			if (this.HeaderCell.Column == null)
			{
				return;
			}
			this.SendNotification(Notifications.SORTING_REQUESTED,
			                      new SortDescription { PropertyName = this.HeaderCell.Column.Binding.Path.Path },
			                      NotificationTypes.REMOVED_SORTING);
		}

		public HeaderCell HeaderCell
		{
			get
			{
				return (HeaderCell) this.ViewComponent;
			}
		}

		public override IList<string> ListNotificationInterests()
		{
			return new List<string> { Notifications.SORTED };
		}

		public override void HandleNotification(INotification notification)
		{
			SortDescription sortDescription = (SortDescription) notification.Body;
			if (sortDescription.PropertyName != null && sortDescription.PropertyName != this.HeaderCell.Column.Binding.Path.Path)
			{
				return;
			}
			switch (notification.Name)
			{
				case Notifications.SORTED:
					switch (notification.Type)
					{
						case NotificationTypes.SORTED:
							this.HeaderCell.IsChecked = sortDescription.Direction == ListSortDirection.Descending ? true : false;
							break;
						case NotificationTypes.REMOVED_SORTING:
							this.HeaderCell.IsChecked = null;
							break;
					}
					break;
			}
		}
	}
}