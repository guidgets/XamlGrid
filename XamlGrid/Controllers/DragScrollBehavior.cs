using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Threading;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Behavior{T}"/> that enables a <see cref="ScrollViewer"/> to scroll its contents while the pointing device performs dragging.
	/// </summary>
	public class DragScrollBehavior : Behavior<ScrollViewer>
	{
		private Boolean scrollDown;
		private Boolean scrollLeft;
		private Boolean scrollRight;
		private Boolean scrollUp;
		private DispatcherTimer myDispatcherTimer;
		private Panel panel;
		private PropertyInfo orientationProperty;


		public DragScrollBehavior()
		{
			this.ScrollArea = 40;
			this.ScrollPixelsPerTick = 10;
		}


		/// <summary>
		/// Defines the width (in pixels) of the zone at the edge of the <see cref="ScrollViewer"/> that will trigger automatic scrolling. Default is 40.
		/// </summary>
		public double ScrollArea
		{
			get; 
			set;
		}

		/// <summary>
		/// The number of pixels that will be scrolled per 100 milliseconds when scrolling is activated. Default is 5.
		/// </summary>
		public double ScrollPixelsPerTick
		{
			get; 
			set;
		}

		/// <summary>
		/// Causes the targeted <see cref="ScrollViewer"/> to scroll left.
		/// </summary>
		public Boolean ScrollLeft
		{
			get
			{
				return this.scrollLeft;
			}
			set
			{
				if (value)
				{
					if (this.ScrollUp == this.ScrollDown == this.ScrollRight == this.scrollLeft == false)
					{
						this.StartTimer();
					}
				}
				else
				{
					if (this.ScrollUp == this.ScrollDown == this.ScrollRight == false)
					{
						this.StopTimer();
					}
				}
				this.scrollLeft = value;
			}
		}

		/// <summary>
		/// Causes the targeted <see cref="ScrollViewer"/> to scroll right, at a rate defined by the <see cref="ScrollPixelsPerTick"/> property.
		/// </summary>
		public Boolean ScrollRight
		{
			get
			{
				return this.scrollRight;
			}
			set
			{
				if (value)
				{
					if (this.ScrollUp == this.ScrollDown == this.ScrollRight == this.scrollLeft == false)
					{
						this.StartTimer();
					}
				}
				else
				{
					if (this.ScrollUp == this.ScrollDown == this.ScrollLeft == false)
					{
						this.StopTimer();
					}
				}
				this.scrollRight = value;
			}
		}

		/// <summary>
		/// Causes the targeted <see cref="ScrollViewer"/> to scroll up, at a rate defined by the <see cref="ScrollPixelsPerTick"/> property.
		/// </summary>
		public Boolean ScrollUp
		{
			get
			{
				return this.scrollUp;
			}
			set
			{
				if (value)
				{
					if (this.ScrollUp == this.ScrollDown == this.ScrollRight == this.scrollLeft == false)
					{
						this.StartTimer();
					}
				}
				else
				{
					if (this.ScrollRight == this.ScrollDown == this.ScrollLeft == false)
					{
						this.StopTimer();
					}
				}
				this.scrollUp = value;
			}
		}

		/// <summary>
		/// Causes the targeted <see cref="ScrollViewer"/> to scroll down, at a rate defined by the <see cref="ScrollPixelsPerTick"/> property.
		/// </summary>
		public Boolean ScrollDown
		{
			get
			{
				return this.scrollDown;
			}
			set
			{
				if (value)
				{
					if (this.ScrollUp == this.ScrollDown == this.ScrollRight == this.scrollLeft == false)
					{
						this.StartTimer();
					}
				}
				else
				{
					if (this.ScrollUp == this.ScrollRight == this.ScrollLeft == false)
					{
						this.StopTimer();
					}
				}
				this.scrollDown = value;
			}
		}


		/// <summary>
		/// Called after the behaviour is attached to an AssociatedObject.
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent,
			                                 new MouseButtonEventHandler(this.MouseLeftButtonDownHandler), true);

			this.AssociatedObject.LayoutUpdated += this.AssociatedObject_LayoutUpdated;
		}

		private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
		{
			this.AssociatedObject.LayoutUpdated -= this.AssociatedObject_LayoutUpdated;
			this.panel = (Panel) VisualTreeHelper.GetChild((DependencyObject) this.AssociatedObject.Content, 0);
			Type typeOrientation = typeof(Orientation);
			this.orientationProperty = (from property in this.panel.GetType().GetProperties()
			                            where property.PropertyType == typeOrientation
			                            select property).FirstOrDefault();
		}

		private void StartTimer()
		{
			this.myDispatcherTimer = new DispatcherTimer();
			this.myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			this.myDispatcherTimer.Tick += this.Each_Tick;
			this.myDispatcherTimer.Start();
		}

		private void StopTimer()
		{
			if (this.myDispatcherTimer != null)
			{
				this.myDispatcherTimer.Stop();
				this.myDispatcherTimer.Tick -= this.Each_Tick;
			}
		}

		private void DownScroll()
		{
			double offset = this.GetOffset(Orientation.Vertical);
			this.AssociatedObject.ScrollToVerticalOffset(this.AssociatedObject.VerticalOffset + offset);
		}

		private void UpScroll()
		{
			double offset = this.GetOffset(Orientation.Vertical);
			this.AssociatedObject.ScrollToVerticalOffset(this.AssociatedObject.VerticalOffset - offset);
		}

		private void RightScroll()
		{
			double offset = this.GetOffset(Orientation.Horizontal);
			this.AssociatedObject.ScrollToHorizontalOffset(this.AssociatedObject.HorizontalOffset + offset);
		}

		private void LeftScroll()
		{
			double offset = this.GetOffset(Orientation.Horizontal);
			this.AssociatedObject.ScrollToHorizontalOffset(this.AssociatedObject.HorizontalOffset - offset);
		}

		private double GetOffset(Orientation orientation)
		{
			if (this.orientationProperty != null &&
				(Orientation) this.orientationProperty.GetValue(this.panel, null) == orientation)
			{
				return 1;
			}
			return this.ScrollPixelsPerTick;
		}


		private void MouseLeftButtonDownHandler(object sender, MouseButtonEventArgs e)
		{
			this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonUp), true);
			this.AssociatedObject.MouseMove += this.AssociatedObject_MouseMove;
			this.AssociatedObject.MouseLeave += this.AssociatedObject_MouseLeave;
		}

		private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.ScrollLeft = this.ScrollDown = this.ScrollUp = this.ScrollRight = false;
			this.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonUp));
			this.AssociatedObject.MouseMove -= this.AssociatedObject_MouseMove;
			this.AssociatedObject.MouseLeave -= this.AssociatedObject_MouseLeave;
			this.AssociatedObject.ReleaseMouseCapture();
		}

		private void Each_Tick(object o, EventArgs sender)
		{
			if (this.ScrollRight)
			{
				if (this.AssociatedObject.HorizontalOffset == this.AssociatedObject.ScrollableWidth)
				{
					this.ScrollRight = false;
				}
				else
				{
					this.RightScroll();
				}
			}
			if (this.ScrollLeft)
			{
				if (this.AssociatedObject.HorizontalOffset == 0)
				{
					this.ScrollLeft = false;
				}
				else
				{
					this.LeftScroll();
				}
			}
			if (this.ScrollDown)
			{
				if (this.AssociatedObject.VerticalOffset == this.AssociatedObject.ScrollableHeight)
				{
					this.ScrollDown = false;
				}
				else
				{
					this.DownScroll();
				}
			}
			if (this.ScrollUp)
			{
				if (this.AssociatedObject.VerticalOffset == 0)
				{
					this.ScrollUp = false;
				}
				else
				{
					this.UpScroll();
				}
			}
		}

		private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
		{
			Point mousePosition = e.GetPosition(this.AssociatedObject);

			if (mousePosition.X < this.ScrollArea && this.AssociatedObject.HorizontalOffset > 0)
			{
				this.ScrollLeft = true;
			}
			else
			{
				if (this.ScrollLeft)
				{
					this.ScrollLeft = false;
				}
			}

			if (mousePosition.Y < this.ScrollArea && this.AssociatedObject.VerticalOffset > 0)
			{
				this.ScrollUp = true;
			}
			else
			{
				if (this.ScrollUp)
				{
					this.ScrollUp = false;
				}
			}

			if (mousePosition.X > (this.AssociatedObject.ActualWidth - this.ScrollArea))
			{
				this.ScrollRight = true;
			}
			else
			{
				if (this.ScrollRight)
				{
					this.ScrollRight = false;
				}
			}

			if (mousePosition.Y > (this.AssociatedObject.ActualHeight - this.ScrollArea))
			{
				this.ScrollDown = true;
			}
			else
			{
				if (this.ScrollDown)
				{
					this.ScrollDown = false;
				}
			}
		}

		private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
		{
			this.ScrollUp = this.ScrollDown = this.ScrollLeft = this.ScrollRight = false;
			this.AssociatedObject.CaptureMouse();
		}
	}
}