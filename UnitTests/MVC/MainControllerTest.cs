/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Guidgets.XamlGrid.Core;


namespace UnitTests.MVC
{
    /**
	 * Test the PureMVC MainController class.
	 */
	[TestFixture]
	public class MainControllerTest
    {
        public int? lastNotification;	
  		public bool onRegisterCalled;
  		public bool onRemoveCalled;
  		public Int32 counter;
  		
 		public const int NOTE1 = 1;
		public const int NOTE2 = 2;
		public const int NOTE3 = 3;
		public const int NOTE4 = 4;
		public const int NOTE5 = 5;
		public const int NOTE6 = 6;

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
  		 * Tests the MainController Singleton Factory Method 
  		 */
		[Test]
		[Description("MainController Tests")]
		public void GetInstance()
        {
   			// Test Factory Method
   			IMainController mainController = MainController.Instance;
   			
   			// test assertions
            Assert.IsNotNull(mainController, "Expecting instance not null");
   			Assert.IsTrue(mainController != null, "Expecting instance implements IMainController");
   			
   		}

  		/**
  		 * Tests registration and notification of Observers.
  		 * 
  		 * <P>
  		 * An Observer is created to callback the MainControllerTestMethod of
  		 * this MainControllerTest instance. This Observer is registered with
  		 * the MainController to be notified of 'MainControllerTestEvent' events. Such
  		 * an event is created, and a value set on its payload. Then
  		 * the MainController is told to notify interested observers of this
  		 * Event. 
  		 * 
  		 * <P>
  		 * The MainController calls the Observer's notifyObserver method
  		 * which calls the MainControllerTestMethod on this instance
  		 * of the MainControllerTest class. The MainControllerTestMethod method will set 
  		 * an instance variable to the value passed in on the Event
  		 * payload. We evaluate the instance variable to be sure
  		 * it is the same as that passed out as the payload of the 
  		 * original 'MainControllerTestEvent'.
  		 * 
 		 */
		[Test]
		[Description("MainController Tests")]
		public void RegisterAndNotifyObserver()
        {
  			// Get the Singleton MainController instance
  			IMainController mainController = MainController.Instance;
  			
   			// Create observer, passing in notification method and context
   			IObserver observer = new Observer(this.MainControllerTestMethod, this);
   			
   			// Register Observer's interest in a particulat Notification with the MainController 
			int name = Thread.CurrentThread.ManagedThreadId;

			lock (m_MainControllerTestVarsLock)
			{
				this.mainControllerTestVars.Remove(name);
			}

			mainController.RegisterObserver(MainControllerTestNote.NAME + name, observer);
  			
   			// Create a MainControllerTestNote, setting 
   			// a body value, and tell the MainController to notify 
   			// Observers. Since the Observer is this class 
   			// and the notification method is MainControllerTestMethod,
   			// successful notification will result in our local 
   			// MainControllerTestVar being set to the value we pass in 
   			// on the note body.
			INotification note = MainControllerTestNote.Create(name, 10);
			mainController.NotifyObservers(note);

			// test assertions  			
			Assert.IsTrue(this.mainControllerTestVars.ContainsKey(name), "Expecting MainControllerTestVars.ContainsKey(name)");
			Assert.IsTrue(this.mainControllerTestVars[name] == 10, "Expecting MainControllerTestVar[name] = 10");

			mainController.RemoveObserver(MainControllerTestNote.NAME + name, this);
   		}
   		
  		/**
  		 * A test variable that proves the MainControllerTestMethod was
  		 * invoked by the MainController.
  		 */
  		private readonly IDictionary<int, int> mainControllerTestVars = new Dictionary<int, int>();

		private readonly object m_MainControllerTestVarsLock = new object();

  		/**
  		 * A utility method to test the notification of Observers by the MainController
  		 */
		public void MainControllerTestMethod(INotification note)
  		{
  			// set the local MainControllerTestVar to the number on the event payload
			int name = Thread.CurrentThread.ManagedThreadId;

			lock (m_MainControllerTestVarsLock)
			{
				this.mainControllerTestVars.Remove(name);
				this.mainControllerTestVars.Add(name, (int) note.Body);
			}
  		}

