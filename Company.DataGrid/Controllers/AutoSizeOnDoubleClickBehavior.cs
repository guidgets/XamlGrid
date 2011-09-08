using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Represents a <see cref="Behavior{T}"/> for a <see cref="Thumb"/> which enables the thumb to auto-size a <see cref="Column"/> of a <see cref="Company.Widgets"/>.
	/// </summary>
	public class AutoSizeOnDoubleClickBehavior : Behavior<Thumb>
	{
		/// <summary>
		/// Called after the behavior is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent,
			                                 new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown), true);
		}

		/// <summary>
		/// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetaching()
		{
			this.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent,
			                                    new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonDown));
			base.OnDetaching();
		}

		private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				Column columnToResize = this.AssociatedObject.Tag as Column;
				if (columnToResize != null && columnToResize.IsResizable)
				{
					columnToResize.AutoSize();
				}
			}
		}
	}
}
