using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Company.Widgets.Controllers
{
	public class ScrollIntoViewBehavior : Behavior<ScrollViewer>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.GotFocus += this.AssociatedObject_GotFocus;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			this.AssociatedObject.GotFocus -= this.AssociatedObject_GotFocus;
		}

		private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
		{
			UIElement focusedElement = e.OriginalSource as UIElement;
			if (focusedElement == null)
			{
				return;
			}
			IScrollInfo scrollInfo = null;
			ItemsPresenter itemsPresenter = this.AssociatedObject.Content as ItemsPresenter;
			if (itemsPresenter != null)
			{
				scrollInfo = VisualTreeHelper.GetChild(itemsPresenter, 0) as IScrollInfo ??
				             this.AssociatedObject.Content as IScrollInfo;
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
		}

		private void MakeVisible(UIElement focusedElement)
		{
			GeneralTransform generalTransform = focusedElement.TransformToVisual(this.AssociatedObject);
			Point point = generalTransform.Transform(new Point(0, 0));

			double horizontalOffset = point.X + focusedElement.RenderSize.Width - this.AssociatedObject.ViewportWidth;
			if (horizontalOffset > 0)
			{
				ScrollExtensions.SetHorizontalOffset(this.AssociatedObject,
				                                     this.AssociatedObject.HorizontalOffset + horizontalOffset);
			}
			else
			{
				if (point.X < 0)
				{
					ScrollExtensions.SetHorizontalOffset(this.AssociatedObject, this.AssociatedObject.HorizontalOffset + point.X);
				}
			}

			double verticalOffset = point.Y + focusedElement.RenderSize.Height - this.AssociatedObject.ViewportHeight;
			if (verticalOffset > 0)
			{
				ScrollExtensions.SetVerticalOffset(this.AssociatedObject,
				                                   this.AssociatedObject.VerticalOffset + verticalOffset);
			}
			else
			{
				if (point.Y < 0)
				{
					ScrollExtensions.SetVerticalOffset(this.AssociatedObject, this.AssociatedObject.VerticalOffset + point.Y);
				}
			}
		}
	}
}
