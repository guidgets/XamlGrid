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

		public override void Validate(object value, string argumentName)
		{
			string @string = (string) value;
			if (string.IsNullOrEmpty(@string) || (this.checkTrimmed && string.IsNullOrEmpty(@string.Trim())))
			{
				throw new ArgumentException(argumentName, string.Format("{0} must not be empty.", argumentName));
			}
		}
	}
}
