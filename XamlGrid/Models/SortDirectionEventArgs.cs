using System;
using System.ComponentModel;

namespace XamlGrid.Models
{
	public class SortDirectionEventArgs : EventArgs
	{
		public SortDirectionEventArgs(ListSortDirection? sortDirection)
		{
			this.SortDirection = sortDirection;
		}


		public virtual ListSortDirection? SortDirection { get; private set; }
	}
}