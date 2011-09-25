using XamlGrid.Aspects;
using XamlGrid.Models;

namespace XamlGrid.Controllers
{
	public static class DataBinder
	{
		[Validate]
		public static object GetValue(object dataItem, [NotNull] string propertyPath)
		{
			using (PropertyPathWalker propertyPathWalker = new PropertyPathWalker(propertyPath))
			{
				return propertyPathWalker.GetValue(dataItem);
			}
		}
	}
}
