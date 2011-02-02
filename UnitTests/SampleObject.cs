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
