using System;

namespace Company.Widgets.Aspects
{
	public abstract class ArgumentValidationAttribute : Attribute
	{
		public abstract void Validate(object value, string argumentName, Type parameterType);
	}
}