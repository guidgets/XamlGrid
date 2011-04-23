using System.Windows;
using System.Windows.Input;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Marks events caused by the some navigation keys as handled.
	/// </summary>
	public class HandleNavigationBehaviour : Behavior<UIElement>
	{
		/// <summary>
		/// Called after the behaviour is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttach()
		{
			base.OnAttach();

			this.AssociatedObject.KeyDown += AssociatedObject_KeyDown;
		}

		/// <summary>
		/// Called when the behaviour is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetach()
		{
			base.OnDetach();

			this.AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
		}

		private static void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Left:
				case Key.Right:
					if ((Keyboard.Modifiers & KeyHelper.CommandModifier) != KeyHelper.CommandModifier)
					{
						e.Handled = true;
					}
					break;
				case Key.Up:
				case Key.Down:
				case Key.Home:
				case Key.End:
					e.Handled = true;
					break;
			}
		}
	}
}
