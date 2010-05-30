/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

using Company.Widgets.Core;
using NUnit.Framework;



namespace UnitTests.MVC
{
    /**
	 * Test the PureMVC Controller class.
	 * 
	 * @see org.puremvc.csharp.interfaces.IController IController
	 * @see org.puremvc.csharp.patterns.Controller.Controller Controller
	 */
    [TestFixture]
    public class ControllerTest
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
  		 * Tests getting the name using Controller class accessor method. 
  		 */
		[Test]
		[Description("Controller Tests")]
		public void NameAccessor()
        {
			// Create a new Controller and use accessors to set the Controller name 
   			IController Controller = new Controller("TestController");
   			
   			// test assertions
            Assert.IsTrue(Controller.Name == "TestController", "Expecting Controller.Name == 'TestController'");
   		}

        /**
  		 * Tests getting the name using Controller class accessor method. 
  		 */
		[Test]
		[Description("Controller Tests")]
		public void MainControllerAccessor()
        {
			// Create a MainController object
			object MainController = new object();
			
			// Create a new Model and use accessors to set the Model name 
            IController Controller = new Controller("TestController", MainController);
			   			
   			// test assertions
   			Assert.IsNotNull(Controller.ViewComponent, "Expecting Controller.MainControllerComponent not null");
   		}
    }
}
