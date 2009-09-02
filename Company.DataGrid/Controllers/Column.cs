using System;
using System.Windows;
using System.Windows.Data;
using Company.DataGrid.Views;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
	/// </summary>
	public class Column
	{
		/// <summary>
		/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
		/// </summary>
		public Column()
		{
			// default value
            this.ActualWidth = 200;
			this.DataType = typeof(object);
		}

		/// <summary>
		/// Gets or sets the actual width of the cells in this <see cref="Column"/>.
		/// </summary>
		/// <value>The actual width of the cells in this <see cref="Column"/>.</value>
        public double ActualWidth
        {
        	get; 
			set;
        }

		/// <summary>
		/// Gets or sets the binding which the <see cref="Cell"/>s in this <see cref="Column"/> use to get the data they display.
		/// </summary>
		/// <value>The binding which the <see cref="Cell"/>s in this <see cref="Column"/> use to get the data they display.</value>
		public Binding Binding
		{
        	get; 
			set;
		}

		/// <summary>
		/// Gets or sets the type of the data in the <see cref="Cell"/>s in this <see cref="Column"/>.
		/// </summary>
		/// <value>The type of the data in the <see cref="Cell"/>s in this <see cref="Column"/>.</value>
		public Type DataType
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets the style of the <see cref="Cell"/>s in this <see cref="Column"/>.
		/// </summary>
		/// <value>The style of the <see cref="Cell"/>s in this <see cref="Column"/>.</value>
		public Style CellStyle
		{
			get; 
			set;
		}
    }
}
