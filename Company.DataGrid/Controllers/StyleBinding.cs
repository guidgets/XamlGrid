using System.Windows;
using System.Windows.Data;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Enables <see cref="System.Windows.Data.Binding"/>s to be used in <see cref="Style"/>s.
	/// </summary>
	public class StyleBinding
	{
		/// <summary>
		/// Gets or sets the binding to apply.
		/// </summary>
		/// <value>The binding to apply.</value>
		public virtual Binding Binding
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets the the name of the dependency Property of the GUI element that the binding is applied to.
		/// </summary>
		public virtual string Property
		{
			get; 
			set;
		}
	}
}
