using System;
using System.Reflection;

namespace Company.Widgets.Models
{
	public interface IPropertyPathNode
	{
		event EventHandler ValueChanged;

		bool IsBroken { get; }

		IPropertyPathNode Next { get; set; }

		void SetValue(object value);

		object Source { get; }

		object Value { get; }

		PropertyInfo PropertyInfo { get; }

		Type ValueType { get; }

		bool EnsureNonNulls { get; set; }

		void SetSource(object source);
	}
}
