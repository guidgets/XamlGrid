using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

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
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.KeyDown += AssociatedObject_KeyDown;
		}

		/// <summary>
		/// Called when the behaviour is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetaching()
		{
			base.OnDetaching();

			this.AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
		}

		private static void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
		{
			if ((Keyboard.Modifiers & KeyHelper.CommandModifier) != KeyHelper.CommandModifier &&
				(e.Key == Key.Right || e.Key == Key.Left))
			{
				e.Handled = true;
			}
			if (e.Key == Key.Home || e.Key == Key.End)
			{
				e.Handled = true;
			}
		}
	}
}
