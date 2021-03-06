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
// File:	DataCommand.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Guidgets.XamlGrid.Core;
using Guidgets.XamlGrid.Models;

namespace Guidgets.XamlGrid.Controllers
{
	public class DataCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			base.Execute(notification);
			DataModel dataModel;
			switch (notification.Code)
			{
				case Notifications.DataSourceChanged:
					dataModel = (DataModel) DataGridFacade.Instance.RetrieveModel(DataModel.NAME);
					dataModel.Create((IEnumerable) notification.Body);
					break;
				case Notifications.ColumnsChanged:
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
