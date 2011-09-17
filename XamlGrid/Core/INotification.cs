/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using



#endregion

namespace XamlGrid.Core
{
	/// <summary>
	/// The interface definition for a PureMVC Notification
	/// </summary>
	/// <remarks>
	///     <para>PureMVC does not rely upon underlying event models</para>
	///     <para>The Observer Pattern as implemented within PureMVC exists to support event-driven communication between the application and the actors of the MVC triad</para>
	///     <para>Notifications are not meant to be a replacement for Events. Generally, <c>IController</c> implementors place event handlers on their Controller components, which they then handle in the usual way. This may lead to the broadcast of <c>Notification</c>s to trigger <c>IMainCommand</c>s or to communicate with other <c>IViews</c>. <c>IModel</c> and <c>IMainCommand</c> instances communicate with each other and <c>IController</c>s by broadcasting <c>INotification</c>s</para>
	/// </remarks>
	/// <see cref="IObserver"/>
	/// <see cref="IMainController"/>
	public interface INotification
	{
		/// <summary>
		/// The name of the <c>INotification</c> instance
		/// </summary>
		/// <remarks>No setter, should be set by constructor only</remarks>
		int Code { get; }

		/// <summary>
		/// The body of the <c>INotification</c> instance
		/// </summary>
		object Body { get; set; }

		/// <summary>
		/// The type of the <c>INotification</c> instance
		/// </summary>
		string Type { get; set; }

		/// <summary>
		/// Get the string representation of the <c>INotification</c> instance
		/// </summary>
		/// <returns>The string representation of the <c>INotification</c> instance</returns>
		string ToString();
	}
}
