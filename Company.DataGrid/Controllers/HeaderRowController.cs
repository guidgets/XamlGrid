using System.Windows;
using Company.Widgets.Core;
using Company.Widgets.Views;

namespace Company.Widgets.Controllers
{
	public class HeaderRowController : Controller
	{
		public new const string NAME = "HeaderRowController";

		public HeaderRowController(object headerRow) : base(NAME, headerRow)
		{
			this.HeaderRow.Loaded += this.HeaderRow_Loaded;
		}

		private void HeaderRow_Loaded(object sender, RoutedEventArgs e)
		{
			this.SendNotification(Notifications.HEADER_ROW_LOADED, this.HeaderRow);
		}

		public virtual HeaderRow HeaderRow
		{
			get
			{
				return (HeaderRow) this.ViewComponent;
			}
		}
	}
}
