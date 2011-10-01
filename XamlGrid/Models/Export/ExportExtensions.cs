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
// File:	ExportExtensions.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using XamlGrid.Aspects;
using XamlGrid.Views;

namespace XamlGrid.Models.Export
{
	/// <summary>
	/// Contains convenient methods to export the data of a <see cref="DataGrid"/>.
	/// </summary>
	public static class ExportExtensions
	{
		/// <summary>
		/// Copies the selected items of the specified data grid.
		/// </summary>
		/// <param name="dataGrid">The data grid to copy the selected items of.</param>
		[Validate]
		public static void Copy([NotNull] this DataGrid dataGrid)
		{
			ClipboardExporter clipboardExporter = new ClipboardExporter();
			clipboardExporter.Options = dataGrid.CopyOptions;
			clipboardExporter.ExportSelected(dataGrid);
		}
	}
}
