/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using System.Collections.Generic;
using Company.DataGrid.Core;
using NUnit.Framework;




namespace UnitTests.MVC
{
    /**
	 * Test the PureMVC Facade class.
	 *
  	 * @see org.puremvc.csharp.patterns.facade.FacadeTestVO FacadeTestVO
  	 * @see org.puremvc.csharp.patterns.facade.FacadeTestCommand FacadeTestCommand
	 */
    [TestFixture]
    public class FacadeTest
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
  		 * Tests the Facade Singleton Factory Method 
  		 */
		[Test]
		[Description("Facade Tests")]
		public void GetInstance()
        {
   			// Test Factory Method
			IFacade facade = Facade.Instance;
   			
   			// test assertions
            Assert.IsNotNull(facade, "Expecting instance not null");
   			Assert.IsTrue(facade != null, "Expecting instance implements IFacade");
   		}

        /**
  		 * Tests Command registration and execution via the Facade.
  		 * 
  		 * <P>
  		 * This test gets the Singleton Facade instance 
  		 * and registers the FacadeTestCommand class 
  		 * to handle 'FacadeTest' Notifcations.<P>
  		 * 
  		 * <P>
  		 * It then constructs such a Notification and notifies Observers
  		 * via the Facade. Success is determined by evaluating 
  		 * a property on an object placed in the body of
  		 * the Notification, which will be modified by the Command.</P>
  		 * 
  		 */
		[Test]
		[Description("Facade Tests")]
		public void RegisterCommandAndNotifyObservers()
        {
   			// Create the Facade, register the FacadeTestCommand to 
   			// handle 'FacadeTest' events
			IFacade facade = Facade.Instance;
   			facade.RegisterCommand("FacadeTestNote", typeof(FacadeTestCommand));

			// Send notification. The Command associated with the event
			// (FacadeTestCommand) will be invoked, and will multiply 
			// the vo.input value by 2 and set the result on vo.result
			FacadeTestVO vo = new FacadeTestVO(32);
            facade.SendNotification("FacadeTestNote", vo);
   			
   			// test assertions 
   			Assert.IsTrue(vo.result == 64, "Expecting vo.result == 64");
   		}

  		/**
  		 * Tests Command removal via the Facade.
  		 * 
  		 * <P>
  		 * This test gets the Singleton Facade instance 
  		 * and registers the FacadeTestCommand class 
  		 * to handle 'FacadeTest' Notifcations. Then it removes the command.<P>
  		 * 
  		 * <P>
  		 * It then sends a Notification using the Facade. 
  		 * Success is determined by evaluating 
  		 * a property on an object placed in the body of
  		 * the Notification, which will NOT be modified by the Command.</P>
  		 * 
  		 */
		[Test]
		[Description("Facade Tests")]
		public void RegisterAndRemoveCommandAndSendNotification()
        {
   			// Create the Facade, register the FacadeTestCommand to 
   			// handle 'FacadeTest' events
			IFacade facade = Facade.Instance;
   			facade.RegisterCommand("FacadeTestNote", typeof(FacadeTestCommand));
   			facade.RemoveCommand("FacadeTestNote");

			// Send notification. The Command associated with the event
			// (FacadeTestCommand) will NOT be invoked, and will NOT multiply 
			// the vo.input value by 2 
            FacadeTestVO vo = new FacadeTestVO(32);
   			facade.SendNotification("FacadeTestNote", vo);
   			
   			// test assertions 
   			Assert.IsTrue(vo.result != 64, "Expecting vo.result != 64");
   		}

        /**
  		 * Tests the regsitering and retrieving MainModel Models via the Facade.
  		 * 
  		 * <P>
  		 * Tests <code>registerMainModelModel</code> and <code>retrieveMainModelModel</code> in the same test.
  		 * These methods cannot currently be tested separately
  		 * in any meaningful way other than to show that the
  		 * methods do not throw exception when called. </P>
  		 */
		[Test]
		[Description("Facade Tests")]
		public void RegisterAndRetrieveModel()
        {
   			// register a Model and retrieve it.
			IFacade facade = Facade.Instance;
			facade.RegisterModel(new Model("colors", new List<string>(new[] { "red", "green", "blue" })));
			IModel Model = facade.RetrieveModel("colors");
			
			// test assertions
   			Assert.IsTrue(Model != null, "Expecting Model is IModel");

			// retrieve data from Model
			List<string> data = (List<string>) Model.Data;
			
			// test assertions
   			Assert.IsNotNull(data, "Expecting data not null");
			Assert.IsTrue(data != null, "Expecting data is ArrayList");
   			Assert.IsTrue(data.Count == 3, "Expecting data.Count == 3");
   			Assert.IsTrue(data[0] == "red", "Expecting data[0] == 'red'");
            Assert.IsTrue(data[1] == "green", "Expecting data[1] == 'green'");
            Assert.IsTrue(data[2] == "blue", "Expecting data[2] == 'blue'");
   		}

