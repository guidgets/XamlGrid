using Company.Widgets.Models;

namespace Company.Widgets.Controllers
{
	public static class DataBinder
	{
		public static object GetValue(object dataItem, string propertyPath)
		{
			return new PropertyPathWalker(propertyPath).GetValue(dataItem);
		}
	}
}
