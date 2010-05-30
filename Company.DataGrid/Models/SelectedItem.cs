namespace Company.Widgets.Models
{
	/// <summary>
	/// Represents a selected from a list item.
	/// </summary>
	public struct SelectedItem
	{
		/// <summary>
		/// Represents a selected from a list item.
		/// </summary>
		/// <param name="item">The selected item.</param>
		/// <param name="index">The index of the item in its list.</param>
		public SelectedItem(object item, int index) : this()
		{
			this.Item = item;
			this.Index = index;
		}

		/// <summary>
		/// Gets or sets the selected item.
		/// </summary>
		public object Item { get; private set; }

		/// <summary>
		/// Gets or sets the index of the item in its list.
		/// </summary>
		public int Index { get; private set; }
	}
}
