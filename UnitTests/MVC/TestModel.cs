/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

using Company.Widgets.Core;


namespace UnitTests.MVC
{
	public class TestModel : Model
	{
		public new const string NAME = "MainModelTestModel";
		public const string ON_REGISTER_CALLED = "onRegister Called";
		public const string ON_REMOVE_CALLED = "onRemove Called";

		public TestModel()
			: base(NAME, "")
		{
		}

		public override void OnRegister()
		{
			Data = ON_REGISTER_CALLED;
		}		

		public override void OnRemove()
		{
			Data = ON_REMOVE_CALLED;
		}		
	}
}
