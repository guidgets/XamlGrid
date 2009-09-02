using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using Company.DataGrid.Core;

namespace Company.DataGrid.Controllers
{
	public class DataGridController : Controller
	{
		public new const string NAME = "DataGridController";

		public DataGridController(object viewComponent) : base(NAME, viewComponent)
		{
			this.DataGrid.DataSourceChanged += this.DataGrid_DataSourceChanged;
			this.DataGrid.ItemsSourceChanged += this.DataGrid_ItemsChanged;
			this.DataGrid.Columns.CollectionChanged += this.Columns_CollectionChanged;
		}

		public Views.DataGrid DataGrid
		{
			get
			{
				return (Views.DataGrid) this.ViewComponent;
			}
		}

		public override IList<string> ListNotificationInterests()
		{
			return new List<string> { Notifications.DATA_WRAPPED };
		}

		public override void HandleNotification(INotification notification)
		{
			switch (notification.Name)
			{
				case Notifications.DATA_WRAPPED:
					this.DataGrid.ItemsSource = (IEnumerable) notification.Body;
					break;
			}
		}

		private void DataGrid_DataSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == null || e.NewValue is IEnumerable)
			{
				this.SendNotification(Notifications.DATA_WRAPPING_REQUESTED, e.NewValue);
			}
		}

		private void DataGrid_ItemsChanged(object sender, DependencyPropertyChangedEventArgs e1)
		{
			this.SendNotification(Notifications.ITEMS_SOURCE_CHANGED, this.DataGrid.ItemsSource);
		}

		private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.SendNotification(Notifications.COLUMNS_CHANGED, e);
		}
	}
}