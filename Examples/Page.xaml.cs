using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Examples
{
	public partial class Page
	{
		private readonly ICollectionView collectionView;

		public Page()
		{
			InitializeComponent();

			this.dataGrid.Columns[4].DataType = typeof(bool?);

			IEnumerable<Customer> customers = from i in Enumerable.Range(0, 100)
			                                  select new Customer
			                                         	{
			                                         		Name = string.Format("Ivan{0}", i),
			                                         		Age = i,
			                                         		HireDate = DateTime.Now,
			                                         		IsSingle = i % 2 == 0,
			                                         		MaritalStatus = i % 3 == 0 ? true : (i % 3 == 1 ? false : (bool?) null),
			                                         		Orders = new ObservableCollection<Order> { new Order { Name = "Constructor Lego Number " + Math.Pow(2, i) } }
			                                         	};
			List<Customer> list = customers.ToList();
			int index = 0;
			foreach (Customer customer in list)
			{
				customer.OrderPhoto = SamplePictures.PNGByteArrays[index];
				customer.OrderThumbnail = new Uri(string.Format("/Images/p{0}.png", index), UriKind.Relative);
				++index;
			}
			this.dataGrid.DataSource = list;
			this.listBoxCustomers.ItemsSource = this.collectionView = new CollectionViewSource { Source = list }.View;
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			this.dataGrid.ColumnWidth = new GridLength(1, GridUnitType.Auto);
			//this.dataGrid.Columns.Clear();
			//this.dataGrid.DataSource = null;
			//this.dataGrid.Columns.RemoveAt(0);
			//((Customer) this.dataGrid.Items[0]).Orders[0] = new Order { Name = "Lego: Pirate Ship" };
			//this.dataGrid.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
			//GC.Collect();
			//ObservableCollection<object> objects = (ObservableCollection<object>) this.dataGrid.DataSource;
			//Customer customer = new Customer
			//                    {
			//                        Name = string.Format("Ivan{0}", 100),
			//                        Age = 100,
			//                        HireDate = DateTime.Now,
			//                        IsSingle = 100 % 2 == 0,
			//                        MaritalStatus = (false),
			//                        Orders = new ObservableCollection<Order> { new Order { Name = "Lego Number " + 100 } }
			//                    };
			//objects.Insert(2, customer);
			//objects[5] = customer;
			//this.dataGrid.SortDescriptions.Clear();
			//this.dataGrid.Columns.RemoveAt(2);
			//if (this.dataGrid.ItemsSource == null)
			//{
			//    IEnumerable<Customer> customers = from i in Enumerable.Range(0, 10000)
			//                                      select new Customer
			//                                      {
			//                                          Name = string.Format("Ivan{0}", i),
			//                                          Age = i,
			//                                          HireDate = DateTime.Now,
			//                                          IsSingle = i % 2 == 0,
			//                                          MaritalStatus = i % 3 == 0 ? true : (i % 3 == 1 ? false : (bool?) null),
			//                                          Orders = new ObservableCollection<Order> { new Order { Name = "Lego Number " + i } }
			//                                      };
			//    this.dataGrid.DataSource = customers.ToList();
			//}
			//else
			//{
			//    this.dataGrid.ItemsSource = null;
			//}
        }

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			if (this.collectionView.SortDescriptions.Count == 0)
			{
				this.collectionView.SortDescriptions.Add(new SortDescription("Orders[0].Name", ListSortDirection.Ascending));				
			}
			else
			{
				this.collectionView.SortDescriptions[0] = new SortDescription("Orders[0].Name", ListSortDirection.Ascending);
			}
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			if (this.collectionView.SortDescriptions.Count == 0)
			{
				this.collectionView.SortDescriptions.Add(new SortDescription("Orders[0].Name", ListSortDirection.Descending));
			}
			else
			{
				this.collectionView.SortDescriptions[0] = new SortDescription("Orders[0].Name", ListSortDirection.Descending);
			}
		}
	}

	public class Customer : INotifyPropertyChanged
	{
		private string name;

		public byte[] OrderPhoto
		{
			get;
			set;
		}

		public Uri OrderThumbnail
		{
			get;
			set;
		}

		public ObservableCollection<Order> Orders
		{
			get;
			set;
		}

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

	public class FullName
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }
	}

	public class Order : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

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

		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
