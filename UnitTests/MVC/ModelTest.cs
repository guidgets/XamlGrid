/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using System.Collections.Generic;
using Company.Widgets.Core;
using NUnit.Framework;



namespace UnitTests.MVC
{
    /**
	 * Test the PureMVC Model class.
	 * 
	 * @see org.puremvc.interfaces.IModel IModel
	 * @see org.puremvc.patterns.Model.Model Model
	 */
    [TestFixture]
    public class ModelTest
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
  		 * Tests getting the name using Model class accessor method. Setting can only be done in constructor.
  		 */
		[Test]
		[Description("Model Tests")]
		public void NameAccessor()
        {
			// Create a new Model and use accessors to set the Model name 
   			IModel Model = new Model("TestModel");
   			
   			// test assertions
   			Assert.IsTrue(Model.ModelName == "TestModel", "Expecting Model.ModelName == 'TestModel'");
   		}

  		/**
  		 * Tests setting and getting the data using Model class accessor methods.
  		 */
		[Test]
		[Description("Model Tests")]
		public void TestDataAccessors()
        {
			// Create a new Model and use accessors to set the data
   			IModel Model = new Model("colors");
			Model.Data = new List<string>(new[] { "red", "green", "blue" });
			List<string> data = (List<string>) Model.Data;
   			
   			// test assertions
   			Assert.IsTrue(data.Count == 3, "Expecting data.Count == 3");
   			Assert.IsTrue(data[0] == "red", "Expecting data[0] == 'red'");
   			Assert.IsTrue(data[1] == "green", "Expecting data[1] == 'green'");
   			Assert.IsTrue(data[2] == "blue", "Expecting data[2] == 'blue'");
   		}

  		/**
  		 * Tests setting the name and body using the Notification class Constructor.
  		 */
		[Test]
		[Description("Model Tests")]
		public void TestConstructor()
        {
			// Create a new Model using the Constructor to set the name and data
			IModel Model = new Model("colors", new List<string>(new[] { "red", "green", "blue" }));
			List<string> data = (List<string>) Model.Data;
   			
   			// test assertions
   			Assert.IsNotNull(Model, "Expecting Model not null");
   			Assert.IsTrue(Model.ModelName == "colors", "Expecting Model.ModelName == 'colors'");
            Assert.IsTrue(data.Count == 3, "Expecting data.Count == 3");
            Assert.IsTrue(data[0] == "red", "Expecting data[0] == 'red'");
            Assert.IsTrue(data[1] == "green", "Expecting data[1] == 'green'");
            Assert.IsTrue(data[2] == "blue", "Expecting data[2] == 'blue'");
   		}
    }
}
