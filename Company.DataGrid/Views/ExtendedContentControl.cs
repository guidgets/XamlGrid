using System;
using System.Windows.Controls;

namespace Company.Widgets.Views
{
	/// <summary>
	/// Represents a <see cref="ContentControl"/> which raises an event when its template is applied.
	/// </summary>
	public abstract class ExtendedContentControl : ContentControl, ITemplateNotify
	{
		/// <summary>
		/// Occurs when a template is applied to an object.
		/// </summary>
		public virtual event EventHandler TemplateApplied;


		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			this.OnTemplateApplied();
		}


		private void OnTemplateApplied()
		{
			EventHandler handler = this.TemplateApplied;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
	}
}
