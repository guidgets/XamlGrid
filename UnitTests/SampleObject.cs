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
// File:	SampleObject.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public class SampleObject
	{
		public const string StringPropertyName = "StringProperty";
		public const string NumberPropertyName = "NumberProperty";
		public const string DatePropertyName = "DateProperty";

		public static IList<SampleObject> GetList()
		{
			return (IList<SampleObject>) GetCollection();
		}

		public static ICollection<SampleObject> GetCollection()
		{
			return GetEnumerable().ToList();
		}

		public static IEnumerable<SampleObject> GetEnumerable()
		{
			return (from i in Enumerable.Range(0, 100)
			        select new SampleObject { StringProperty = "String" + i, NumberProperty = i });
		}

		public string StringProperty
		{
			get; 
			set;
		}

		public int NumberProperty
		{
			get; 
			set;
		}

		public DateTime DateProperty
		{
			get; 
			set;
		}
	}
}
