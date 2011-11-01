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
// File:	AvailableSizeController.cs
// Authors:	Dimitar Dobrev

using Guidgets.XamlGrid.Core;
using Guidgets.XamlGrid.Views;

namespace Guidgets.XamlGrid.Controllers
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