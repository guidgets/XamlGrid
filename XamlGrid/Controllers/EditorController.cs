using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XamlGrid.Aspects;
using XamlGrid.Core;
using XamlGrid.Views;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="Views.Editor"/>.
	/// </summary>
	public class EditorController : Controller
	{
		/// <summary>
		/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="Views.Editor"/>.
		/// </summary>
		/// <param name="editor">The editor for which functionality the <see cref="Controller"/> is responsible.</param>
		public EditorController(Editor editor) : base(editor.GetHashCode().ToString(), editor)
		{

		}

		/// <summary>
		/// Gets the editor for which functionality the <see cref="EditorController"/> is responsible.
		/// </summary>
		public virtual Editor Editor
		{
			get
			{
				return (Editor) this.ViewComponent;
			}
		}


		/// <summary>
		/// Called by the <see cref="Controller"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.Editor.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.Editor_KeyDown), true);
			this.Editor.Unloaded += this.Editor_Unloaded;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.Editor.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.Editor_KeyDown));
			this.Editor.Unloaded -= this.Editor_Unloaded;
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
						this.Editor.Save();
						e.Handled = false;
					}
					break;
				case Key.Escape:
					this.Editor.Cancel();
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
