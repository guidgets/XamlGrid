using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using Company.DataGrid.Views;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Behavior{T}"/> for a <see cref="Thumb"/> which enables the thumb to resize a <see cref="Column"/> of a <see cref="DataGrid"/>.
	/// </summary>
	public class ColumnResizingBehavior : Behavior<Thumb>
	{
		/// <summary>
		/// Called after the behaviour is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.Cursor = Cursors.SizeWE;

			this.AssociatedObject.DragDelta += this.AssociatedObject_DragDelta;
			this.AssociatedObject.MouseLeftButtonUp += this.AssociatedObject_MouseLeftButtonUp;
		}

		/// <summary>
		/// Called when the behaviour is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetaching()
		{
			base.OnDetaching();

			this.AssociatedObject.Cursor = Cursors.Arrow;

			this.AssociatedObject.DragDelta -= this.AssociatedObject_DragDelta;
			this.AssociatedObject.MouseLeftButtonUp -= this.AssociatedObject_MouseLeftButtonUp;
		}

		private void AssociatedObject_DragDelta(object sender, DragDeltaEventArgs e)
		{
			if (this.AssociatedObject.Tag is Column)
			{
				Column columnToResize = (Column) this.AssociatedObject.Tag;
				double newWidth = columnToResize.ActualWidth + e.HorizontalChange;
				columnToResize.ActualWidth = newWidth > 1 ? newWidth : 1;
			}
		}

		private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this.AssociatedObject.IsDragging)
			{
				this.AssociatedObject.CancelDrag();
			}
		}
	}
}
