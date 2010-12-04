//
// IndexedPropertyPathNode.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using Company.Widgets.Controllers;

namespace Company.Widgets.Models
{
	public class IndexedPropertyPathNode : PropertyPathNode
	{
		private static readonly PropertyInfo IListIndexer = GetIndexer(true, typeof(IList));

		private bool isBroken;

		public IndexedPropertyPathNode(string index)
		{
			int val;
			this.Index = int.TryParse(index, out val) ? (object) val : index;
		}

		public override bool IsBroken
		{
			get
			{
				return this.isBroken || base.IsBroken;
			}
		}

		public object Index
		{
			get;
			private set;
		}

		private void GetIndexer()
		{
			this.PropertyInfo = null;
			if (this.Source != null)
			{
				this.PropertyInfo = GetIndexer(this.Index is int, this.Source.GetType());
				if (this.PropertyInfo == null && this.Source is IList)
				{
					this.PropertyInfo = IListIndexer;
				}
			}
		}

		private static PropertyInfo GetIndexer(bool allowIntIndexer, Type type)
		{
			PropertyInfo propInfo = null;
			MemberInfo[] members = type.GetDefaultMembers();
			foreach (PropertyInfo member in members)
			{
				ParameterInfo[] param = member.GetIndexParameters();
				if (param.Length == 1)
				{
					if (allowIntIndexer && param[0].ParameterType == typeof(int))
					{
						propInfo = member;
						break;
					}
					if (param[0].ParameterType == typeof(string))
					{
						propInfo = member;
					}
				}
			}

			return propInfo;
		}

		private void OnCollectionChanged(object o, NotifyCollectionChangedEventArgs e)
		{
			this.UpdateValue();
			if (this.Next != null)
				this.Next.SetSource(this.Value);
		}

		private void OnPropertyChanged(object o, PropertyChangedEventArgs e)
		{
			this.UpdateValue();
			if (this.Next != null)
				this.Next.SetSource(this.Value);
		}

		protected override void OnSourceChanged(object oldSource, object newSource)
		{
			base.OnSourceChanged(oldSource, newSource);

			if (oldSource is INotifyCollectionChanged)
				((INotifyCollectionChanged) oldSource).CollectionChanged -= this.OnCollectionChanged;
			if (newSource is INotifyCollectionChanged)
				((INotifyCollectionChanged) newSource).CollectionChanged += this.OnCollectionChanged;

			if (oldSource is INotifyPropertyChanged)
				((INotifyPropertyChanged) oldSource).PropertyChanged -= this.OnPropertyChanged;
			if (newSource is INotifyPropertyChanged)
				((INotifyPropertyChanged) newSource).PropertyChanged += this.OnPropertyChanged;

			this.GetIndexer();
		}

		public override void SetValue(object value)
		{
			if (this.PropertyInfo != null)
				this.PropertyInfo.SetValue(this.Source, value, new[] { this.Index });
		}

		public override void UpdateValue()
		{
			this.isBroken = true;
			if (this.PropertyInfo == null)
			{
				this.ValueType = null;
				this.Value = null;
				return;
			}

			try
			{
				if (this.EnsureNonNulls && this.Index is int)
				{
					int index = (int) this.Index;
					int count = int.MaxValue;
					Type sourceType = this.PropertyInfo.DeclaringType;
					if (sourceType.GetInterface(typeof(ICollection<>).FullName, false) != null)
					{
						count = (int) sourceType.GetProperty("Count").GetValue(this.Source, null);
					}
					else
					{
						if (this.Source is IList)
						{
							count = ((IList) this.Source).Count;
						}
					}
					if (count < index + 1)
					{
						MethodInfo methodAdd = sourceType.GetMethod("Add");
						Type elementType = ((IEnumerable) this.Source).GetElementType();
						for (int i = count; i < index + 1; i++)
						{
							methodAdd.Invoke(this.Source, new[] { Activator.CreateInstance(elementType) });
						}
					}
				}
				object newVal = this.PropertyInfo.GetValue(this.Source, new[] { this.Index });
				this.isBroken = false;
				if (this.Value != newVal)
				{
					this.ValueType = newVal == null ? null : newVal.GetType();
					this.Value = newVal;
					if (newVal == null && this.EnsureNonNulls && this.PropertyInfo.PropertyType != typeof(string))
					{
						this.Value = Activator.CreateInstance(this.PropertyInfo.PropertyType);
						this.PropertyInfo.SetValue(this.Source, this.Value, new[] { Index });
					}
				}
			}
			catch
			{
				this.ValueType = null;
				this.Value = null;
			}
		}
	}
}
