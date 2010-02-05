﻿using System;
using System.ComponentModel;

namespace Company.DataGrid.Models
{
	public class SortDirectionEventArgs : EventArgs
	{
		public SortDirectionEventArgs(ListSortDirection? sortDirection)
		{
			this.SortDirection = sortDirection;
		}


		public ListSortDirection? SortDirection { get; private set; }
	}
}