using System.Windows.Controls;
using System.Windows.Data;
using Company.DataGrid.View;
using System.ComponentModel;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
	/// </summary>
	public class Column : INotifyPropertyChanged
	{
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isFrozen;
        private double actualWidth;
        private Binding dataBinding;
		/// <summary>
		/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
		/// </summary>
		public Column()
		{
			// default value
			this.ActualWidth = 200;
            this.IsFrozen = false;
		}

		/// <summary>
		/// Gets or sets the actual width of the <see cref="Column"/>.
		/// </summary>
		/// <value>The actual width of the <see cref="Column"/>.</value>
		public double ActualWidth
		{
            get
            {
                return this.actualWidth;
            }
            set
            {
                this.actualWidth = value;
                this.FirePropertyChangedEvent("ActualWidth");
            }
		}

        [DefaultValue(false)]
		public Binding DataBinding
		{
            get
            {
                return this.dataBinding;
            }
            set
            {
                this.dataBinding = value;
                this.FirePropertyChangedEvent("DataBinding");
            }
		}

        public bool IsFrozen
        {
            get
            {
                return this.isFrozen;
            }
            set
            {
                this.isFrozen = value;
                this.FirePropertyChangedEvent("IsFrozen");
            }
        }

        public void FirePropertyChangedEvent(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
