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
  	 * @see org.puremvc.as3.core.MainController.MainControllerTest MainControllerTest
  	 */
	public class MainControllerTestController5 :Controller 
	{
		/**
		 * The Controller name
		 */
		public new static string NAME = "MainControllerTestController5";
				
		/**
		 * Constructor
		 */
		public MainControllerTestController5(object viewComponent)
			: base(NAME, viewComponent)
		{
		}

		public override IList<int> ListNotificationInterests()
		{
			return new List<int>(new[] { MainControllerTest.NOTE5 });
		}

		public override void HandleNotification(INotification note)
		{
			MainControllerTest.counter++;
		}

        public MainControllerTest MainControllerTest
		{
			get { return (MainControllerTest) this.ViewComponent; }
		}
	}
}
