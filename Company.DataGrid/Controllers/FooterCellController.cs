using System.Collections.Generic;
using System.Collections.Specialized;
using Company.Widgets.Core;
using Company.Widgets.Views;

namespace Company.Widgets.Controllers
{
	public class FooterCellController : Controller
	{
		public const string FOOTER_PREFIX = "footer";


		public FooterCellController(object viewComponent) : base(FOOTER_PREFIX + viewComponent.GetHashCode(), viewComponent)
		{

		}

		public virtual Cell FooterCell
		{
			get
			{
				return (Cell) this.ViewComponent;
			}
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			((INotifyCollectionChanged) this.FooterCell.DataContext).CollectionChanged += this.FooterCellController_CollectionChanged;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			((INotifyCollectionChanged) this.FooterCell.DataContext).CollectionChanged -= this.FooterCellController_CollectionChanged;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names.</returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int> { Notifications.ITEM_PROPERTY_CHANGED };
		}

		/// <summary>
		/// Handle <c>INotification</c>s.
		/// </summary>
		/// <param name="notification">The <c>INotification</c> instance to handle</param>
		/// <remarks>
		/// Typically this will be handled in a switch statement, with one 'case' entry per <c>INotification</c> the <c>Controller</c> is interested in.
		/// </remarks>
		public override void HandleNotification(INotification notification)
		{
			switch (notification.Code)
			{
				case Notifications.ITEM_PROPERTY_CHANGED:
					this.Update();
					break;
			}
		}

		private void FooterCellController_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.Update();
		}

		private void Update()
		{
			this.FooterCell.ClearValue(Cell.ValueProperty);
			if (this.FooterCell.Column.FooterBinding != null)
			{
				this.FooterCell.SetBinding(Cell.ValueProperty, this.FooterCell.Column.FooterBinding);
			}
		}
	}
}
