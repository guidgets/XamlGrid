using System.Windows;
using System.Windows.Input;
using Company.DataGrid.Core;
using Company.DataGrid.Views;

namespace Company.DataGrid.Controllers
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
		public Editor Editor
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

			this.Editor.KeyUp += this.Editor_KeyUp;
			this.Editor.Unloaded += this.Editor_Unloaded;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.Editor.KeyUp -= this.Editor_KeyUp;
			this.Editor.Unloaded -= this.Editor_Unloaded;
		}


		private void Editor_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Enter:
					this.Editor.Save();
					break;
				case Key.Escape:
					this.Editor.Cancel();
					this.SendNotification(Notifications.CELL_EDITING_CANCELLED);
					break;
			}
		}

		private void Editor_Unloaded(object sender, RoutedEventArgs e)
		{
			DataGridFacade.Instance.RemoveController(this.Name);
		}
	}
}
