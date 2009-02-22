namespace Examples
{
	public partial class Page
	{
		public Page()
		{
			InitializeComponent();

			this.dataGrid.ItemsSource = new[] { new Customer { Name = "John", Age = 25 }, new Customer { Name = "Mary", Age = 23 } };
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
	}
}
