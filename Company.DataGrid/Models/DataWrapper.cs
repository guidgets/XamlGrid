using System.Collections;
using System.ComponentModel;
using System.Windows.Data;

namespace Company.Widgets.Models
{
	/// <summary>
	/// Contains functionality to wrap a data source with the purpose of enabling it with advanced functionality.
	/// </summary>
	public static class DataWrapper
	{
		/// <summary>
		/// Wraps the specified source to expose it for manipulation.
		/// </summary>
		/// <param name="source">The source to wrap.</param>
		/// <returns></returns>
		public static ICollectionView Wrap(IEnumerable source)
		{
			return source as ICollectionView ?? new PagedCollectionView(source);
		}
	}
}
