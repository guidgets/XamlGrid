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
// File:	SelectionModelTests.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using XamlGrid.Models;

namespace UnitTests.Selection
{
	[TestFixture]
	public class SelectionModelTests
	{
		private SelectionModel selectionModel;

		[TestFixtureSetUp]
		public void InitializeSelectionModel()
		{
			this.selectionModel = new SelectionModel();
			this.selectionModel.Items = (from index in Enumerable.Range(0, 100)
			                             select new object()).ToArray();
			this.selectionModel.OnRegister();
		}

		[TestFixtureTearDown]
		public void DestroySelectionModel()
		{
			this.selectionModel.OnRemove();
			this.selectionModel.Items = null;
			this.selectionModel = null;
		}

		[TearDown]
		public void Clear()
		{
			this.selectionModel.SelectedItems.Clear();
		}

		[Test]
		public void TestRangeSelection()
		{
			Randomizer randomizer = new Randomizer();
			int[] indices = randomizer.GetInts(0, this.selectionModel.Items.Count, 2);
			Array.Sort(indices);
			this.selectionModel.SelectRange(indices[0], indices[1]);

			AssertRangeSelected(indices);
		}

		[Test]
		public void TestSelectAll()
		{
			this.selectionModel.SelectAll();
			foreach (object item in this.selectionModel.Items)
			{
				Assert.IsTrue(this.selectionModel.SelectedItems.IsSelected(item));
			}
		}

		[Test]
		public void TestSelectAllThenRange()
		{
			Randomizer randomizer = new Randomizer();
			int[] indices = randomizer.GetInts(0, this.selectionModel.Items.Count, 2);
			Array.Sort(indices);

			this.selectionModel.Select(this.selectionModel.Items[indices[0]]);
			this.selectionModel.SelectAll();
			this.selectionModel.SelectRange(indices[1], true);

			this.AssertRangeSelected(indices);
		}

		private void AssertRangeSelected(IList<int> indices)
		{
			string range = string.Format("Range: {0} - {1}", indices[0], indices[1]);
			for (int index = 0; index < indices[0] || index > indices[1]; index++)
			{
				Assert.IsFalse(this.selectionModel.SelectedItems.IsSelected(this.selectionModel.Items[index]), range);
			}
			for (int index = indices[0]; index <= indices[1]; index++)
			{
				Assert.IsTrue(this.selectionModel.SelectedItems.IsSelected(this.selectionModel.Items[index]), range);
			}
		}
	}
}
