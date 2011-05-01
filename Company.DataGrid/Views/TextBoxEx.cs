using System.Windows.Controls;
using System.Windows.Input;

namespace Company.Widgets.Views
{
	/// <summary>
	/// Extends the standard <see cref="TextBox"/> 
	/// </summary>
	public class TextBoxEx : TextBox
	{
		/// <summary>
		/// Called when <see cref="System.Windows.UIElement.KeyDown"/> event occurs. Handles it when a new line is sent and the text box accepts new lines.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == Key.Enter && this.AcceptsReturn)
			{
				e.Handled = true;
			}
		}
	}
}
