using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Company.Widgets.Aspects;
using Company.Widgets.Controllers;
using Company.Widgets.Views;

namespace Company.Widgets.Models.Export
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
					dataToExport[0].Add(header);
				}
			}
			ItemsControl firstRow = (from object item in itemsList
			                         let row = dataGrid.ItemContainerGenerator.ContainerFromItem(item)
			                         where row != null
			                         select row).OfType<ItemsControl>().FirstOrDefault() ??
			                        (from item in dataGrid.Items
			                         let row = dataGrid.ItemContainerGenerator.ContainerFromItem(item)
			                         where row != null
			                         select row).OfType<ItemsControl>().FirstOrDefault();
			Dictionary<Column, CellInfo> backups = new Dictionary<Column, CellInfo>(visibleColumns.Count());
			foreach (Column column in visibleColumns)
			{
				CellInfo backup = CellInfo.Default;
				if (firstRow != null)
				{
					Control cell = firstRow.ItemContainerGenerator.ContainerFromItem(column) as Control;
					if (cell != null)
					{
						Font font = new Font(cell.FontFamily, cell.FontSize, cell.FontStretch, cell.FontStyle, cell.FontWeight, cell.Foreground);
						backup = new CellInfo(null, cell.Background, cell.BorderBrush, font);
					}
				}
				backups.Add(column, backup);
			}
			foreach (object item in itemsList)
			{
				List<CellInfo> rowInfo = new List<CellInfo>(visibleColumnsCount);
				ItemsControl row = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as ItemsControl;
				foreach (Column column in visibleColumns)
				{
					CellInfo cellInfo = backups[column];
					if (row != null)
					{
						Control cell = row.ItemContainerGenerator.ContainerFromItem(column) as Control;
						if (cell != null)
						{
							Font font = new Font(cell.FontFamily, cell.FontSize, cell.FontStretch, cell.FontStyle, cell.FontWeight,
							                     cell.Foreground);
							cellInfo = new CellInfo(null, cell.Background, cell.BorderBrush, font);
						}
					}
					cellInfo.Value = DataBinder.GetValue(item, column.Binding.Path.Path);
					rowInfo.Add(cellInfo);
				}
				dataToExport.Add(rowInfo);
			}
			return dataToExport;
		}
	}
}
