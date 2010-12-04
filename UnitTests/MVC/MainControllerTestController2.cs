/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using System.Collections.Generic;

using Company.Widgets.Core;


namespace UnitTests.MVC
{
    /**
  	 * A Controller class used by MainControllerTest.
  	 * 
  	 * @see org.puremvc.csharp.core.MainController.MainControllerTest MainControllerTest
  	 */
    public class MainControllerTestController2 : Controller
    {
        /**
		 * The Controller name
		 */
        public new static string NAME = "MainControllerTestController2";

        /**
         * Constructor
         */
        public MainControllerTestController2(object MainController)
            : base(NAME, MainController)
        { }

		override public IList<int> ListNotificationInterests()
        {
            // be sure that the Controller has some Observers created
            // in order to test removeController
			return new List<int>(new[] { MainControllerTest.NOTE1, MainControllerTest.NOTE2 });
        }

        override public void HandleNotification(INotification notification)
		{
			base.HandleNotification(notification);
			MainControllerTest.lastNotification = notification.Code;
		}

        public MainControllerTest MainControllerTest
		{
            get { return (MainControllerTest) this.ViewComponent; }
		}
    }
}
