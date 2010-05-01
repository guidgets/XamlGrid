using System;

namespace Company.DataGrid.Models
{
	/// <summary>
	/// Provides data for the <see cref="ObservableItemCollection{T}.ItemPropertyChanged"/> event.
	/// </summary>
	public class ItemPropertyChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Provides data for the <see cref="ObservableItemCollection{T}.ItemPropertyChanged"/> event.
		/// </summary>
		/// <param name="item">The item which had a property changed.</param>
		/// <param name="propertyPath">The property path in which the name of the changed property is located.</param>
		/// <param name="propertyName">The name of the property which value changed.</param>
		public ItemPropertyChangedEventArgs(object item, string propertyPath, string propertyName)
		{
			this.Item = item;
			this.PropertyPath = propertyPath;
			this.PropertyName = propertyName;
		}

		/// <summary>
		/// Gets the item which had a property changed.
		/// </summary>
		public virtual object Item
		{
			get; 
			private set;
		}

		/// <summary>
		/// Gets the property path in which the name of the changed property is located.
		/// </summary>
		/// <value>The property path.</value>
		public virtual string PropertyPath
		{
			get; 
			private set;
		}

		/// <summary>
		/// Gets the name of the property which value changed.
		/// </summary>
		/// <value>The name of the property which value changed.</value>
		public virtual string PropertyName
		{
			get; 
			private set;
		}
	}
}
