// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
// 
// File:	ValidStringEnumAttribute.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System;

namespace XamlGrid.Aspects
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
