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
