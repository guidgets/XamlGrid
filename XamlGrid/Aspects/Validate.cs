using System;
using System.Reflection;
using PostSharp.Aspects;

namespace XamlGrid.Aspects
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false)]
	public class Validate : OnMethodBoundaryAspect
	{
		public override void OnEntry(MethodExecutionArgs eventArgs)
		{
			ParameterInfo[] pinfo = eventArgs.Method.GetParameters();
			foreach (ParameterInfo info in pinfo)
			{
				foreach (ArgumentValidationAttribute validator in GetCustomAttributes(info, typeof(ArgumentValidationAttribute)))
				{
					validator.Validate(eventArgs.Arguments[info.Position], info.Name, info.ParameterType);
				}
			}
			base.OnEntry(eventArgs);
		}
	}
}