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
// File:	KeyHelper.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System;
using System.Windows.Input;

namespace Guidgets.XamlGrid.Controllers
{
	/// <summary>
	/// Contains common functionalities needed when working with the keys of a keyboard.
	/// </summary>
	public static class KeyHelper
	{
		/// <summary>
		/// Gets the modifier that is used to issue a command to an application or a component.
		/// </summary>
		public static readonly ModifierKeys CommandModifier = Environment.OSVersion.Platform == PlatformID.MacOSX
		                                                      	? ModifierKeys.Apple
		                                                      	: ModifierKeys.Control;
	}
}
