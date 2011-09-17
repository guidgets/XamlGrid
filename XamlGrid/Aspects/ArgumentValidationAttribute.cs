using System;

namespace XamlGrid.Aspects
{
	public abstract class ArgumentValidationAttribute : Attribute
	{
		public abstract void Validate(object value, string argumentName, Type parameterType);
	}
}