// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
// 
// File:	EditorController.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Guidgets.XamlGrid.Aspects;
using Guidgets.XamlGrid.Core;
using Guidgets.XamlGrid.Views;

namespace Guidgets.XamlGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller{T}"/> which is responsible for the functionality of a <see cref="Views.Editor"/>.
	/// </summary>
	public class EditorController : Controller<Editor>
	{
		/// <summary>
		/// Represents a <see cref="Controller{T}"/> which is responsible for the functionality of a <see cref="Views.Editor"/>.
		/// </summary>
		/// <param name="editor">The editor for which functionality the <see cref="Controller{T}"/> is responsible.</param>
		public EditorController(Editor editor) : base(editor.GetHashCode().ToString(), editor)
		{

		}


		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.View.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.Editor_KeyDown), true);
			this.View.Unloaded += this.Editor_Unloaded;
		}

		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.View.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.Editor_KeyDown));
			this.View.Unloaded -= this.Editor_Unloaded;
		}

		[Validate]
		public static bool SentFromMultilineTextBox([NotNull] RoutedEventArgs e)
		{
			TextBox textBox = e.OriginalSource as TextBox;
			if (textBox != null && textBox.AcceptsReturn)
			{
				return true;
			}
			RichTextBox richTextBox = e.OriginalSource as RichTextBox;
			return richTextBox != null && richTextBox.AcceptsReturn;
		}


		private void Editor_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Enter:
					if (!SentFromMultilineTextBox(e))
					{
						this.View.Save();
						e.Handled = false;
					}
					break;
				case Key.Escape:
					this.View.Cancel();
					this.SendNotification(Notifications.CellEditingCancelled);
					break;
			}
		}

		private void Editor_Unloaded(object sender, RoutedEventArgs e)
		{
			DataGridFacade.Instance.RemoveController(this.Name);
		}
	}
}
