using System;

namespace Company.Widgets.Aspects
{
	public class ValidStringEnumAttribute : ArgumentValidationAttribute
	{
		private readonly Type enumType;

		public ValidStringEnumAttribute(Type enumType)
		{
			this.enumType = enumType;
		}

		public override void Validate(object value, string argumentName, Type parameterType)
		{
			string source = (string) value;
			if (string.IsNullOrEmpty(source))
			{
				throw new ArgumentException(string.Format("{0} must not be empty.", argumentName), argumentName);
			}
			string[] enumValues = source.Split('|');
			foreach (string enumValue in source.Split('|'))
			{
				try
				{
					Enum.Parse(this.enumType, enumValue, true);
				}
				catch (ArgumentException)
				{
					if (enumValues.Length == 1)
					{
						throw;
					}
					const string message = "The enum value '{0}' from the bitwise concatenation '{1}' is an invalid value of the enum of {2}.";
					throw new ArgumentException(string.Format(message, enumValue, source, this.enumType.FullName), source);
				}
			}
		}
	}
}
