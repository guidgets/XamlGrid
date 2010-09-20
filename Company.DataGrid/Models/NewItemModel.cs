using System;
using Company.Widgets.Core;

namespace Company.Widgets.Models
{
	public class NewItemModel : Model
	{
		public static readonly string NAME = typeof(NewItemModel).Name;


		public NewItemModel() : base(NAME)
		{
		}

		public Type ItemType
		{
			get; 
			set;
		}
	}
}