		/**
		 * Tests registering and retrieving a Controller with
		 * the MainController.
		 */
		[Test]
		[Description("MainController Tests")]
		public void RegisterAndRetrieveController()
        {
  			// Get the Singleton MainController instance
  			IMainController mainController = MainController.Instance;

			// Create and register the test Controller
			IController MainControllerTestController = new MainControllerTestController(Thread.CurrentThread.Name, this);
			string name = MainControllerTestController.Name;
			mainController.RegisterController(MainControllerTestController);

			// Retrieve the component
			IController Controller = mainController.RetrieveController(name);
			
			// test assertions  			
   			Assert.IsTrue(Controller is MainControllerTestController, "Expecting comp is MainControllerTestController");
			// Remove our Controller
			mainController.RemoveController(name);
		}
 		
  		/**
  		 * Tests the hasController Method
  		 */
		[Test]
		[Description("MainController Tests")]
		public void HasController()
		{
  			
   			// register a Controller
   			IMainController mainController = MainController.Instance;
			
			// Create and register the test Controller
			string name = "HasControllerTest" + Thread.CurrentThread.Name;
			Controller<object> Controller = new Controller<object>(name, this);
			mainController.RegisterController(Controller);
			
   			// assert that the MainController.hasController method returns true
   			// for that Controller name
			Assert.IsTrue(mainController.HasController(name), "Expecting MainController.hasController(name) == true");

			mainController.RemoveController(name);
			
   			// assert that the MainController.hasController method returns false
   			// for that Controller name
			Assert.IsTrue(mainController.HasController(name) == false, "Expecting MainController.hasController(name) == false");
   		}

		/**
		 * Tests registering and removing a Controller 
		 */
		[Test]
		[Description("MainController Tests")]
		public void RegisterAndRemoveController()
        {
  			// Get the Singleton MainController instance
  			IMainController mainController = MainController.Instance;

			// Create and register the test Controller, 
			// but not so we have a reference to it
			string name = "Testing" + Thread.CurrentThread.Name;
			mainController.RegisterController(new Controller<object>(name, this));
			
			// Remove the component
			IController removedController = mainController.RemoveController(name);
			
			// test assertions  		
			Assert.IsTrue(removedController.Name == name, "Expecting removedController.Name == name");
			Assert.IsNull(mainController.RetrieveController(name), "Expecting MainController.retrieveController(name) == null");

			mainController.RemoveController(name);
		}
		
		/**
		 * Tests that the MainController callse the onRegister and onRemove methods
		 */
		[Test]
		[Description("MainController Tests")]
		public void OnRegisterAndOnRemove()
		{
			
  			// Get the Singleton MainController instance
  			IMainController mainController = MainController.Instance;

			// Create and register the test Controller
			IController Controller = new MainControllerTestController4(this);
			string name = Controller.Name;
			mainController.RegisterController(Controller);

			// assert that onRegsiter was called, and the Controller responded by setting our boolean
   			Assert.IsTrue(onRegisterCalled, "Expecting onRegisterCalled == true");
				
			// Remove the component
			mainController.RemoveController(name);
			
			// assert that the Controller is no longer retrievable
   			Assert.IsTrue(onRemoveCalled, "Expecting onRemoveCalled == true");
		}

		/**
		 * Tests successive regster and remove of same Controller.
		 */
		[Test]
		[Description("MainController Tests")]
		public void SuccessiveRegisterAndRemoveController()
        {
  			// Get the Singleton MainController instance
  			IMainController mainController = MainController.Instance;

			// Create and register the test Controller, 
			// but not so we have a reference to it
			IController MainControllerTestController = new MainControllerTestController(Thread.CurrentThread.Name, this);
			string name = MainControllerTestController.Name;
			mainController.RegisterController(MainControllerTestController);
			
			// test that we can retrieve it
			Assert.IsTrue(mainController.RetrieveController(name) is MainControllerTestController, "Expecting MainController.retrieveController( MainControllerTestController.NAME ) is MainControllerTestController"); 

			// Remove the Controller
			mainController.RemoveController(name);

			// test that retrieving it now returns null			
			Assert.IsNull(mainController.RetrieveController(name), "Expecting MainController.retrieveController( MainControllerTestController.NAME ) == null");

			// test that removing the Controller again once its gone doesn't cause crash 	
            try
            {
				mainController.RemoveController(name);
            }
            catch
            {
                Assert.Fail("Expecting MainController.removeController( MainControllerTestController.NAME ) doesn't crash", null);
            }

			// Create and register another instance of the test Controller, 
			MainControllerTestController = new MainControllerTestController(Thread.CurrentThread.Name, this);
			name = MainControllerTestController.Name;
			mainController.RegisterController(MainControllerTestController);

			Assert.IsTrue(mainController.RetrieveController(name) is MainControllerTestController, "Expecting MainController.retrieveController( MainControllerTestController.NAME ) is MainControllerTestController"); 

			// Remove the Controller
			mainController.RemoveController(name);
			
			// test that retrieving it now returns null			
			Assert.IsNull(mainController.RetrieveController(name), "Expecting MainController.retrieveController( MainControllerTestController.NAME ) == null");
		}
		
