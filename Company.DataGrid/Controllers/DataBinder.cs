using Company.Widgets.Aspects;
using Company.Widgets.Models;

namespace Company.Widgets.Controllers
{
	public static class DataBinder
	{
		[Validate]
		public static object GetValue(object dataItem, [NotNull] string propertyPath)
		{
			return new PropertyPathWalker(propertyPath).GetValue(dataItem);
		}
	}
}
