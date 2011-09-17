using System;
using System.Collections.Generic;
using NUnit.Framework;
using XamlGrid.Models;

namespace UnitTests
{
	[TestFixture]
	public class NewItemTests
	{
		private class NoEmptyConstructor
		{
			public NoEmptyConstructor(int test)
			{
			}
		}


		private NewItemModel newItemModel;
		private ICollection<SampleObject> sampleObjects;


		[SetUp]
		public void InitializeNewItemModel()
		{
			this.sampleObjects = SampleObject.GetCollection();
			this.newItemModel = new NewItemModel();
			this.newItemModel.SetSource(DataWrapper.Wrap(this.sampleObjects));
		}

		[TearDown]
		public void DestroyNewItemModel()
		{
			this.sampleObjects.Clear();
			this.sampleObjects = null;
			this.newItemModel.SetSource(null);
			this.newItemModel = null;
		}

		[Test]
		public void TestItemType()
		{
			Assert.AreEqual(typeof(SampleObject), newItemModel.ItemType);
		}

		[Test]
		public void TestAddItem()
		{
			int count = this.sampleObjects.Count;

			this.newItemModel.AddItem();

			Assert.AreEqual(typeof(SampleObject), this.newItemModel.Data.GetType());
			Assert.AreEqual(sampleObjects.Count, count);
		}

		[Test]
		public void TestCommitItem()
		{
			int count = this.sampleObjects.Count;

			this.newItemModel.AddItem();
			int hashCode = this.newItemModel.Data.GetHashCode();
			this.newItemModel.CommitItem();

			Assert.AreEqual(sampleObjects.Count, count + 1);
			Assert.AreNotEqual(this.newItemModel.Data.GetHashCode(), hashCode);
		}

		[Test]
		public void TestExceptionOnCommitItem()
		{
			InvalidOperationException exception = Assert.Catch<InvalidOperationException>(() => this.newItemModel.CommitItem());
			Assert.AreEqual(exception.Message, "A new item must be added before being committed.");
		}

		[Test]
		public void TestExceptionOnImmutableCollection()
		{
			this.newItemModel.SetSource(DataWrapper.Wrap(SampleObject.GetEnumerable()));
			this.newItemModel.AddItem();
			const string message = "The addition of a new item was requested " +
			                       "but the source collection does not support adding elements.";
			NotSupportedException exception = Assert.Catch<NotSupportedException>(() => this.newItemModel.CommitItem());
			Assert.AreEqual(exception.Message, message);
		}

		[Test]
		public void TestExceptionOnNoEmptyObjectConstructor()
		{
			this.newItemModel.SetSource(DataWrapper.Wrap((new[] { new NoEmptyConstructor(0) })));
			const string error = "A new item cannot be created because the type of {0} " +
								 "does not have a parameterless constructor.";
			string message = string.Format(error, typeof(NoEmptyConstructor).FullName);
			MissingMemberException exception = Assert.Catch<MissingMemberException>(() => this.newItemModel.AddItem());
			Assert.AreEqual(exception.Message, message);
		}
	}
}
