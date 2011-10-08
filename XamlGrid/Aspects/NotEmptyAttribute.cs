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
// File:	NotEmptyAttribute.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

using System;

namespace XamlGrid.Aspects
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
