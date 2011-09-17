using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Causes the associated <see cref="ScrollViewer"/> to scroll appropriately when the Home or End key is pressed.
	/// </summary>
	public class ScrollOnHomeEndBehaviour : Behavior<ScrollViewer>
	{
		/// <summary>
		/// Called after the behaviour is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.AssociatedObject_KeyDown), true);
		}

		/// <summary>
		/// Called when the behaviour is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetaching()
		{
			base.OnDetaching();

			this.AssociatedObject.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.AssociatedObject_KeyDown));
		}

		private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Home:
					ScrollExtensions.SetVerticalOffset(this.AssociatedObject, double.MinValue);
					break;
				case Key.End:
					ScrollExtensions.SetVerticalOffset(this.AssociatedObject, double.MaxValue);
					break;
			}
		}
	}
}
