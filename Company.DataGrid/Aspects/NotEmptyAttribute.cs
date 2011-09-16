using System;

namespace Company.Widgets.Aspects
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class NotEmptyAttribute : ArgumentValidationAttribute
	{
		private bool checkTrimmed = true;

		public bool CheckTrimmed
		{
			get { return this.checkTrimmed; }
			set { this.checkTrimmed = value; }
		}

		public override void Validate(object value, string argumentName, Type parameterType)
		{
			string source = (string) value;
			if (string.IsNullOrEmpty(source) || (this.checkTrimmed && string.IsNullOrEmpty(source.Trim())))
			{
				throw new ArgumentException(string.Format("{0} must not be empty.", argumentName), argumentName);
			}
		}
	}
}
