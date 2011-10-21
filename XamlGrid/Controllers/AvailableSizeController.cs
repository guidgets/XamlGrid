using XamlGrid.Core;
using XamlGrid.Views;

namespace XamlGrid.Controllers
{
	public class AvailableSizeController : Controller
	{
		public MeasuringContentPresenter MeasuringContentPresenter
		{
			get { return (MeasuringContentPresenter) this.ViewComponent; }
		}


		/// <summary>
		/// Called by the <see cref="Controller"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.MeasuringContentPresenter.AvailableSizeChanged += this.MeasuringContentPresenter_AvailableSizeChanged;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.MeasuringContentPresenter.AvailableSizeChanged -= this.MeasuringContentPresenter_AvailableSizeChanged;
		}


		private void MeasuringContentPresenter_AvailableSizeChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.AvailableSizeChanged, this.MeasuringContentPresenter.AvailableSize);
		}
	}
}