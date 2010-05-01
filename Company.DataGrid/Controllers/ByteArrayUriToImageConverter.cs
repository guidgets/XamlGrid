using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Converts an array of bytes or an <see cref="Uri"/> to an <see cref="ImageSource"/>, for example to use in an <see cref="Image"/> control. 
	/// </summary>
	public class ByteArrayUriToImageConverter : IValueConverter
	{
		/// <summary>
		/// Modifies the source data before passing it to the target for display in the UI.
		/// </summary>
		/// <param name="value">The source data being passed to the target.</param>
		/// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture of the conversion.</param>
		/// <returns>
		/// The value to be passed to the target dependency property.
		/// </returns>
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			byte[] bytes = value as byte[];
			if (bytes != null)
			{
				using (MemoryStream memoryStream = new MemoryStream(bytes))
				{
					BitmapImage image = new BitmapImage();
					image.SetSource(memoryStream);
					return image;
				}
			}
			return value;
		}

		/// <summary>
		/// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
		/// </summary>
		/// <param name="value">The target data being passed to the source.</param>
		/// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture of the conversion.</param>
		/// <returns>
		/// The value to be passed to the source object.
		/// </returns>
		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
