using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Company.Widgets.Controllers;

namespace Examples
{
	public partial class Page
	{
		private readonly ICollectionView collectionView;

		public Page()
		{
			InitializeComponent();

			this.dataGrid.Columns[4].DataType = typeof(bool?);

			IEnumerable<Customer> customers = from i in Enumerable.Range(0, 0)
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
			this.dataGrid.DataSource = new List<Customer>(list);
			this.listBoxCustomers.ItemsSource = this.collectionView = new CollectionViewSource { Source = list }.View;

			//this.dataGrid.Columns.CollectionChanged += this.Columns_CollectionChanged;
		}

		private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				Column column = (Column) e.NewItems[0];
				if (column.DataType.IsNumeric())
				{
					column.FooterBinding = new Binding { Converter = new SumAggregate() };					
				}
			}
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			//this.dataGrid.Columns[0].CellStyle = null;
			////this.dataGrid.DataSource = new ObservableCollection<Customer> { new Customer { Age = 25 } };
			//((ObservableCollection<Customer>) this.dataGrid.DataSource).Add(new Customer { Age = 25 });
        	//this.dataGrid.DataSource = new[] { "Ivan1", "Ivan2", "Ivan3", "Ivan4" };
        	//((ObservableCollection<Customer>) this.dataGrid.DataSource).RemoveAt(3);
        	//this.dataGrid.Columns[1].Visibility = this.dataGrid.Columns[1].Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        	//this.dataGrid.ResizableColumns = !this.dataGrid.ResizableColumns;
			//this.dataGrid.FooterVisibility = this.dataGrid.FooterVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
			//this.dataGrid.HeaderVisibility = this.dataGrid.HeaderVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        	//this.dataGrid.ColumnWidth = new GridLength(1, GridUnitType.Auto);
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
		private int age;

		public virtual byte[] OrderPhoto
		{
			get;
			set;
		}

		public virtual Uri OrderThumbnail
		{
			get;
			set;
		}

		public virtual ObservableCollection<Order> Orders
		{
			get;
			set;
		}

		public virtual string Name
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

		public virtual int Age
		{
			get
			{
				return this.age;
			}
			set
			{
				if (this.age != value)
				{
					this.age = value;
					this.OnPropertyChanged("Age");
				}
			}
		}

		public virtual DateTime HireDate
        {
            get;
            set;
        }

		private bool isSingle;

		public virtual bool IsSingle
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

		public virtual bool? MaritalStatus
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

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public virtual event PropertyChangedEventHandler PropertyChanged;
	}

	public class FullName
	{
		public virtual string FirstName { get; set; }

		public virtual string LastName { get; set; }
	}

	public class Order : INotifyPropertyChanged
	{
		public virtual event PropertyChangedEventHandler PropertyChanged;

		private string name;

		public virtual string Name
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

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
