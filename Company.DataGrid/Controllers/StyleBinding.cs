using System.Windows.Data;

namespace Company.DataGrid.Controllers
{
	public class StyleBinding
	{
		public Binding Binding { get; set; }
		/// <summary>
		/// The Dependency Property of the ui element that the binding is applied to
		/// </summary>
		public string Property { get; set; }
	}
}