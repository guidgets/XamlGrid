using Company.Widgets.Models;

namespace Company.Widgets.Controllers
{
	public static class DataBinder
	{
		public static object GetValue(object dataItem, string propertyPath)
		{
			PropertyPathWalker propertyPathWalker = new PropertyPathWalker(propertyPath);
			propertyPathWalker.Update(dataItem);
			return propertyPathWalker.FinalNode.Value;
		}
	}
}
