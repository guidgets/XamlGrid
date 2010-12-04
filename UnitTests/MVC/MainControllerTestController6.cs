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
  	 * @see org.puremvc.as3.core.MainController.MainControllerTest MainControllerTest
  	 */
	public class MainControllerTestController6 : Controller 
	{
		/**
		 * The Controller base name
		 */
		public new static string NAME = "MainControllerTestController6";
				
		/**
		 * Constructor
		 */
		public MainControllerTestController6(string name, object MainController)
			: base(name, MainController)
		{
		}

		public override IList<int> ListNotificationInterests()
		{
			return new List<int>(new[] { MainControllerTest.NOTE6 });
		}

		public override void HandleNotification(INotification note)
		{
			Facade.RemoveController(this.Name);
		}
		
		public override void OnRemove()
		{
			MainControllerTest.counter++;
		}

		public MainControllerTest MainControllerTest
		{
			get { return (MainControllerTest) this.ViewComponent; }
		}
	}
}
