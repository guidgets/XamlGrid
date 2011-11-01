/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

using Guidgets.XamlGrid.Core;


namespace UnitTests.MVC
{
    /**
	 * A SimpleCommand subclass used by MainCommandTest.
	 *
  	 * @see org.puremvc.csharp.core.MainCommand.MainCommandTest MainCommandTest
  	 * @see org.puremvc.csharp.core.MainCommand.MainCommandTestVO MainCommandTestVO
	 */
    public class MainCommandTestCommand : SimpleCommand
    {
        /**
		 * Constructor.
		 */

    	/**
		 * Fabricate a result by multiplying the input by 2
		 * 
		 * @param note the note carrying the MainCommandTestVO
		 */
		override public void Execute( INotification note )
		{

			MainCommandTestVO vo = (MainCommandTestVO) note.Body;
			
			// Fabricate a result
			vo.result = 2 * vo.input;

		}
    }
}
