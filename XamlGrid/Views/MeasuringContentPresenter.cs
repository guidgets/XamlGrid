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
// File:	MeasuringContentPresenter.cs
// Authors:	Dimitar Dobrev

using System.Windows;
using System.Windows.Controls;

namespace Guidgets.XamlGrid.Views
{
	public class MeasuringContentPresenter : ContentPresenter
	{
		public event DependencyPropertyChangedEventHandler AvailableSizeChanged;


		public static readonly DependencyProperty AvailableSizeProperty =
			DependencyProperty.Register("AvailableSize", typeof(Size), typeof(MeasuringContentPresenter),
			                            new PropertyMetadata(Size.Empty, OnAvailableSizeChanged));


		public Size AvailableSize
		{
			get { return (Size) GetValue(AvailableSizeProperty); }
			set { SetValue(AvailableSizeProperty, value); }
		}


		/// <summary>
		/// Provides the behavior for the Measure pass of Silverlight layout. Classes can override this method to define their own Measure pass behavior.
		/// </summary>
		/// <param name="availableSize">The available size that this object can give to child objects. Infinity (<see cref="F:System.Double.PositiveInfinity"/>) can be specified as a value to indicate that the object will size to whatever content is available.</param>
		/// <returns>
		/// The size that this object determines it needs during layout, based on its calculations of the allocated sizes for child objects; or based on other considerations, such as a fixed container size.
		/// </returns>
		protected override Size MeasureOverride(Size availableSize)
		{
			AvailableSize = availableSize;
			return base.MeasureOverride(availableSize);
		}

		private static void OnAvailableSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MeasuringContentPresenter) d).OnAvailableSizeChanged(e);
		}

		protected virtual void OnAvailableSizeChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.AvailableSizeChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
