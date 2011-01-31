using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public class SampleObject
	{
		public static ICollection<SampleObject> GetCollection()
		{
			return GetEnumerable().ToList();
		}

		public static IEnumerable<SampleObject> GetEnumerable()
		{
			return (from i in Enumerable.Range(0, 100)
					select new SampleObject());
		}
	}
}
