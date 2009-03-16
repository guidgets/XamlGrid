using System;

namespace Examples
{
	public partial class Page
	{
		public Page()
		{
			InitializeComponent();

			this.dataGrid.ItemsSource = new[] { 
				new Customer { Name = "John", Age = 25, HireDate = new DateTime(2008, 1, 14), IsSingle = false }, 
				new Customer { Name = "Mary", Age = 23, HireDate = new DateTime(2005, 11, 10), IsSingle = true} };
		}
	}

	public class Customer
	{
		public string Name
		{
			get;
			set;
		}

		public int Age
		{
			get;
			set;
		}

		public DateTime HireDate
		{
			get;
			set;
		}

		public bool IsSingle
		{
			get;
			set;
		}
	}
}