        /**
  		 * Tests the removing MainModel Models via the Facade.
  		 */
		[Test]
		[Description("Facade Tests")]
		public void RegisterAndRemoveModel()
        {
   			// register a Model, remove it, then try to retrieve it
			IFacade facade = Facade.Instance;
			facade.RegisterModel(new Model("sizes", new List<int>(new[] { 7, 13, 21 })));

			IModel removedModel = facade.RemoveModel("sizes");

            Assert.IsTrue(removedModel.ModelName == "sizes", "Expecting removedModel.ModelName == 'sizes'");

			IModel Model = facade.RetrieveModel("sizes");
			
			// test assertions
   			Assert.IsNull(Model, "Expecting Model is null");
   		}

  		/**
  		 * Tests registering, retrieving and removing Controllers via the Facade.
  		 */
		[Test]
		[Description("Facade Tests")]
		public void RegisterRetrieveAndRemoveController()
        {  			
   			// register a Controller, remove it, then try to retrieve it
			IFacade facade = Facade.Instance;
			facade.RegisterController(new Controller(Controller.NAME, new object()));
			
			// retrieve the Controller
   			Assert.IsNotNull(facade.RetrieveController(Controller.NAME), "Expecting Controller is not null");

			// remove the Controller
			IController removedController = facade.RemoveController(Controller.NAME);

			// assert that we have removed the appropriate Controller
   			Assert.IsTrue(removedController.Name == Controller.NAME, "Expecting removedController.Name == Controller.NAME");
				
			// assert that the Controller is no longer retrievable
   			Assert.IsTrue(facade.RetrieveController( Controller.NAME ) == null, "Expecting facade.retrieveController(Controller.NAME) == null )");		  			
   		}

	
  		/**
  		 * Tests the hasModel Method
  		 */
		[Test]
		[Description("Facade Tests")]
		public void HasModel()
		{
   			// register a Model
			IFacade facade = Facade.Instance;
			facade.RegisterModel(new Model("hasModelTest", new List<int>(new[] { 1, 2, 3 })));
			
   			// assert that the MainModel.hasModel method returns true
   			// for that Model name
   			Assert.IsTrue(facade.HasModel("hasModelTest"), "Expecting facade.hasModel('hasModelTest') == true");
   		}

  		/**
  		 * Tests the hasController Method
  		 */
		[Test]
		[Description("Facade Tests")]
		public void HasController()
		{
   			// register a Controller
			IFacade facade = Facade.Instance;
			facade.RegisterController( new Controller( "facadeHasControllerTest", new object() ) );
			
   			// assert that the facade.hasController method returns true
   			// for that Controller name
   			Assert.IsTrue(facade.HasController("facadeHasControllerTest"), "Expecting facade.hasController('facadeHasControllerTest') == true");
   						
   			facade.RemoveController( "facadeHasControllerTest" );
   			
   			// assert that the facade.hasController method returns false
   			// for that Controller name
   			Assert.IsTrue(facade.HasController("facadeHasControllerTest") == false, "Expecting facade.hasController('facadeHasControllerTest') == false");
   		}

  		/**
  		 * Test hasCommand method.
  		 */
		[Test]
		[Description("Facade Tests")]
		public void HasCommand()
		{
   			// register the MainCommandTestCommand to handle 'hasCommandTest' notes
   			IFacade facade = Facade.Instance;
   			facade.RegisterCommand("facadeHasCommandTest", typeof(FacadeTestCommand));
   			
   			// test that hasCommand returns true for hasCommandTest notifications 
   			Assert.IsTrue(facade.HasCommand("facadeHasCommandTest"), "Expecting facade.hasCommand('facadeHasCommandTest') == true");
   			
   			// Remove the Command from the MainCommand
   			facade.RemoveCommand("facadeHasCommandTest");
			
   			// test that hasCommand returns false for hasCommandTest notifications 
   			Assert.IsTrue(facade.HasCommand("facadeHasCommandTest") == false, "Expecting facade.hasCommand('facadeHasCommandTest') == false");
   			
   		}
	}
}
