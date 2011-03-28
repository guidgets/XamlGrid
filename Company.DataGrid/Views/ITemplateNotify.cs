using System;

namespace Company.Widgets.Views
{
	/// <summary>
	/// Provides notifications for changes in templates of objects.
	/// </summary>
	public interface ITemplateNotify
	{
		/// <summary>
		/// Occurs when a template is applied to an object.
		/// </summary>
		event EventHandler TemplateApplied;
	}
}
