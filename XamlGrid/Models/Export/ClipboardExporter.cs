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
// File:	ClipboardExporter.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows;
using XamlGrid.Aspects;
using XamlGrid.Views;

namespace XamlGrid.Models.Export
{
	/// <summary>
	/// Represents an exporter to the system clipboard.
	/// </summary>
	public class ClipboardExporter : Exporter
	{
		/// <summary>
		/// Exports the specified information about the data, borders, colors, fonts, etc. of the <see cref="Cell"/>s of a <see cref="DataGrid"/>.
		/// </summary>
		/// <param name="exportInfo">The information to export.</param>
		[Validate]
		public override void Export([NotNull] IEnumerable<List<CellInfo>> exportInfo)
		{
			StringBuilder copy = new StringBuilder();
			foreach (List<CellInfo> rowInfo in exportInfo)
			{
				foreach (CellInfo cellInfo in rowInfo)
				{
					copy.Append(cellInfo.Value).Append('\t');
				}
				copy.Append(Environment.NewLine);
			}
			try
			{
				Clipboard.SetText(copy.ToString());
			}
			catch (SecurityException)
			{
				// access to the clipboard may be denied by the user
			}
		}
	}
}
