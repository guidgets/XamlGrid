using System;
using System.Windows;
using Company.Widgets.Aspects;
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
		[Validate]
		public static ControllerCollection GetControllers([NotNull] DependencyObject o)
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
		[Validate]
		public static void SetControllers([NotNull] DependencyObject o, ControllerCollection controllers)
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

			this.RegisterCommand(Notifications.ItemsSourceChanged, typeof(ItemsSourceChangedCommand));
			this.RegisterCommand(Notifications.ItemsChanged, typeOfSelectionCommand);

			this.RegisterCommand(Notifications.DataSourceChanged, typeof(DataCommand));
			this.RegisterCommand(Notifications.ColumnsChanged, typeof(DataCommand));

			Type typeOfNewItemCommand = typeof(NewItemCommand);
			this.RegisterCommand(Notifications.ItemTypeChanged, typeOfNewItemCommand);
			this.RegisterCommand(Notifications.NewItemAdd, typeOfNewItemCommand);
			this.RegisterCommand(Notifications.NewItemCommit, typeOfNewItemCommand);

			Type typeOfSortingCommand = typeof(SortingCommand);
			this.RegisterCommand(Notifications.SortingState, typeOfSortingCommand);
			this.RegisterCommand(Notifications.SortingRequested, typeOfSortingCommand);
			this.RegisterCommand(Notifications.ItemPropertyChanged, typeOfSortingCommand);

			Type typeOfCurrentItemCommand = typeof(CurrentItemCommand);
			this.RegisterCommand(Notifications.CurrentItemChanging, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.CurrentItemUp, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.CurrentItemDown, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.CurrentItemToPosition, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.CurrentItemFirst, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.CurrentItemLast, typeOfCurrentItemCommand);
			this.RegisterCommand(Notifications.IsItemCurrent, typeOfCurrentItemCommand);

			this.RegisterCommand(Notifications.SelectingItems, typeOfSelectionCommand);
			this.RegisterCommand(Notifications.SelectAll, typeOfSelectionCommand);
			this.RegisterCommand(Notifications.SelectRange, typeOfSelectionCommand);
			this.RegisterCommand(Notifications.DeselectingItems, typeOfSelectionCommand);
			this.RegisterCommand(Notifications.IsItemSelected, typeOfSelectionCommand);
			this.RegisterCommand(Notifications.SelectionModeChanging, typeOfSelectionCommand);
		}
	}
}
