using System;

namespace Company.Widgets.Models
{
	/// <summary>
	/// Contains event data about events raised when an attempt is made to create a new item to be added to a list.
	/// </summary>
	public class NewItemEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the new item to add.
		/// </summary>
		/// <value>The new item to add.</value>
		public object NewItem
		{
			get; 
			set;
		}
	}
}
