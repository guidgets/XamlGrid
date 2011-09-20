using System;

namespace XamlGrid.Aspects
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false)]
	public class Validate : Attribute
	{
	}
}