/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

using Guidgets.XamlGrid.Core;


namespace UnitTests.MVC
{
    /**
  	 * A Notification class used by MainControllerTest.
  	 * 
  	 * @see org.puremvc.csharp.core.MainController.MainControllerTest MainControllerTest
  	 */
    public class MainControllerTestNote : Notification
    {
        /**
		 * The name of this Notification.
		 */
		public const int NAME = 100;
		
		/**
		 * Constructor.
		 * 
		 * @param name Ignored and forced to NAME.
		 * @param body the body of the Notification to be constructed.
		 */
		public MainControllerTestNote(int threadName, object body)
            : base(NAME + threadName, body)
		{ }
		
		/**
		 * Factory method.
		 * 
		 * <P> 
		 * This method creates new instances of the MainControllerTestNote class,
		 * automatically setting the note name so you don't have to. Use
		 * this as an alternative to the constructor.</P>
		 * 
		 * @param threadName the name of the thread creating this notification.
		 * @param body the body of the Notification to be constructed.
		 */
		public static INotification Create(int threadName, object body) 		
		{
			return new MainControllerTestNote(threadName, body);
		}
    }
}
