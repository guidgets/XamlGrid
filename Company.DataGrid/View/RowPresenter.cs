using System.Collections.Generic;
using System.Windows.Controls;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.View
{
	public class RowPresenter : ContentPresenter
	{
		public IEnumerable<Column> Columns
		{
			get;
			set;
		}
	}
}
