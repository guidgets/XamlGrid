using XamlGrid.Aspects;
using XamlGrid.Models;

namespace XamlGrid.Controllers
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
