using System.Collections.Generic;
using System.Windows.Data;
using Company.Widgets.Models;
using NUnit.Framework;

namespace UnitTests
{
	public class SortingModelTests
	{
		private SortingModel sortingModel;
		private IList<SampleObject> sampleObjects;


		[TestFixtureSetUp]
		public void CreateSortingModel()
		{
			//this.sampleObjects = SampleObject.GetCollection();
			//this.sortingModel = new SortingModel();
			//this.sortingModel.SetCollectionView(new PagedCollectionView(sampleObjects));
		}

		[TestFixtureTearDown]
		public void DestroySortingModel()
		{
			this.sampleObjects.Clear();
			this.sampleObjects = null;
			this.sortingModel.SetCollectionView(null);
			this.sortingModel = null;
		}

		[Test]
		public void TestAscending()
		{
			
		}
	}
}
