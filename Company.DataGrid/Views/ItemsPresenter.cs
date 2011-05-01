//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright (c) 2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Markup;

namespace Company.Widgets.Views
{
	public sealed class ItemsPresenter : FrameworkElement
	{
		static ItemsPanelTemplate StackPanelFallbackTemplate = CreateStackPanelFallbackTemplate ();
		static ItemsPanelTemplate VirtualizingStackPanelFallbackTemplate = CreateVirtualizingStackPanelFallbackTemplate ();

		internal Panel _elementRoot;

		public ItemsPresenter ()
		{
		}

		internal override void InvokeOnApplyTemplate ()
		{
			ItemsControl c = (ItemsControl) TemplateOwner;
			c.SetItemsPresenter (this);
			base.InvokeOnApplyTemplate ();
		}

		internal override UIElement GetDefaultTemplate ()
		{
			// ItemsPresenter only works when it's attached to an ItemsControl
			// but the user may try to attach it to any custom control
			ItemsControl c = TemplateOwner as ItemsControl;
			if (c == null)
				return null;

			if (_elementRoot != null)
				return _elementRoot;

			if (c.ItemsPanel != null) {
				DependencyObject root = c.ItemsPanel.GetVisualTree (this);
				if (root != null && !(root is Panel))
					throw new InvalidOperationException ("The root element of an ItemsPanelTemplate must be a Panel subclass");
				_elementRoot = (Panel) root;
			}

#if false
			if (_elementRoot == null) {
				if (c is ListBox)
					_elementRoot = new VirtualizingStackPanel ();
				else
					_elementRoot = new StackPanel ();
			}
#else
			if (_elementRoot == null) {
				_elementRoot = (Panel) (c is ListBox ? VirtualizingStackPanelFallbackTemplate : StackPanelFallbackTemplate).GetVisualTree (this);
			}
#endif

			_elementRoot.IsItemsHost = true;
			return _elementRoot;
		}

		static ItemsPanelTemplate CreateStackPanelFallbackTemplate ()
		{
			return (ItemsPanelTemplate) XamlReader.Load (@"
<ItemsPanelTemplate xmlns=""http://schemas.microsoft.com/client/2007"">
	<StackPanel />
</ItemsPanelTemplate>");
		}

		static ItemsPanelTemplate CreateVirtualizingStackPanelFallbackTemplate ()
		{
			return (ItemsPanelTemplate) XamlReader.Load (@"
<ItemsPanelTemplate xmlns=""http://schemas.microsoft.com/client/2007"">
	<VirtualizingStackPanel />
</ItemsPanelTemplate>");
		}
	}
}
