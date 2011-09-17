/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using System.Collections.Generic;
using XamlGrid.Core;


namespace UnitTests.MVC
{
    /**
  	 * A Controller class used by MainControllerTest.
  	 * 
  	 * @see org.puremvc.csharp.core.MainController.MainControllerTest MainControllerTest
  	 */
    public class MainControllerTestController : Controller
    {
        /**
		 * The Controller name
		 */
		public new static string NAME = "MainControllerTestController";
		
		/**
		 * Constructor
		 */
		public MainControllerTestController(string threadName, object MainController) 
            : base(NAME + threadName, MainController)
        { }

		override public IList<int> ListNotificationInterests()
		{
			// be sure that the Controller has some Observers created
			// in order to test removeController
			return new List<int>(new[]{10, 20, 30});
		}
    }
}
