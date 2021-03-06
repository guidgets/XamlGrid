/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

using NUnit.Framework;
using Guidgets.XamlGrid.Core;


namespace UnitTests.MVC
{
    /**
	 * Test the PureMVC Notification class.
	 * 
	 * @see org.puremvc.patterns.observer.Notification Notification
	 */
    [TestFixture]
    public class NotificationTest
    {
        /**
  		 * Constructor.
  		 * 
  		 * @param methodName the name of the test method an instance to run
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
  		 * Tests setting and getting the name using Notification class accessor methods.
  		 */
		[Test]
		[Description("Notification Tests")]
		public void NameAccessors()
        {
			// Create a new Notification and use accessors to set the note name 
   			INotification note = new Notification(1);
   			
   			// test assertions
			Assert.IsTrue(note.Code == 1, "Expecting note.Name == 'TestNote'");
   		}

        /**
  		 * Tests setting and getting the body using Notification class accessor methods.
  		 */
		[Test]
		[Description("Notification Tests")]
		public void BodyAccessors()
        {
			// Create a new Notification and use accessors to set the body
   			INotification note = new Notification(-1);
   			note.Body = 5;
   			
   			// test assertions
			Assert.IsTrue((int) note.Body == 5, "Expecting (int) note.Body == 5");
   		}

        /**
  		 * Tests setting the name and body using the Notification class Constructor.
  		 */
		[Test]
		[Description("Notification Tests")]
		public void TestConstructor()
        {
			// Create a new Notification using the Constructor to set the note name and body
   			INotification note = new Notification(1, 5, "TestNoteType");
   			
   			// test assertions
			Assert.IsTrue(note.Code == 1, "Expecting note.Name == 'TestNote'");
			Assert.IsTrue((int) note.Body == 5, "Expecting (int) note.Body == 5");
   			Assert.IsTrue(note.Type == "TestNoteType", "Expecting note.Type == 'TestNoteType'");
   		}
   		
  		/**
  		 * Tests the toString method of the notification
  		 */
		[Test]
		[Description("Notification Tests")]
		public void TestToString()
		{

			// Create a new Notification and use accessors to set the note name 
   			INotification note = new Notification(1, "1,3,5", "TestType");
   			string ts = "Notification Name: 1\nBody:1,3,5\nType:TestType";
   			
   			// test assertions
			Assert.IsTrue(note.ToString() == ts, "Expecting note.testToString() == '" + ts + "'");
   		}
    }
}