        /**
		 * Tests registering a Controller for 3 different notifications, removing the
		 * Controller from the MainController, and seeing that neither notification causes the
		 * Controller to be notified. Added for the fix deployed in version 1.7
		 */
		[Test]
		[Description("MainController Tests")]
		public void RemoveControllerAndSubsequentNotify()
        {			
  			// Get the Singleton MainController instance
  			IMainController mainController = MainController.Instance;
			
			// Create and register the test Controller to be removed.
			mainController.RegisterController(new MainControllerTestController2(this));
			
			// Create and register the Controller to remain
			IController mainControllerTestController = new MainControllerTestController(Thread.CurrentThread.Name, this);
			mainController.RegisterController(mainControllerTestController);
			
			// test that notifications work
   			mainController.NotifyObservers(new Notification(NOTE1));
            Assert.IsTrue(lastNotification == NOTE1, "Expecting lastNotification == NOTE1");

            mainController.NotifyObservers(new Notification(NOTE2));
            Assert.IsTrue(lastNotification == NOTE2, "Expecting lastNotification == NOTE2");
		   			
			// Remove the Controller
			mainController.RemoveController(MainControllerTestController2.NAME);

			// test that retrieving it now returns null			
   			Assert.IsNull(mainController.RetrieveController(MainControllerTestController2.NAME), "Expecting MainController.retrieveController(MainControllerTestController2.NAME) == null");

			// test that notifications no longer work
			// (MainControllerTestController2 is the one that sets lastNotification
			// on this component, and MainControllerTestController)
			lastNotification = null;

            mainController.NotifyObservers(new Notification(NOTE1));
            Assert.IsTrue(lastNotification != NOTE1, "Expecting lastNotification != NOTE1");

            mainController.NotifyObservers(new Notification(NOTE2));
            Assert.IsTrue(lastNotification != NOTE2, "Expecting lastNotification != NOTE2");

			Cleanup();						  			
		}

        /**
		 * Tests registering one of two registered Controllers and seeing
		 * that the remaining one still responds.
		 * Added for the fix deployed in version 1.7.1
		 */
		[Test]
		[Description("MainController Tests")]
		public void RemoveOneOfTwoControllersAndSubsequentNotify()
        {
  			// Get the Singleton MainController instance
  			IMainController mainController = MainController.Instance;
			
			// Create and register that responds to notifications 1 and 2
			mainController.RegisterController(new MainControllerTestController2(this));
			
			// Create and register that responds to notification 3
			mainController.RegisterController(new MainControllerTestController3(this));
			
			// test that all notifications work
            mainController.NotifyObservers(new Notification(NOTE1));
            Assert.IsTrue(lastNotification == NOTE1, "Expecting lastNotification == NOTE1");

            mainController.NotifyObservers(new Notification(NOTE2));
            Assert.IsTrue(lastNotification == NOTE2, "Expecting lastNotification == NOTE2");

            mainController.NotifyObservers(new Notification(NOTE3));
            Assert.IsTrue(lastNotification == NOTE3, "Expecting lastNotification == NOTE3");
		   			
			// Remove the Controller that responds to 1 and 2
			mainController.RemoveController(MainControllerTestController2.NAME);

			// test that retrieving it now returns null				
            Assert.IsNull(mainController.RetrieveController(MainControllerTestController2.NAME), "Expecting MainController.retrieveController(MainControllerTestController2.NAME) == null");

			// test that notifications no longer work
			// for notifications 1 and 2, but still work for 3
            lastNotification = null;

            mainController.NotifyObservers(new Notification(NOTE1));
            Assert.IsTrue(lastNotification != NOTE1, "Expecting lastNotification != NOTE1");

            mainController.NotifyObservers(new Notification(NOTE2));
            Assert.IsTrue(lastNotification != NOTE2, "Expecting lastNotification != NOTE2");

            mainController.NotifyObservers(new Notification(NOTE3));
            Assert.IsTrue(lastNotification == NOTE3, "Expecting lastNotification == NOTE3");

			Cleanup();						  			
		}
		
