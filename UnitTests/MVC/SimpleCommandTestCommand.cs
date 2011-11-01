/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

using Guidgets.XamlGrid.Core;


namespace UnitTests.MVC
{
    /**
	 * A SimpleCommand subclass used by SimpleCommandTest.
	 *
  	 * @see org.puremvc.csharp.patterns.command.SimpleCommandTest SimpleCommandTest
  	 * @see org.puremvc.csharp.patterns.command.SimpleCommandTestVO SimpleCommandTestVO
	 */
    public class SimpleCommandTestCommand : SimpleCommand
    {
        /**
		 * Constructor.
		 */

    	/**
		 * Fabricate a result by multiplying the input by 2
		 * 
		 * @param event the <code>INotification</code> carrying the <code>SimpleCommandTestVO</code>
		 */
		public override void Execute(INotification note)
		{
			base.Execute(note);
			SimpleCommandTestVO vo = (SimpleCommandTestVO) note.Body;
			
			// Fabricate a result
			vo.result = 2 * vo.input;
		}
    }
}
