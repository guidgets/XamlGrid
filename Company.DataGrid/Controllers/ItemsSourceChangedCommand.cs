using Company.Widgets.Core;

namespace Company.Widgets.Controllers
{
	public class ItemsSourceChangedCommand : MacroCommand
	{
		protected override void InitializeMacroCommand()
		{
			base.InitializeMacroCommand();

			this.AddSubCommand(typeof(SortingCommand));
			this.AddSubCommand(typeof(CurrentItemCommand));
			this.AddSubCommand(typeof(NewItemCommand));
		}
	}
}
