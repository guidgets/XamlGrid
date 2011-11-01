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
// File:	ExportOptions.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System;
using System.ComponentModel;
using Guidgets.XamlGrid.Controllers;

namespace Guidgets.XamlGrid.Models.Export
{
	/// <summary>
	/// Provides options to specify which additional properties except data itself to export.
	/// </summary>
	[Flags, TypeConverter(typeof(StringToExportOptionsConverter))]
	public enum ExportOptions
	{
		/// <summary>
		/// Include only the data (values).
		/// </summary>
		None = 0,
		/// <summary>
		/// Include a header, if any, with metadata.
		/// </summary>
		Header = 1
	}
}
