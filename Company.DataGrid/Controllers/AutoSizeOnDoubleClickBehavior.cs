using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Represents a <see cref="Behavior{T}"/> for a <see cref="Thumb"/> which enables the thumb to auto-size a <see cref="Column"/> of a <see cref="Company.Widgets"/>.
	/// </summary>
	public class AutoSizeOnDoubleClickBehavior : Behavior<Thumb>
	{
		private const int TIME_BETWEEN_CLICKS = 500;

		private readonly DispatcherTimer timer;
		private bool waiting;
		private DateTime waitingSince;

		/// <summary>
		/// Represents a <see cref="Behavior{T}"/> for a <see cref="Thumb"/> which enables the thumb to auto-size a <see cref="Column"/> of a <see cref="Company.Widgets"/>.
		/// </summary>
		public AutoSizeOnDoubleClickBehavior()
		{
			this.timer = new DispatcherTimer();
			this.timer.Interval = new TimeSpan(0, 0, 0, 0, TIME_BETWEEN_CLICKS);
		}

		/// <summary>
		/// Called after the behavior is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttach()
		{
			base.OnAttach();
			this.timer.Tick += this.Timer_Tick;
			this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
		}

		/// <summary>
		/// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetach()
		{
			this.timer.Tick -= this.Timer_Tick;
			this.AssociatedObject.MouseLeftButtonUp -= this.AssociatedObject_MouseLeftButtonUp;
			base.OnDetach();
		}

		private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this.waiting)
			{
				if ((DateTime.Now - this.waitingSince).TotalMilliseconds < TIME_BETWEEN_CLICKS)
				{
					this.waiting = false;
					Column columnToResize = this.AssociatedObject.Tag as Column;
					if (columnToResize != null && columnToResize.IsResizable)
					{
						columnToResize.AutoSize();
					}
				}
			}
			else
			{
				this.waiting = true;
				this.waitingSince = DateTime.Now;
				timer.Start();
			}
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			if (this.waiting)
			{
				timer.Stop();
			}
			this.waiting = false;
		}
	}
}
