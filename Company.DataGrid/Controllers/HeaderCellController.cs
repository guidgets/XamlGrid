using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Company.Widgets.Core;
using Company.Widgets.Models;
using Company.Widgets.Views;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="HeaderCell"/>.
	/// </summary>
	public class HeaderCellController : Controller
	{
		/// <summary>
		/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="HeaderCell"/>.
		/// </summary>
		/// <param name="headerCell">The header cell for which functionality the <see cref="Controller"/> is responsible.</param>
		public HeaderCellController(HeaderCell headerCell) : base(headerCell.GetHashCode().ToString(), headerCell)
		{

		}


		/// <summary>
		/// Gets the <see cref="HeaderCell"/> for which functionality the <see cref="Controller"/> is responsible.
		/// </summary>
		public virtual HeaderCell HeaderCell
		{
			get
			{
				return (HeaderCell) this.ViewComponent;
			}
		}


		/// <summary>
		/// Called by the Controller when the Controller is registered
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.HeaderCell.SortDirectionChanged += this.HeaderCell_SortDirectionChanged;
		}

		/// <summary>
		/// Called by the Controller when the Controller is removed
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.HeaderCell.SortDirectionChanged -= this.HeaderCell_SortDirectionChanged;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names</returns>
		public override IList<string> ListNotificationInterests()
		{
			return new List<string> { Notifications.SORTED };
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
			SortDescription sortDescription = (SortDescription) notification.Body;
			if (sortDescription.PropertyName != null && sortDescription.PropertyName != this.HeaderCell.Column.Binding.Path.Path)
			{
				return;
			}
			switch (notification.Name)
			{
				case Notifications.SORTED:
					if (notification.Type == NotificationTypes.REMOVED_SORTING)
					{
						this.HeaderCell.SortDirection = null;
					}
					else
					{
						this.HeaderCell.SortDirection = sortDescription.Direction;
					}
					break;
			}
		}


		private void HeaderCell_SortDirectionChanged(object sender, SortDirectionEventArgs e)
		{
			ExtendedSortDescription sortDescription = new ExtendedSortDescription();
			sortDescription.Property = this.HeaderCell.Column.Binding.Path.Path;
			sortDescription.ClearPreviousSorting = (Keyboard.Modifiers & ModifierKeys.Shift) == 0;
			sortDescription.SortDirection = e.SortDirection;
			this.SendNotification(Notifications.SORTING_REQUESTED, sortDescription);
		}
	}
}