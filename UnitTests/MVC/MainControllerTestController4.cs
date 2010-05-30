/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

using Company.Widgets.Core;


namespace UnitTests.MVC
{
   	/**
  	 * A Controller class used by MainControllerTest.
  	 * 
  	 * @see org.puremvc.as3.core.MainController.MainControllerTest MainControllerTest
  	 */
	public class MainControllerTestController4 : Controller 
	{
		/**
		 * The Controller name
		 */
		public new static string NAME = "MainControllerTestController4";
				
		/**
		 * Constructor
		 */
		public MainControllerTestController4(object MainController)
			: base(NAME, MainController)
		{
		}

        public MainControllerTest MainControllerTest
		{
			get { return (MainControllerTest) this.ViewComponent; }
		}
				
		public override void OnRegister()
		{
			MainControllerTest.onRegisterCalled = true;
		}
				
		public override  void OnRemove()
		{
			MainControllerTest.onRemoveCalled = true;
		}
				
				
	}
}