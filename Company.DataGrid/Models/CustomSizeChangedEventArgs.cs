using System;
using System.Windows;

namespace Company.Widgets.Models
{
	public class CustomSizeChangedEventArgs : EventArgs
	{
		public CustomSizeChangedEventArgs(Size oldSize, Size newSize)
		{
			this.OldSize = oldSize;
			this.NewSize = newSize;
		}

		public Size OldSize 
		{
			get; 
			set;
		}

		public Size NewSize
		{
			get; 
			set;
		}
	}
}
