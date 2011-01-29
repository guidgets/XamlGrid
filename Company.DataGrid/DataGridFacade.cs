using System;
using System.Windows;
using Company.Widgets.Controllers;
using Company.Widgets.Core;
using Company.Widgets.Models;

namespace Company.Widgets
{
	public class DataGridFacade : Facade
	{
		private static DataGridFacade instance;

		public static readonly DependencyProperty ControllersProperty =
			DependencyProperty.RegisterAttached("Controllers", typeof(ControllerCollection), typeof(DataGridFacade), null);


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


		/// <summary>
		/// Gets the collection of controllers for the specified <see cref="DependencyObject"/>.
		/// </summary>
		/// <param name="o">The <see cref="DependencyObject"/> to lookup.</param>
		/// <returns>The collection of associated controllers.</returns>
		public static ControllerCollection GetControllers(DependencyObject o)
		{
			ControllerCollection controllers = (ControllerCollection) o.GetValue(ControllersProperty);
			if (controllers == null)
			{
				controllers = new ControllerCollection(o);
				SetControllers(o, controllers);
			}

			return controllers;
		}

		/// <summary>
		/// Sets the collection of controllers for the specified <see cref="DependencyObject"/>.
		/// </summary>
		/// <param name="o">The <see cref="DependencyObject"/> to set.</param>
		/// <param name="controllers">The collection of controllers to associate.</param>
		public static void SetControllers(DependencyObject o, ControllerCollection controllers)
		{
			o.SetValue(ControllersProperty, controllers);
		}


		protected override void InitializeMainModel()
		{
			base.InitializeMainModel();

			this.RegisterModel(new DataModel());
			this.RegisterModel(new CurrentItemModel());
			this.RegisterModel(new NewItemModel());
		}

		protected override void InitializeMainCommand()
		{
			base.InitializeMainCommand();

			Type typeOfSelectionCommand = typeof(SelectionCommand);

			this.RegisterCommand(Notifications.ITEMS_SOURCE_CHANGED, typeof(ItemsSourceChangedCommand));
			this.RegisterCommand(Notifications.ITEMS_CHANGED, typeOfSelectionCommand);

			this.RegisterCommand(Notifications.DATA_SOURCE_CHANGED, typeof(DataCommand));
			this.RegisterCommand(Notifications.COLUMNS_CHANGED, typeof(DataCommand));

			Type typeOfNewItemCommand = typeof(NewItemCommand);
			this.RegisterCommand(Notifications.ITEM_TYPE_CHANGED, typeOfNewItemCommand);
			this.RegisterCommand(Notifications.NEW_ITEM_ADD, typeOfNewItemCommand);
			this.RegisterCommand(Notifications.NEW_ITEM_COMMIT, typeOfNewItemCommand);

			Type typeOfSortingCommand = typeof(SortingCommand);
			this.RegisterCommand(Notifications.SORTING_STATE, typeOfSortingCommand);
			this.RegisterCommand(Notifications.SORTING_REQUESTED, typeOfSortingCommand);
			this.RegisterCommand(Notifications.ITEM_PROPERTY_CHANGED, typeOfSortingCommand);

			Type typeOfCurrentItemCommand = typeof(CurrentItemCommand);
			this.RegisterCommand(Notifications.CURRENT_ITEM_CHANGING, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.CURRENT_ITEM_UP, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.CURRENT_ITEM_DOWN, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.CURRENT_ITEM_TO_POSITION, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.CURRENT_ITEM_FIRST, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.CURRENT_ITEM_LAST, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.IS_ITEM_CURRENT, typeOfCurrentItemCommand);

			this.RegisterCommand(Notifications.SELECTING_ITEMS, typeOfSelectionCommand);
			this.RegisterCommand(Notifications.SELECT_ALL, typeOfSelectionCommand);
			this.RegisterCommand(Notifications.SELECT_RANGE, typeOfSelectionCommand);
			this.RegisterCommand(Notifications.DESELECTING_ITEMS, typeOfSelectionCommand);
			this.RegisterCommand(Notifications.IS_ITEM_SELECTED, typeOfSelectionCommand);
			this.RegisterCommand(Notifications.SELECTION_MODE_CHANGING, typeOfSelectionCommand);
		}
	}
}
