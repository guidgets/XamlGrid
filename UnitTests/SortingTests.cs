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
// File:	SortingTests.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using XamlGrid.Models;

namespace UnitTests
{
	[TestFixture]
	public class SortingTests
	{
		private SortingModel sortingModel;
		private IList<SampleObject> sampleObjects;


		[TestFixtureSetUp]
		public void Initialize()
		{
			this.sortingModel = new SortingModel();
			this.sampleObjects = SampleObject.GetList();
			this.sortingModel.SetCollectionView(DataWrapper.Wrap(this.sampleObjects));
		}

		[TestFixtureTearDown]
		public void Destroy()
		{
			this.sortingModel.SetCollectionView(null);
			this.sortingModel = null;
			this.sampleObjects.Clear();
			this.sampleObjects = null;
		}

		[TearDown]
		public void Clear()
		{
			this.sortingModel.SortDescriptions.Clear();
		}

		[Test]
		public void TestStacking()
		{
			this.sortingModel.Sort(new ExtendedSortDescription(SampleObject.StringPropertyName, ListSortDirection.Ascending));
			this.sortingModel.Sort(new ExtendedSortDescription(SampleObject.NumberPropertyName, ListSortDirection.Descending));

			Assert.AreEqual(this.sortingModel.SortDescriptions.Count, 2);
			Assert.AreEqual(this.sortingModel.SortDescriptions[1].PropertyName, SampleObject.NumberPropertyName);

			this.sortingModel.Sort(new ExtendedSortDescription(SampleObject.DatePropertyName, ListSortDirection.Ascending, true));

			Assert.AreEqual(this.sortingModel.SortDescriptions.Count, 1);
			Assert.AreEqual(this.sortingModel.SortDescriptions[0].PropertyName, SampleObject.DatePropertyName);
		}

		[Test]
		public void TestRepeatedSorting()
		{
			this.sortingModel.Sort(new ExtendedSortDescription(SampleObject.StringPropertyName, ListSortDirection.Ascending));
			this.sortingModel.Sort(new ExtendedSortDescription(SampleObject.StringPropertyName, ListSortDirection.Ascending));

			Assert.AreEqual(this.sortingModel.SortDescriptions.Count, 1);

			this.sortingModel.Sort(new ExtendedSortDescription(SampleObject.StringPropertyName, ListSortDirection.Descending));

			Assert.AreEqual(this.sortingModel.SortDescriptions.Count, 1);
			Assert.AreEqual(this.sortingModel.SortDescriptions[0].Direction, ListSortDirection.Descending);
		}

		[Test]
		public void TestPreliminarySortings()
		{
			this.sortingModel.SetCollectionView(null);
			this.sortingModel.Sort(new ExtendedSortDescription(SampleObject.DatePropertyName, ListSortDirection.Ascending));
			this.sortingModel.Sort(new ExtendedSortDescription(SampleObject.NumberPropertyName, ListSortDirection.Descending));
			ICollectionView collectionView = DataWrapper.Wrap(this.sampleObjects);
			this.sortingModel.SetCollectionView(collectionView);

			Assert.AreEqual(collectionView.SortDescriptions.Count, 2);
			Assert.AreEqual(collectionView.SortDescriptions[0].PropertyName, SampleObject.DatePropertyName);
			Assert.AreEqual(collectionView.SortDescriptions[0].Direction, ListSortDirection.Ascending);
			Assert.AreEqual(collectionView.SortDescriptions[1].PropertyName, SampleObject.NumberPropertyName);
			Assert.AreEqual(collectionView.SortDescriptions[1].Direction, ListSortDirection.Descending);
		}
	}
}
