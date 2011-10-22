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
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using XamlGrid.Core;

namespace XamlGrid.Controllers
{
	public class ScrollIntoViewController : Controller<ScrollViewer>
	{
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
					IScrollInfo scrollInfo = this.View.GetScrollInfo();
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
					if (index < this.View.VerticalOffset || this.View.VerticalOffset + this.View.ViewportHeight < index)
					{
						this.View.ScrollToVerticalOffset(index);						
					}
			        break;
			}
		}

		private void MakeVisible(UIElement focusedElement)
		{
			GeneralTransform generalTransform = focusedElement.TransformToVisual(this.View);
			Point point = generalTransform.Transform(new Point(0, 0));

			double horizontalOffset = point.X + focusedElement.RenderSize.Width - this.View.ViewportWidth;
			if (horizontalOffset > 0)
			{
				ScrollExtensions.SetHorizontalOffset(this.View,
				                                     this.View.HorizontalOffset + horizontalOffset);
			}
			else
			{
				if (point.X < 0)
				{
					ScrollExtensions.SetHorizontalOffset(this.View, this.View.HorizontalOffset + point.X);
				}
			}

			double verticalOffset = point.Y + focusedElement.RenderSize.Height - this.View.ViewportHeight;
			if (verticalOffset > 0)
			{
				ScrollExtensions.SetVerticalOffset(this.View,
				                                   this.View.VerticalOffset + verticalOffset);
			}
			else
			{
				if (point.Y < 0)
				{
					ScrollExtensions.SetVerticalOffset(this.View, this.View.VerticalOffset + point.Y);
				}
			}
		}
	}
}
