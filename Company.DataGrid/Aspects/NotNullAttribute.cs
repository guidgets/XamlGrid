using System;

namespace Company.Widgets.Aspects
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class NotNullAttribute : ArgumentValidationAttribute
	{
		public override void Validate(object value, string argumentName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}
	}
}