﻿/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

using Company.Widgets.Core;


namespace UnitTests.MVC
{
    /**
	 * A MacroCommand subclass used by MacroCommandTest.
	 *
  	 * @see org.puremvc.csharp.patterns.command.MacroCommandTest MacroCommandTest
  	 * @see org.puremvc.csharp.patterns.command.MacroCommandTestSub1Command MacroCommandTestSub1Command
  	 * @see org.puremvc.csharp.patterns.command.MacroCommandTestSub2Command MacroCommandTestSub2Command
  	 * @see org.puremvc.csharp.patterns.command.MacroCommandTestVO MacroCommandTestVO
	 */
    public class MacroCommandTestCommand : MacroCommand
    {
        /**
		 * Constructor.
		 */

    	/**
		 * Initialize the MacroCommandTestCommand by adding
		 * its 2 SubCommands.
		 */
		protected override void InitializeMacroCommand()
		{
			base.InitializeMacroCommand();
			AddSubCommand(typeof(MacroCommandTestSub1Command));
			AddSubCommand(typeof(MacroCommandTestSub2Command));
		}
    }
}
