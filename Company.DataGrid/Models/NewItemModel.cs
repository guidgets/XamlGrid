using System;
using System.Linq;
using System.Reflection;
using Company.Widgets.Core;

namespace Company.Widgets.Models
{
	public class NewItemModel : Model
	{
		public new static readonly string NAME = typeof(NewItemModel).Name;


		public NewItemModel() : base(NAME)
		{
			this.ItemType = typeof(object);
		}

		public Type ItemType
		{
			get; 
			set;
		}

		public object AddItem()
		{
			// TODO: signal the data grid to raise an event for a new item
			object newItem = (from constructor in this.ItemType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
							  where constructor.GetParameters().Length == 0
							  select constructor.Invoke(null)).FirstOrDefault();
			if (newItem == null)
			{
				// TODO: what? Hide the insertion row? Throw an exception that a new element cannot be created?
			}
			else
			{
				this.SendNotification(Notifications.NEW_ITEM_ADDED, newItem);
			}
			return newItem;
		}
	}
}
