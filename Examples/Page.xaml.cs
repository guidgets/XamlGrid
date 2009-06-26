using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Examples
{
	public partial class Page
	{
		public Page()
		{
			InitializeComponent();

			this.dataGrid.Columns[3].DataType = typeof(bool?);

            List<object> customers = new List<object>();
            for (int i = 0; i < 10000; i++)
            {
            	customers.Add(new Customer()
            	              	{
            	              		Name = string.Format("Ivan{0}", i),
            	              		Age = i,
            	              		HireDate = DateTime.Now,
            	              		IsSingle = i % 2 == 0,
									MaritalStatus = i % 3 == 0? true: (i % 3 == 1? false: (bool?) null)
            	              	});
            }
            this.dataGrid.ItemsSource = customers;
			this.listBoxCustomers.ItemsSource = customers;
		}

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GC.Collect();
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
					this.OnPropertyChanged("Name");
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

		private bool isSingle;

		public bool IsSingle
		{
			get
			{
				return this.isSingle;
			}
			set
			{
				if (this.isSingle != value)
				{
					this.isSingle = value;
					this.OnPropertyChanged("IsSingle");
				}
			}
		}

		private bool? maritalStatus;

		public bool? MaritalStatus
		{
			get
			{
				return this.maritalStatus;
			}
			set
			{
				if (this.maritalStatus != value)
				{
					this.maritalStatus = value;
					this.OnPropertyChanged("MaritalStatus");
				}
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
