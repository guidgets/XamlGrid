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
// File:	Exporter.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Guidgets.XamlGrid.Aspects;
using Guidgets.XamlGrid.Controllers;
using Guidgets.XamlGrid.Views;

namespace Guidgets.XamlGrid.Models.Export
{
	/// <summary>
	/// Provides the base for exporting the items of a <see cref="DataGrid"/> to an outside source.
	/// </summary>
	public abstract class Exporter
	{
		private ExportOptions options;


		/// <summary>
		/// Provides the base for exporting the items of a <see cref="DataGrid"/> to an outside source.
		/// </summary>
		protected Exporter()
		{
			options = ExportOptions.Header;
		}


		/// <summary>
		/// Gets or sets the options to use when exporting.
		/// </summary>
		/// <value>The options to use when exporting.</value>
		public virtual ExportOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}


		/// <summary>
		/// Exports all items of the specified <see cref="DataGrid"/>.
		/// </summary>
		/// <param name="dataGrid">The data grid to export the items of.</param>
		[Validate]
		public virtual void ExportAll([NotNull] DataGrid dataGrid)
		{
			this.Export(this.GetDataToExport(dataGrid, dataGrid.Items));
		}

		/// <summary>
		/// Exports the selected items of the specified <see cref="DataGrid"/>.
		/// </summary>
		/// <param name="dataGrid">The data grid to export the selected items of.</param>
		[Validate]
		public virtual void ExportSelected([NotNull] DataGrid dataGrid)
		{
			this.Export(this.GetDataToExport(dataGrid, from selectedItem in dataGrid.SelectedItems
			                                           orderby selectedItem.Index
			                                           select selectedItem.Item));
		}

		/// <summary>
		/// Exports the specified information about the data, borders, colors, fonts, etc. of the <see cref="Cell"/>s of a <see cref="DataGrid"/>.
		/// </summary>
		/// <param name="exportInfo">The information to export.</param>
		public abstract void Export(IEnumerable<List<CellInfo>> exportInfo);


		/// <summary>
		/// Gets the data to export from the specified <paramref name="dataGrid"/> and the specified <paramref name="items"/>.
		/// </summary>
		/// <param name="dataGrid">The <see cref="DataGrid"/> to export.</param>
		/// <param name="items">The items to export.</param>
		/// <returns>The values of the data to export along with optional additional properties like borders, backgrounds and fonts.</returns>
		[Validate]
		protected virtual IEnumerable<List<CellInfo>> GetDataToExport([NotNull] DataGrid dataGrid, [NotNull] IEnumerable<object> items)
		{
			List<object> itemsList = items.ToList();
			List<List<CellInfo>> dataToExport = new List<List<CellInfo>>(itemsList.Count + 1);
			if (itemsList.Count == 0)
			{
				return dataToExport;
			}
			List<Column> visibleColumns = dataGrid.Columns.Where(column => column.Visibility == Visibility.Visible).ToList();
			int visibleColumnsCount = visibleColumns.Count();
			if ((this.options & ExportOptions.Header) != ExportOptions.None)
			{
				dataToExport.Add(new List<CellInfo>(visibleColumnsCount));
				foreach (Column column in visibleColumns)
				{
					CellInfo header = CellInfo.Default;
					header.Value = column.Header;
					header.DataType = column.DataType;
					dataToExport[0].Add(header);
				}
			}
			foreach (object item in itemsList)
			{
				List<CellInfo> rowInfo = new List<CellInfo>(visibleColumnsCount);
				foreach (Column column in visibleColumns)
				{
					CellInfo cellInfo = new CellInfo(null);
					cellInfo.Value = DataBinder.GetValue(item, column.Binding.Path.Path);
					cellInfo.DataType = column.DataType;
					rowInfo.Add(cellInfo);
				}
				dataToExport.Add(rowInfo);
			}
			return dataToExport;
		}
	}
}
