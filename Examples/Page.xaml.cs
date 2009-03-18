using System;
using System.ComponentModel;

namespace Examples
{
	public partial class Page
	{
		public Page()
		{
			InitializeComponent();

			this.DataContext = new[] { 
				new Customer { Name = "John", Age = 25, HireDate = new DateTime(2008, 1, 14), IsSingle = false }, 
				new Customer { Name = "Mary", Age = 23, HireDate = new DateTime(2005, 11, 10), IsSingle = true} };
		}
	}

	public class Customer : INotifyPropertyChanged
	{
		private string name;

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
					if (this.PropertyChanged != null)
					{
						this.PropertyChanged(this, new PropertyChangedEventArgs("Name"));
					}					
				}
			}
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

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
