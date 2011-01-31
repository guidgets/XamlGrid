using System;
using System.Collections.Generic;
using System.Windows.Data;
using Company.Widgets.Models;
using NUnit.Framework;

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
			this.newItemModel.SetSource(new PagedCollectionView(this.sampleObjects));
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
			const string message = "A new item must be added before being committed.";
			Assert.That(() => this.newItemModel.CommitItem(),
			            Throws.InvalidOperationException.With.Property("Message").EqualTo(message));
		}

		[Test]
		public void TestExceptionOnImmutableCollection()
		{
			this.newItemModel.SetSource(new PagedCollectionView(SampleObject.GetEnumerable()));
			this.newItemModel.AddItem();
			const string message = "The addition of a new item was requested " +
			                       "but the source collection does not support adding elements.";
			Assert.That(() => this.newItemModel.CommitItem(),
			            Throws.TypeOf(typeof(NotSupportedException)).With.Property("Message").EqualTo(message));
		}

		[Test]
		public void TestExceptionOnNoEmptyObjectConstructor()
		{
			this.newItemModel.SetSource(new PagedCollectionView(new[] { new NoEmptyConstructor(0) }));
			const string error = "A new item cannot be created because the type of {0} " +
								 "does not have a parameterless constructor.";
			string message = string.Format(error, typeof(NoEmptyConstructor).FullName);
			Assert.That(() => this.newItemModel.AddItem(),
			            Throws.TypeOf(typeof(MissingMemberException)).With.Property("Message").EqualTo(message));
		}
	}
}
