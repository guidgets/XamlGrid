using Company.DataGrid.Core;

namespace Company.DataGrid.Controllers
{
	public class ItemsSourceChangedCommand : MacroCommand
	{
		protected override void InitializeMacroCommand()
		{
			base.InitializeMacroCommand();

			this.AddSubCommand(typeof(SortingCommand));
			this.AddSubCommand(typeof(CurrentItemCommand));
		}
	}
}
