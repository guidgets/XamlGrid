using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Company.Widgets.Core;

namespace Company.Widgets.Controllers
{
	public class ScrollIntoViewBehavior : Controller
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
			return new List<int> { Notifications.CELL_FOCUSED };
		}

		/// <summary>
		/// Handles the notification.
		/// </summary>
		/// <param name="notification">The notification.</param>
		public override void HandleNotification(INotification notification)
		{
			switch (notification.Code)
			{
				case Notifications.CELL_FOCUSED:
					UIElement focusedElement = (UIElement) notification.Body;
					IScrollInfo scrollInfo = null;
					ItemsPresenter itemsPresenter = this.Scroll.Content as ItemsPresenter;
					if (itemsPresenter != null)
					{
						scrollInfo = VisualTreeHelper.GetChild(itemsPresenter, 0) as IScrollInfo ??
						             this.Scroll.Content as IScrollInfo;
					}
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
