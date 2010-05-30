using System;
using System.Windows.Input;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Contains common functionalities needed when working with the keys of a keyboard.
	/// </summary>
	public static class KeyHelper
	{
		/// <summary>
		/// Gets the modifier that is used to issue a command to an application or a component.
		/// </summary>
		public static readonly ModifierKeys CommandModifier;

		static KeyHelper()
		{
			CommandModifier = Environment.OSVersion.Platform == PlatformID.MacOSX ? ModifierKeys.Apple : ModifierKeys.Control;
		}
	}
}
