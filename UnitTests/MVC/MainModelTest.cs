/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Guidgets.XamlGrid.Core;


namespace UnitTests.MVC
{
    /**
	 * Test the PureMVC MainModel class.
	 */
	[TestFixture]
	public class MainModelTest
    {
         /**
  		 * Constructor.
  		 */

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

        /**
  		 * Tests the MainModel Singleton Factory Method 
  		 */
		[Test]
		[Description("MainModel Tests")]
		public void GetInstance()
        {
   			// Test Factory Method
   			IMainModel mainModel = MainModel.Instance;
   			
   			// test assertions
            Assert.IsNotNull(mainModel, "Expecting instance not null");
            Assert.IsTrue(mainModel != null, "Expecting instance implements IMainModel");
   		}

  		/**
  		 * Tests the Model registration and retrieval methods.
  		 * 
  		 * <P>
  		 * Tests <code>registerModel</code> and <code>retrieveModel</code> in the same test.
  		 * These methods cannot currently be tested separately
  		 * in any meaningful way other than to show that the
  		 * methods do not throw exception when called. </P>
  		 */
		[Test]
		[Description("MainModel Tests")]
		public void RegisterAndRetrieveModel()
        {
   			// register a Model and retrieve it.
   			IMainModel mainModel = MainModel.Instance;
			string name = "colors" + Thread.CurrentThread.Name;
			mainModel.RegisterModel(new Model(name, new List<string>(new[] { "red", "green", "blue" })));
			IModel Model = mainModel.RetrieveModel(name);
			List<string> data = (List<string>) Model.Data;
			
			// test assertions
            Assert.IsNotNull(data, "Expecting data not null");
			Assert.IsTrue(data != null, "Expecting data type is ArrayList");
   			Assert.IsTrue(data.Count == 3, "Expecting data.length == 3");
   			Assert.IsTrue(data[0] == "red", "Expecting data[0] == 'red'");
            Assert.IsTrue(data[1] == "green", "Expecting data[1] == 'green'");
            Assert.IsTrue(data[2] == "blue", "Expecting data[2] == 'blue'");
   		}
  		
  		/**
  		 * Tests the Model removal method.
  		 */
		[Test]
		[Description("MainModel Tests")]
		public void RegisterAndRemoveModel()
        {
   			// register a Model, remove it, then try to retrieve it
   			IMainModel mainModel = MainModel.Instance;
			string name = "sizes" + Thread.CurrentThread.Name;
			mainModel.RegisterModel(new Model(name, new List<int>(new[] { 7, 13, 21 })));

			IModel removedModel = mainModel.RemoveModel(name);

			Assert.IsTrue(removedModel.ModelName == name, "Expecting removedModel.ModelName == name");

			IModel Model = mainModel.RetrieveModel(name);
			
			// test assertions
   			Assert.IsNull(Model, "Expecting Model is null");
   		}
  		
  		/**
  		 * Tests the hasModel Method
  		 */
		[Test]
		[Description("MainModel Tests")]
		public void HasModel()
		{
  			
   			// register a Model
   			IMainModel mainModel = MainModel.Instance;
			string name = "aces" + Thread.CurrentThread.Name;
			IModel Model = new Model(name, new List<string>(new[] { "clubs", "spades", "hearts", "diamonds" }));
			mainModel.RegisterModel(Model);
			
   			// assert that the MainModel.hasModel method returns true
   			// for that Model name
			Assert.IsTrue(mainModel.HasModel(name), "Expecting MainModel.hasModel(name) == true");
			
			// remove the Model
			mainModel.RemoveModel(name);
			
   			// assert that the MainModel.hasModel method returns false
   			// for that Model name
			Assert.IsTrue(mainModel.HasModel(name) == false, "Expecting MainModel.hasModel(name) == false");
   		}
  		
		/**
		 * Tests that the MainModel calls the onRegister and onRemove methods
		 */
		[Test]
		[Description("MainModel Tests")]
		public void OnRegisterAndOnRemove()
		{
			
  			// Get the Singleton MainController instance
  			IMainModel mainModel = MainModel.Instance;

			// Create and register the test Controller
			IModel model = new TestModel();
			mainModel.RegisterModel(model);

			// assert that onRegsiter was called, and the Model responded by setting its data accordingly
			Assert.IsTrue(model.Data.ToString() == TestModel.ON_REGISTER_CALLED, "Expecting Model.Data.ToString() == MainModelTestModel.ON_REGISTER_CALLED");
			
			// Remove the component
			mainModel.RemoveModel(TestModel.NAME);
			
			// assert that onRemove was called, and the Model responded by setting its data accordingly
   			Assert.IsTrue(model.Data.ToString() == TestModel.ON_REMOVE_CALLED, "Expecting Model.Data.ToString() == MainModelTestModel.ON_REMOVE_CALLED");
		}

		/// <summary>
		/// Test all of the function above using many threads at once.
		/// </summary>
		[Test]
		[Description("MainModel Tests")]
		public void MultiThreadedOperations()
		{
			count = 20;
			IList<Thread> threads = new List<Thread>();

			for (int i = 0; i < count; i++)
			{
				Thread t = new Thread(this.MultiThreadedTestFunction);
				t.Name = "MainCommandTest" + i;
				threads.Add(t);
			}

			foreach (Thread t in threads)
			{
				t.Start();
			}

			while (true)
			{
				if (count <= 0) break;
				Thread.Sleep(100);
			}
		}

		private int count;

		private const int threadIterationCount = 10000;

		private void MultiThreadedTestFunction()
		{
			for (int i = 0; i < threadIterationCount; i++)
			{
				// All we need to do is test the registration and removal of proxies.
				RegisterAndRetrieveModel();
				RegisterAndRemoveModel();
				HasModel();
			}

			count--;
		}
	}
}
