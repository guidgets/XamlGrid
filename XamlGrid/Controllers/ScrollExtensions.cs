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
// File:	ScrollExtensions.cs
// Authors:	Dimitar Dobrev

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using XamlGrid.Aspects;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Contains methods that complement missing features of the <see cref="ScrollViewer"/>.
	/// </summary>
	public static class ScrollExtensions
	{
		public static readonly DependencyProperty VerticalOffsetProperty =
			DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollExtensions),
			                                    new PropertyMetadata(0d, OnVerticalOffsetChanged));

		public static readonly DependencyProperty HorizontalOffsetProperty =
			DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollExtensions),
			                                    new PropertyMetadata(0d, OnHorizontalOffsetChanged));


		[Validate]
		public static double GetVerticalOffset([NotNull] ScrollViewer scrollViewer)
		{
			return (double) scrollViewer.GetValue(VerticalOffsetProperty);
		}

		[Validate]
		public static void SetVerticalOffset([NotNull] ScrollViewer scrollViewer, double value)
		{
			scrollViewer.SetValue(VerticalOffsetProperty, value);
		}

		[Validate]
		public static double GetHorizontalOffset([NotNull] ScrollViewer scrollViewer)
		{
			return (double) scrollViewer.GetValue(HorizontalOffsetProperty);
		}

		[Validate]
		public static void SetHorizontalOffset([NotNull] ScrollViewer scrollViewer, double value)
		{
			scrollViewer.SetValue(HorizontalOffsetProperty, value);
		}

		private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ScrollViewer) d).ScrollToVerticalOffset((double) e.NewValue);
		}

		private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ScrollViewer) d).ScrollToHorizontalOffset((double) e.NewValue);
		}

		[Validate]
		public static IScrollInfo GetScrollInfo([NotNull] this ScrollViewer scroll)
		{
			ItemsPresenter itemsPresenter = scroll.Content as ItemsPresenter;
			if (itemsPresenter != null)
			{
				return VisualTreeHelper.GetChild(itemsPresenter, 0) as IScrollInfo;
			}
			return scroll.Content as IScrollInfo;
		}
	}
}
