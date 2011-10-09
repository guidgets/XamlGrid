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
// File:	RangeTests.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using NUnit.Framework;
using XamlGrid.Models;

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
