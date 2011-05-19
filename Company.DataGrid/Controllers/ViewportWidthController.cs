using System.Windows;
using Company.Widgets.Core;

namespace Company.Widgets.Controllers
{
	public class ViewportWidthController : Controller
	{
		public FrameworkElement Element
		{
			get
			{
				return (FrameworkElement) this.ViewComponent;
			}
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.Element.SizeChanged += this.Element_SizeChanged;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.Element.SizeChanged -= this.Element_SizeChanged;
		}

		private void Element_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.SendNotification(Notifications.ViewportWidthChanged, e.NewSize);
		}
	}
}
