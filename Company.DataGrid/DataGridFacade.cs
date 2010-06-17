using Company.Widgets.Controllers;
using Company.Widgets.Core;
using Company.Widgets.Models;

namespace Company.Widgets
{
	public class DataGridFacade : Facade
	{
		private static DataGridFacade instance;

		private DataGridFacade()
		{
			
		}

		/// <summary>
		/// Facade Singleton Factory method.  This method is thread safe.
		/// </summary>
		public new static DataGridFacade Instance
		{
			get
			{
				if (m_instance == null)
				{
					lock (m_staticSyncRoot)
					{
						if (m_instance == null)
						{
							m_instance = instance = new DataGridFacade();
						}
					}
				}
				return instance;
			}
		}

		protected override void InitializeMainModel()
		{
			base.InitializeMainModel();

			this.RegisterModel(new DataModel());
			this.RegisterModel(new CurrentItemModel());
		}

		protected override void InitializeMainCommand()
		{
			base.InitializeMainCommand();

			this.RegisterCommand(Notifications.ITEMS_SOURCE_CHANGED, typeof(ItemsSourceChangedCommand));
			this.RegisterCommand(Notifications.ITEMS_CHANGED, typeof(SelectionCommand));

			this.RegisterCommand(Notifications.HEADER_ROW_LOADED, typeof(LoadedCommand));

			this.RegisterCommand(Notifications.DATA_WRAPPING_REQUESTED, typeof(DataCommand));
			this.RegisterCommand(Notifications.COLUMNS_CHANGED, typeof(DataCommand));

			this.RegisterCommand(Notifications.SORTING_REQUESTED, typeof(SortingCommand));
			this.RegisterCommand(Notifications.SORTED, typeof(SortingCommand));
			this.RegisterCommand(Notifications.REFRESH_SORTING, typeof(SortingCommand));

			this.RegisterCommand(Notifications.CURRENT_ITEM_CHANGING, typeof(CurrentItemCommand));
			this.RegisterCommand(Notifications.CURRENT_ITEM_UP, typeof(CurrentItemCommand));
			this.RegisterCommand(Notifications.CURRENT_ITEM_DOWN, typeof(CurrentItemCommand));
			this.RegisterCommand(Notifications.CURRENT_ITEM_TO_POSITION, typeof(CurrentItemCommand));
			this.RegisterCommand(Notifications.CURRENT_ITEM_FIRST, typeof(CurrentItemCommand));
			this.RegisterCommand(Notifications.CURRENT_ITEM_LAST, typeof(CurrentItemCommand));

			this.RegisterCommand(Notifications.SELECTING_ITEMS, typeof(SelectionCommand));
			this.RegisterCommand(Notifications.SELECT_ALL, typeof(SelectionCommand));
			this.RegisterCommand(Notifications.SELECT_RANGE, typeof(SelectionCommand));
			this.RegisterCommand(Notifications.DESELECTING_ITEMS, typeof(SelectionCommand));
			this.RegisterCommand(Notifications.IS_ITEM_SELECTED, typeof(SelectionCommand));
			this.RegisterCommand(Notifications.SELECTION_MODE_CHANGING, typeof(SelectionCommand));
		}
	}
}
