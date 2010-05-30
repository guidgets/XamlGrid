using Company.Widgets.Models;
using NUnit.Framework;

namespace UnitTests.Selection
{
	[TestFixture]
	public class RangeTests
	{
		[Test]
		public void TestRemovingRanges()
		{
			RangeCollection ranges = new RangeCollection();

			ranges.AddRange(0, 4);
			ranges.RemoveRange(1, 2);
			Assert.AreEqual(2, ranges.Count);
			Assert.AreEqual(new Range(0, 0), ranges[0]);
			Assert.AreEqual(new Range(3, 4), ranges[1]);

			ranges.Clear();

			ranges.AddRange(0, 4);
			ranges.RemoveRange(0, 1);
			Assert.AreEqual(1, ranges.Count);
			Assert.AreEqual(new Range(2, 4), ranges[0]);

			ranges.Clear();

			ranges.AddRange(0, 4);
			ranges.RemoveRange(4, 4);
			Assert.AreEqual(1, ranges.Count);
			Assert.AreEqual(new Range(0, 3), ranges[0]);
		}
	}
}
