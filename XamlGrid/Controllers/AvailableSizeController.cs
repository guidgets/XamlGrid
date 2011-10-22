using XamlGrid.Core;
using XamlGrid.Views;

namespace XamlGrid.Controllers
{
	public class AvailableSizeController : Controller<MeasuringContentPresenter>
	{
		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.View.AvailableSizeChanged += this.MeasuringContentPresenter_AvailableSizeChanged;
		}

		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.View.AvailableSizeChanged -= this.MeasuringContentPresenter_AvailableSizeChanged;
		}


		private void MeasuringContentPresenter_AvailableSizeChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.AvailableSizeChanged, this.View.AvailableSize);
		}
	}
}