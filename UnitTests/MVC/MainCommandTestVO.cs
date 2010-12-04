/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
namespace UnitTests.MVC
{
    /**
  	 * A utility class used by MainCommandTest.
  	 * 
  	 * @see org.puremvc.csharp.core.MainCommand.MainCommandTest MainCommandTest
  	 * @see org.puremvc.csharp.core.MainCommand.MainCommandTestCommand MainCommandTestCommand
  	 */
    public class MainCommandTestVO
    {
        /**
		 * Constructor.
		 * 
		 * @param input the number to be fed to the MainCommandTestCommand
		 */
		public MainCommandTestVO (int input)
        {
			this.input = input;
		}

		public int input;
		public int result;
    }
}
