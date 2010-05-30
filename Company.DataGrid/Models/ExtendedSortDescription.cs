using System.ComponentModel;

namespace Company.Widgets.Models
{
	/// <summary>
	/// Contains the property and the direction to use when sorting a collection.
	/// </summary>
	public struct ExtendedSortDescription
	{
		/// <summary>
		/// Gets or sets the property by which to sort.
		/// </summary>
		/// <value>The property by which to sort.</value>
		public string Property
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the direction in which to sort.
		/// </summary>
		/// <value>The direction in which to sort.</value>
		public ListSortDirection? SortDirection
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether all previous sorting has to be cleared.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if all previous sorting has to be cleared; otherwise, <c>false</c>.
		/// </value>
		public bool ClearPreviousSorting
		{
			get;
			set;
		}
	}
}
