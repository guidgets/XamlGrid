﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Company.DataGrid.Core;
using Company.DataGrid.Models;

namespace Company.DataGrid.Controllers
{
	public class DataCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			DataModel dataModel;
			switch (notification.Name)
			{
				case Notifications.DATA_WRAPPING_REQUESTED:
					dataModel = (DataModel) DataGridFacade.Instance.RetrieveModel(DataModel.NAME);
					dataModel.Create((IEnumerable) notification.Body);
					break;
				case Notifications.COLUMNS_CHANGED:
					NotifyCollectionChangedEventArgs e = (NotifyCollectionChangedEventArgs) notification.Body;
					IEnumerable<string> oldPropertyPaths = e.OldItems == null ? null : from Column column in e.OldItems
																					   where column.Binding != null
																					   select column.Binding.Path.Path;
					IEnumerable<string> newPropertyPaths = e.NewItems == null ? null : from Column column in e.NewItems
																					   where column.Binding != null
																					   select column.Binding.Path.Path;
					dataModel = ((DataModel) DataGridFacade.Instance.RetrieveModel(DataModel.NAME));
					dataModel.UpdatePropertyPaths(e.Action, oldPropertyPaths, newPropertyPaths);
					break;
			}
		}
	}
}
