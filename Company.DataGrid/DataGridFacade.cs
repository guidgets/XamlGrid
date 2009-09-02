using Company.DataGrid.Controllers;
using Company.DataGrid.Core;
using Company.DataGrid.Models;

namespace Company.DataGrid
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
		}

		protected override void InitializeMainCommand()
		{
			base.InitializeMainCommand();

			this.RegisterCommand(Notifications.DATA_WRAPPING_REQUESTED, typeof(DataCommand));
			this.RegisterCommand(Notifications.COLUMNS_CHANGED, typeof(DataCommand));
			this.RegisterCommand(Notifications.SORTING_REQUESTED, typeof(SortingCommand));
			this.RegisterCommand(Notifications.SORTED, typeof(SortingCommand));
			this.RegisterCommand(Notifications.REFRESH_SORTING, typeof(SortingCommand));
			this.RegisterCommand(Notifications.ITEMS_SOURCE_CHANGED, typeof(SortingCommand));
			this.RegisterCommand(Notifications.HEADER_ROW_LOADED, typeof(LoadedCommand));
		}
	}
}
