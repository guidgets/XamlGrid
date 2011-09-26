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
// File:	ColumnResizingBehavior.cs
// Authors:	Dimitar Dobrev

using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using XamlGrid.Models;
using XamlGrid.Views;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Behavior{T}"/> for a <see cref="Thumb"/> which enables the thumb to resize a <see cref="Column"/> of a <see cref="DataGrid"/>.
	/// </summary>
	public class ColumnResizingBehavior : Behavior<Thumb>
	{
		/// <summary>
		/// Called after the behavior is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.DragDelta += this.AssociatedObject_DragDelta;
			this.AssociatedObject.MouseLeftButtonUp += this.AssociatedObject_MouseLeftButtonUp;
		}

		/// <summary>
		/// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetaching()
		{
			base.OnDetaching();

			this.AssociatedObject.DragDelta -= this.AssociatedObject_DragDelta;
			this.AssociatedObject.MouseLeftButtonUp -= this.AssociatedObject_MouseLeftButtonUp;
		}

		private void AssociatedObject_DragDelta(object sender, DragDeltaEventArgs e)
		{
			Column columnToResize = this.AssociatedObject.Tag as Column;
			if (columnToResize != null && columnToResize.IsResizable)
			{
				double newWidth = columnToResize.ActualWidth + e.HorizontalChange;
				columnToResize.Width = new ColumnWidth(newWidth > 1 ? newWidth : 1);
			}
		}

		private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.AssociatedObject.CancelDrag();
		}
	}
}
