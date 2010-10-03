using Company.Widgets.Controllers;

namespace Company.Widgets.Views
{
	public class NewRow : Row
	{
		public NewRow()
		{
			DataGridFacade.Instance.RemoveController(this.GetHashCode().ToString());
			DataGridFacade.Instance.RegisterController(new NewRowController(this));
		}
	}
}