		/**
		 * Tests registering the same Controller twice. 
		 * A subsequent notification should only illicit
		 * one response. Also, since reregistration
		 * was causing 2 observers to be created, ensure
		 * that after removal of the Controller there will
		 * be no further response.
		 * 
		 * Added for the fix deployed in version 2.0.4
		 */
		[Test]
		[Description("MainController Tests")]
		public void ControllerReregistration()
		{
			
  			// Get the Singleton MainController instance
  			IMainController mainController = MainController.Instance;
			
			// Create and register that responds to notification 5
			mainController.RegisterController(new MainControllerTestController5( this ) );
			
			// try to register another instance of that Controller (uses the same NAME constant).
			mainController.RegisterController( new MainControllerTestController5( this ) );
			
			// test that the counter is only incremented once (Controller 5's response) 
			counter = 0;
   			mainController.NotifyObservers( new Notification(NOTE5) );
			Assert.IsTrue(counter == 1, "Expecting counter == 1");

			// Remove the Controller 
			mainController.RemoveController( MainControllerTestController5.NAME );

			// test that retrieving it now returns null			
   			Assert.IsTrue(mainController.RetrieveController( MainControllerTestController5.NAME ) == null, "Expecting MainController.retrieveController( MainControllerTestController5.NAME ) == null");

			// test that the counter is no longer incremented  
			counter = 0;
   			mainController.NotifyObservers( new Notification(NOTE5) );
   			Assert.IsTrue(counter == 0, "Expecting counter == 0");

			Cleanup();
		}
		
		
		/**
		 * Tests the ability for the observer list to 
		 * be modified during the process of notification,
		 * and all observers be properly notified. This
		 * happens most often when multiple Controllers
		 * respond to the same notification by removing
		 * themselves.  
		 * 
		 * Added for the fix deployed in version 2.0.4
		 */
		[Test]
		[Description("MainController Tests")]
		public void ModifyObserverListDuringNotification()
		{
			
  			// Get the Singleton MainController instance
  			IMainController mainController = MainController.Instance;
			
			// Create and register several Controller instances that respond to notification 6 
			// by removing themselves, which will cause the observer list for that notification 
			// to change. versions prior to Standard Version 2.0.4 will see every other Controller
			// fails to be notified.  
			mainController.RegisterController( new MainControllerTestController6(  "MainControllerTestController6/1", this ) );
			mainController.RegisterController( new MainControllerTestController6(  "MainControllerTestController6/2", this ) );
			mainController.RegisterController( new MainControllerTestController6(  "MainControllerTestController6/3", this ) );
			mainController.RegisterController( new MainControllerTestController6(  "MainControllerTestController6/4", this ) );
			mainController.RegisterController( new MainControllerTestController6(  "MainControllerTestController6/5", this ) );
			mainController.RegisterController( new MainControllerTestController6(  "MainControllerTestController6/6", this ) );
			mainController.RegisterController( new MainControllerTestController6(  "MainControllerTestController6/7", this ) );
			mainController.RegisterController( new MainControllerTestController6(  "MainControllerTestController6/8", this ) );

			// clear the counter
			counter = 0;
			// send the notification. each of the above Controllers will respond by removing
			// themselves and incrementing the counter by 1. This should leave us with a
			// count of 8, since 8 Controllers will respond.
			mainController.NotifyObservers( new Notification( NOTE6 ) );
			// verify the count is correct
   			Assert.IsTrue(counter == 8, "Expecting counter == 8");
	
			// clear the counter
			counter=0;
			mainController.NotifyObservers( new Notification( NOTE6 ) );
			// verify the count is 0
   			Assert.IsTrue(counter == 0, "Expecting counter == 0");

			Cleanup();
		}

		private static void Cleanup()
		{
            MainController.Instance.RemoveController(MainControllerTestController.NAME);
            MainController.Instance.RemoveController(MainControllerTestController2.NAME);
            MainController.Instance.RemoveController(MainControllerTestController3.NAME);
			MainController.Instance.RemoveController(MainControllerTestController4.NAME);
			MainController.Instance.RemoveController(MainControllerTestController5.NAME);
			MainController.Instance.RemoveController(MainControllerTestController6.NAME);
		}

		/// <summary>
		/// Test all of the function above using many threads at once.
		/// </summary>
		[Test]
		[Description("MainController Tests")]
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
				// All we need to do is test the registration and removal of Controllers and observers
				RegisterAndNotifyObserver();
				RegisterAndRetrieveController();
				HasController();
				RegisterAndRemoveController();
			}

			count--;
		}
	}
}
