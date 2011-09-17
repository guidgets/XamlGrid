namespace XamlGrid.Models
{
	/// <summary>
	/// Provides values for different types of sizing.
	/// </summary>
	public enum SizeMode
	{
		/// <summary>
		/// The size is an absolute value.
		/// </summary>
		Absolute,
		/// <summary>
		/// The size fills all available space.
		/// </summary>
		Fill,
		/// <summary>
		/// The size is automatically calculated.
		/// </summary>
		Auto,
		/// <summary>
		/// The size is automatically calculated according to a header element.
		/// </summary>
		ToHeader,
		/// <summary>
		/// The size is automatically calculated according to the displayed data.
		/// </summary>
		ToData
	}
}
