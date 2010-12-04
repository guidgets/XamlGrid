using System.Collections.Generic;
/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using System.Threading;
using Company.Widgets.Core;
using NUnit.Framework;




namespace UnitTests.MVC
{
	/**
	 * Test the PureMVC MainCommand class.
	 */
	[TestFixture]
	public class MainCommandTest
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
  		 * Tests the MainCommand Singleton Factory Method 
  		 */
		[Test]
		[Description("MainCommand Tests")]
		public void GetInstance()
        {
   			// Test Factory Method
   			IMainCommand mainCommand = MainCommand.Instance;
   			
   			// test assertions
            Assert.IsNotNull(mainCommand, "Expecting instance not null");
            Assert.IsTrue(mainCommand != null, "Expecting instance implements IMainCommand");
   		}

        /**
  		 * Tests Command registration and execution.
  		 * 
  		 * <P>
  		 * This test gets the Singleton MainCommand instance 
  		 * and registers the MainCommandTestCommand class 
  		 * to handle 'MainCommandTest' Notifications.<P>
  		 * 
  		 * <P>
  		 * It then constructs such a Notification and tells the 
  		 * MainCommand to execute the associated Command.
  		 * Success is determined by evaluating a property
  		 * on an object passed to the Command, which will
  		 * be modified when the Command executes.</P>
  		 * 
  		 */
		[Test]
		[Description("MainCommand Tests")]
		public void RegisterAndExecuteCommand() 
        {
   			// Create the MainCommand, register the MainCommandTestCommand to handle 'MainCommandTest' notes
   			IMainCommand mainCommand = MainCommand.Instance;
			int name = int.MinValue + Thread.CurrentThread.ManagedThreadId;
   			mainCommand.RegisterCommand(name, typeof(MainCommandTestCommand));
   			
   			// Create a 'MainCommandTest' note
            MainCommandTestVO vo = new MainCommandTestVO(12);
   			INotification note = new Notification(name, vo);

			// Tell the MainCommand to execute the Command associated with the note
			// the MainCommandTestCommand invoked will multiply the vo.input value
			// by 2 and set the result on vo.result
   			mainCommand.ExecuteCommand(note);
   			
   			// test assertions 
            Assert.IsTrue(vo.result == 24, "Expecting vo.result == 24");
   		}

        /**
  		 * Tests Command registration and removal.
  		 * 
  		 * <P>
  		 * Tests that once a Command is registered and verified
  		 * working, it can be removed from the MainCommand.</P>
  		 */
		[Test]
		[Description("MainCommand Tests")]
		public void RegisterAndRemoveCommand()
        {
   			// Create the MainCommand, register the MainCommandTestCommand to handle 'MainCommandTest' notes
			IMainCommand mainCommand = MainCommand.Instance;
			int name = int.MinValue + Thread.CurrentThread.ManagedThreadId;
			mainCommand.RegisterCommand(name, typeof(MainCommandTestCommand));

			// Create a 'MainCommandTest' note
			MainCommandTestVO vo = new MainCommandTestVO(12);
			INotification note = new Notification(name, vo);

			// Tell the MainCommand to execute the Command associated with the note
			// the MainCommandTestCommand invoked will multiply the vo.input value
			// by 2 and set the result on vo.result
			mainCommand.ExecuteCommand(note);

			// test assertions 
			Assert.IsTrue(vo.result == 24, "Expecting vo.result == 24");

			// Reset result
			vo.result = 0;

			// Remove the Command from the MainCommand
			mainCommand.RemoveCommand(name);

			// Tell the MainCommand to execute the Command associated with the
			// note. This time, it should not be registered, and our vo result
			// will not change   			
			mainCommand.ExecuteCommand(note);

			// test assertions 
			Assert.IsTrue(vo.result == 0, "Expecting vo.result == 0");
   		}
  		
  		/**
  		 * Test hasCommand method.
  		 */
		[Test]
		[Description("MainCommand Tests")]
		public void HasCommand()
		{
   			// register the MainCommandTestCommand to handle 'hasCommandTest' notes
			IMainCommand mainCommand = MainCommand.Instance;
			int name = int.MinValue + Thread.CurrentThread.ManagedThreadId;
			mainCommand.RegisterCommand(name, typeof(MainCommandTestCommand));

			// test that hasCommand returns true for hasCommandTest notifications 
			Assert.IsTrue(mainCommand.HasCommand(name), "Expecting MainCommand.HasCommand(name) == true");

			// Remove the Command from the MainCommand
			mainCommand.RemoveCommand(name);

			// test that hasCommand returns false for hasCommandTest notifications 
			Assert.IsTrue(mainCommand.HasCommand(name) == false, "Expecting MainCommand.HasCommand(name) == false");
   		}
   		
 		/**
  		 * Tests Removing and Reregistering a Command
  		 * 
  		 * <P>
  		 * Tests that when a Command is re-registered that it isn't fired twice.
  		 * This involves, minimally, registration with the MainCommand but
  		 * notification via the MainController, rather than direct execution of
  		 * the MainCommand's executeCommand method as is done above in 
  		 * testRegisterAndRemove. The bug under test was fixed in AS3 Standard 
  		 * Version 2.0.2. If you run the unit tests with 2.0.1 this
  		 * test will fail.</P>
  		 */
		[Test]
		[Description("MainCommand Tests")]
		public void ReregisterAndExecuteCommand()
		{
  			 
   			// Fetch the MainCommand, register the MainCommandTestCommand2 to handle 'MainCommandTest2' notes
			IMainCommand mainCommand = MainCommand.Instance;
			int name = int.MinValue + Thread.CurrentThread.ManagedThreadId;
			mainCommand.RegisterCommand(name, typeof(MainCommandTestCommand2));

			// Remove the Command from the MainCommand
			mainCommand.RemoveCommand(name);

			// Re-register the Command with the MainCommand
			mainCommand.RegisterCommand(name, typeof(MainCommandTestCommand2));

			// Create a 'MainCommandTest2' note
			MainCommandTestVO vo = new MainCommandTestVO(12);
			Notification note = new Notification(name, vo);

			// retrieve a reference to the MainController.
			IMainController mainController = MainController.Instance;

			// send the Notification
			mainController.NotifyObservers(note);

			// test assertions 
			// if the command is executed once the value will be 24
			Assert.IsTrue(vo.result == 24, "Expecting vo.result == 24");

			// Prove that accumulation works in the VO by sending the notification again
			mainController.NotifyObservers(note);

			// if the command is executed twice the value will be 48
			Assert.IsTrue(vo.result == 48, "Expecting vo.result == 48");
   		}

		/// <summary>
		/// Test all of the function above using many threads at once.
		/// </summary>
		[Test]
		[Description("MainCommand Tests")]
		public void MultiThreadedOperations()
		{
			count = 20;
			IList<Thread> threads = new List<Thread>();

			for (int i = 0; i < count; i++) {
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
				// All we need to do is test the registration and removal of commands.
				RegisterAndExecuteCommand();
				RegisterAndRemoveCommand();
				HasCommand();
				ReregisterAndExecuteCommand();
			}

			count--;
		}
	}
}
