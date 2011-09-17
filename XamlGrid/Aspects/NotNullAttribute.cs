using System;

namespace XamlGrid.Aspects
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class NotNullAttribute : ArgumentValidationAttribute
	{
		public override void Validate(object value, string argumentName, Type parameterType)
		{
			if (value == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}
	}
}