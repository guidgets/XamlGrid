//
// CollectionViewNode.cs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2010 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.ComponentModel;
using System.Windows.Data;

namespace XamlGrid.Models
{
	public class CollectionViewNode : PropertyPathNode {

		public override bool IsBroken {
			get { return Source == null; }
		}

		bool BindsDirectlyToSource {
			get; set;
		}

		public bool BindToView {
			get; set;
		}

		public CollectionViewNode (bool bindsDirectlyToSource, bool bindToView)
		{
			BindsDirectlyToSource = bindsDirectlyToSource;
			BindToView = bindToView;
		}

		public override void SetValue (object value)
		{
			throw new System.NotImplementedException ();
		}

		public override void UpdateValue ()
		{
			// The simple case - we use the source object directly, whatever it is.
			if (BindsDirectlyToSource) {
				ValueType = Source == null ? null : Source.GetType ();
				Value = Source;
			} else {
				var usableSource = Source;
				ICollectionView view = null;
				if (Source is CollectionViewSource) {
					usableSource = null;
					view = ((CollectionViewSource) Source).View;
				} else if (Source is ICollectionView) {
					view = (ICollectionView) Source;
				}

				// If we have no ICollectionView, then use the Source directly.
				if (view == null) {
					ValueType = usableSource == null ? null : usableSource.GetType ();
					Value = usableSource;
				} else {
					// If we have an ICollectionView and the property we're binding to exists
					// on ICollectionView, we bind to the view. Otherwise we bind to its CurrentItem.
					if (BindToView) {
						ValueType = view.GetType ();
						Value = view;
					} else {
						ValueType = view.CurrentItem == null ? null : view.CurrentItem.GetType ();
						Value = view.CurrentItem;
					}
				}
			}
		}
	}
}

