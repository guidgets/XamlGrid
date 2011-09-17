/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

namespace XamlGrid.Core
{
	/// <summary>
	/// The interface definition for a PureMVC Controller
	/// </summary>
	/// <remarks>
	///     <para>In PureMVC, <c>IController</c> implementors assume these responsibilities:</para>
	///     <list type="bullet">
	///         <item>Maintain a cache of <c>IController</c> instances</item>
	///         <item>Provide methods for registering, retrieving, and removing <c>IViews</c></item>
	///         <item>Managing the observer lists for each <c>INotification</c> in the application</item>
	///         <item>Providing a method for attaching <c>IObservers</c> to an <c>INotification</c>'s observer list</item>
	///         <item>Providing a method for broadcasting an <c>INotification</c></item>
	///         <item>Notifying the <c>IObservers</c> of a given <c>INotification</c> when it broadcast</item>
	///     </list>
	/// </remarks>
	/// <see cref="IController"/>
	/// <see cref="IObserver"/>
	/// <see cref="INotification"/>
	public interface IMainController
	{
		#region Observer

		/// <summary>
		/// Register an <c>IObserver</c> to be notified of <c>INotifications</c> with a given name
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotifications</c> to notify this <c>IObserver</c> of</param>
		/// <param name="observer">The <c>IObserver</c> to register</param>
		void RegisterObserver(int notificationName, IObserver observer);

		/// <summary>
		/// Remove a group of observers from the observer list for a given Notification name.
		/// </summary>
		/// <param name="notificationName">which observer list to remove from</param>
		/// <param name="notifyContext">removed the observers with this object as their notifyContext</param>
		void RemoveObserver(int notificationName, object notifyContext);

		/// <summary>
		/// Notify the <c>IObservers</c> for a particular <c>INotification</c>
		/// </summary>
		/// <param name="note">The <c>INotification</c> to notify <c>IObservers</c> of</param>
		/// <remarks>
		///     <para>All previously attached <c>IObservers</c> for this <c>INotification</c>'s list are notified and are passed a reference to the <c>INotification</c> in the order in which they were registered</para>
		/// </remarks>
		void NotifyObservers(INotification note);

		#endregion

		#region Controller

		/// <summary>
		/// Register an <c>IController</c> instance with the <c>Controller</c>
		/// </summary>
		/// <param name="controller">A a reference to the <c>IController</c> instance</param>
		/// <remarks>
		///     <para>Registers the <c>IController</c> so that it can be retrieved by name, and further interrogates the <c>IController</c> for its <c>INotification</c> interests</para>
		///     <para>If the <c>IController</c> returns any <c>INotification</c> names to be notified about, an <c>Observer</c> is created encapsulating  the <c>IController</c> instance's <c>handleNotification</c> method and registering it as an <c>Observer</c> for all <c>INotifications</c> the <c>IController</c> is interested in</para>
		/// </remarks>
		void RegisterController(IController controller);

		/// <summary>
		/// Retrieve an <c>IController</c> from the <c>Controller</c>
		/// </summary>
		/// <param name="ControllerName">The name of the <c>IController</c> instance to retrieve</param>
		/// <returns>The <c>IController</c> instance previously registered with the given <c>ControllerName</c></returns>
		IController RetrieveController(string ControllerName);

		/// <summary>
		/// Remove an <c>IController</c> from the <c>Controller</c>
		/// </summary>
		/// <param name="ControllerName">The name of the <c>IController</c> instance to be removed</param>
		IController RemoveController(string ControllerName);

		/// <summary>
		/// Check if a Controller is registered or not
		/// </summary>
		/// <param name="ControllerName">The name of the <c>IController</c> instance to check for</param>
		/// <returns>whether a Controller is registered with the given <c>ControllerName</c>.</returns>
		bool HasController(string ControllerName);

		#endregion
	}
}
