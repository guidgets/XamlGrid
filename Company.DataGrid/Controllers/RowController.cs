using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using Company.Widgets.Core;
using Company.Widgets.Views;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="Views.Row"/>.
	/// </summary>
	public class RowController : Controller
	{
		/// <summary>
		/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="Views.Row"/>.
		/// </summary>
		/// <param name="row">The row for which functionality the <see cref="Controller"/> is responsible.</param>
		public RowController(Row row) : base(row.GetHashCode().ToString(), row)
		{

		}


		/// <summary>
		/// Gets the row for which functionality the <see cref="RowController"/> is responsible.
		/// </summary>
		public virtual Row Row
		{
			get
			{
				return (Row) this.ViewComponent;
			}
		}


		/// <summary>
		/// Called by the <see cref="Controller"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();
			this.Row.DataContextChanged += this.Row_DataContextChanged;
			this.Row.HasFocusChanged += this.Row_HasFocusedChanged;
			this.Row.IsSelectedChanged += this.Row_IsSelectedChanged;
			this.Row.KeyDown += this.Row_KeyDown;
			this.Row.MouseLeftButtonUp += this.Row_MouseLeftButtonUp;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();
			this.Row.DataContextChanged -= this.Row_DataContextChanged;
			this.Row.HasFocusChanged -= this.Row_HasFocusedChanged;
			this.Row.IsSelectedChanged -= this.Row_IsSelectedChanged;
			this.Row.KeyDown -= this.Row_KeyDown;
			this.Row.MouseLeftButtonUp -= this.Row_MouseLeftButtonUp;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names</returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int>
			       	{
			       		Notifications.CurrentItemChanged,
			       		Notifications.SelectedItems,
			       		Notifications.DeselectedItems,
			       		Notifications.ItemIsSelected,
			       		Notifications.ItemsCollectionChanged
			       	};
		}

		/// <summary>
		/// Handle <c>INotification</c>s
		/// </summary>
		/// <param name="notification">The <c>INotification</c> instance to handle</param>
		/// <remarks>
		/// Typically this will be handled in a switch statement, with one 'case' entry per <c>INotification</c> the <c>Controller</c> is interested in.
		/// </remarks>
		public override void HandleNotification(INotification notification)
		{
			switch (notification.Code)
			{
				case Notifications.CurrentItemChanged:
					this.Row.IsFocused = this.Row.DataContext == notification.Body;
					break;
				case Notifications.SelectedItems:
					if (((IList<object>) notification.Body).Contains(this.Row.DataContext))
					{
						this.Row.IsSelected = true;
					}
					break;
				case Notifications.DeselectedItems:
					IList<object> list = (IList<object>) notification.Body;
					if (list.Contains(this.Row.DataContext) || list.Count == 0)
					{
						this.Row.IsSelected = false;
					}
					break;
				case Notifications.ItemIsSelected:
					if (this.Row.DataContext == notification.Body)
					{
						this.Row.IsSelected = bool.Parse(notification.Type);
					}
					break;
				case Notifications.ItemsCollectionChanged:
					NotifyCollectionChangedEventArgs args = (NotifyCollectionChangedEventArgs) notification.Body;
					switch (args.Action)
					{
						case NotifyCollectionChangedAction.Remove:
						case NotifyCollectionChangedAction.Replace:
						case NotifyCollectionChangedAction.Reset:
							if (args.Action == NotifyCollectionChangedAction.Reset || args.OldItems.Contains(this.Row.DataContext))
							{
								DataGridFacade.Instance.RemoveController(this.Name);
								this.Row.ItemsSource = null;
							}
							break;
					}
					break;
			}
		}

		private void Row_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.IsItemCurrent, this.Row.DataContext);
			this.SendNotification(Notifications.IsItemSelected, this.Row.DataContext);
		}

		private void Row_HasFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.Row.IsFocused)
			{
				this.SendNotification(Notifications.CurrentItemChanging, this.Row.DataContext);
			}
		}

		private void Row_IsSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(this.Row.IsSelected ? Notifications.SelectingItems : Notifications.DeselectingItems,
			                      this.Row.DataContext);
		}

		private void Row_KeyDown(object sender, KeyEventArgs e)
		{
			this.SendNotification(Notifications.ItemKeyDown, e);
		}

		private void Row_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.SendNotification(Notifications.ItemClicked, this.Row.DataContext);
		}
	}
}
