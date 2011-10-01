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
// File:	ScrollIntoViewController.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using XamlGrid.Core;

namespace XamlGrid.Controllers
{
	public class ScrollIntoViewController : Controller
	{
		/// <summary>
		/// Gets the row for which functionality the <see cref="RowController"/> is responsible.
		/// </summary>
		public virtual ScrollViewer Scroll
		{
			get
			{
				return (ScrollViewer) this.ViewComponent;
			}
		}


		/// <summary>
		/// Lists the notification interests.
		/// </summary>
		/// <returns></returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int> { Notifications.CellFocused, Notifications.ScrollIntoView };
		}

		/// <summary>
		/// Handles the notification.
		/// </summary>
		/// <param name="notification">The notification.</param>
		public override void HandleNotification(INotification notification)
		{
			base.HandleNotification(notification);
			switch (notification.Code)
			{
				case Notifications.CellFocused:
					UIElement focusedElement = (UIElement) notification.Body;
					IScrollInfo scrollInfo = this.Scroll.GetScrollInfo();
					if (scrollInfo == null)
					{
						this.MakeVisible(focusedElement);
					}
					else
					{
						Rect bounds = new Rect(0, 0, focusedElement.RenderSize.Width, focusedElement.RenderSize.Height);
						scrollInfo.MakeVisible(focusedElement, bounds);
					}
					break;
                case Notifications.ScrollIntoView:
					double index = (double) notification.Body;
					if (index < this.Scroll.VerticalOffset || this.Scroll.VerticalOffset + this.Scroll.ViewportHeight < index)
					{
						this.Scroll.ScrollToVerticalOffset(index);						
					}
			        break;
			}
		}

		private void MakeVisible(UIElement focusedElement)
		{
			GeneralTransform generalTransform = focusedElement.TransformToVisual(this.Scroll);
			Point point = generalTransform.Transform(new Point(0, 0));

			double horizontalOffset = point.X + focusedElement.RenderSize.Width - this.Scroll.ViewportWidth;
			if (horizontalOffset > 0)
			{
				ScrollExtensions.SetHorizontalOffset(this.Scroll,
				                                     this.Scroll.HorizontalOffset + horizontalOffset);
			}
			else
			{
				if (point.X < 0)
				{
					ScrollExtensions.SetHorizontalOffset(this.Scroll, this.Scroll.HorizontalOffset + point.X);
				}
			}

			double verticalOffset = point.Y + focusedElement.RenderSize.Height - this.Scroll.ViewportHeight;
			if (verticalOffset > 0)
			{
				ScrollExtensions.SetVerticalOffset(this.Scroll,
				                                   this.Scroll.VerticalOffset + verticalOffset);
			}
			else
			{
				if (point.Y < 0)
				{
					ScrollExtensions.SetVerticalOffset(this.Scroll, this.Scroll.VerticalOffset + point.Y);
				}
			}
		}
	}
}
