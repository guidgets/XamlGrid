//
// PropertyPathNode.cs
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

using System;
using System.ComponentModel;

namespace Company.Widgets.Models
{
	public class StandardPropertyPathNode : PropertyPathNode
	{
		public StandardPropertyPathNode(string typeName, string propertyName)
		{
			this.TypeName = typeName;
			this.PropertyName = propertyName;
		}

		public string PropertyName
		{
			get;
			private set;
		}

		public string TypeName
		{
			get;
			private set;
		}

		protected override void OnSourceChanged(object oldSource, object newSource)
		{
			base.OnSourceChanged(oldSource, newSource);

			this.PropertyInfo = null;
			if (this.Source == null)
			{
				return;
			}

			Type type = this.Source.GetType();
			if (this.TypeName != null)
			{
				type = Type.GetType(this.TypeName);
			}

			if (type == null)
			{
				return;
			}

			this.PropertyInfo = type.GetProperty(this.PropertyName);
		}

		protected override void OnSourcePropertyChanged(object o, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.PropertyName && this.PropertyInfo != null)
			{
				this.UpdateValue();
				if (this.Next != null)
				{
					this.Next.SetSource(this.Value);
				}
			}
		}

		public override void SetValue(object value)
		{
			if (this.PropertyInfo != null)
			{
				this.PropertyInfo.SetValue(this.Source, value, null);
			}
		}

		public override void UpdateValue()
		{
			if (this.PropertyInfo != null)
			{
				this.ValueType = this.PropertyInfo.PropertyType;
				this.Value = this.PropertyInfo.GetValue(this.Source, null);
				if (this.Value == null && this.EnsureNonNulls && this.PropertyInfo.PropertyType != typeof(string))
				{
					this.Value = Activator.CreateInstance(this.PropertyInfo.PropertyType);
					this.PropertyInfo.SetValue(this.Source, this.Value, null);
				}
			}
			else
			{
				this.ValueType = null;
				this.Value = null;
			}
		}
	}
}
