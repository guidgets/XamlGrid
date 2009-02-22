using System.Windows.Controls;
using System.Windows.Data;
using Company.DataGrid.View;

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
			this.ActualWidth = 50;
		}

		/// <summary>
		/// Gets or sets the actual width of the <see cref="Column"/>.
		/// </summary>
		/// <value>The actual width of the <see cref="Column"/>.</value>
		public double ActualWidth
		{
			get;
			set;
		}

		public Binding DataBinding
		{
			get;
			set;
		}
	}
}
