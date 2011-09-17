/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;

#endregion

namespace XamlGrid.Core
{
	/// <summary>
	/// The interface definition for a PureMVC Facade
	/// </summary>
	/// <remarks>
	///     <para>The Facade Pattern suggests providing a single class to act as a certal point of communication for subsystems</para>
	///     <para>In PureMVC, the Facade acts as an interface between the core MVC actors (Model, Controller, Controller) and the rest of your application</para>
	/// </remarks>
	/// <see cref="IMainModel"/>
	/// <see cref="INotification"/>
	/// <see cref="XamlGrid.Core"/>
	/// <see cref="XamlGrid.Core"/>
	/// <see cref="XamlGrid.Core"/>
	public interface IFacade : INotifier
	{
		#region Model

		/// <summary>
		/// Register an <c>IModel</c> with the <c>Model</c> by name
		/// </summary>
		/// <param name="model">The <c>IModel</c> to be registered with the <c>Model</c></param>
		void RegisterModel(IModel model);

		/// <summary>
		/// Retrieve a <c>IModel</c> from the <c>Model</c> by name
		/// </summary>
		/// <param name="ModelName">The name of the <c>IModel</c> instance to be retrieved</param>
		/// <returns>The <c>IModel</c> previously regisetered by <c>ModelName</c> with the <c>Model</c></returns>
		IModel RetrieveModel(string ModelName);

		/// <summary>
		/// Remove an <c>IModel</c> instance from the <c>Model</c> by name
		/// </summary>
		/// <param name="ModelName">The <c>IModel</c> to remove from the <c>Model</c></param>
		IModel RemoveModel(string ModelName);

		/// <summary>
		/// Check if a Model is registered
		/// </summary>
		/// <param name="ModelName">The name of the <c>IModel</c> instance to check</param>
		/// <returns>whether a Model is currently registered with the given <c>ModelName</c>.</returns>
		bool HasModel(string ModelName);

		#endregion

		#region Command

		/// <summary>
		/// Register an <c>IMainCommand</c> with the <c>Controller</c>
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotification</c> to associate the <c>IMainCommand</c> with.</param>
		/// <param name="commandType">A reference to the <c>Type</c> of the <c>IMainCommand</c></param>
		void RegisterCommand(int notificationName, Type commandType);

		/// <summary>
		/// Remove a previously registered <c>IMainCommand</c> to <c>INotification</c> mapping from the Controller.
		/// </summary>
		/// <param name="notificationName">TRemove a previously registered <c>IMainCommand</c> to <c>INotification</c> mapping from the Controller.</param>
		void RemoveCommand(int notificationName);

		/// <summary>
		/// Check if a Controller is registered for a given Notification 
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotification</c> to check.</param>
		/// <returns>whether a Controller is currently registered for the given <c>notificationName</c>.</returns>
		bool HasCommand(int notificationName);

		#endregion

		#region Controller

		/// <summary>
		/// Register an <c>IController</c> instance with the <c>Controller</c>
		/// </summary>
		/// <param name="controller">A reference to the <c>IController</c> instance</param>
		void RegisterController(IController controller);

		/// <summary>
		/// Retrieve an <c>IController</c> instance from the <c>Controller</c>
		/// </summary>
		/// <param name="controllerName">The name of the <c>IController</c> instance to retrieve</param>
		/// <returns>The <c>IController</c> previously registered with the given <c>controllerName</c></returns>
		IController RetrieveController(string controllerName);

		/// <summary>
		/// Remove a <c>IController</c> instance from the <c>Controller</c>
		/// </summary>
		/// <param name="controllerName">The name of the <c>IController</c> instance to be removed</param>
		IController RemoveController(string controllerName);

		/// <summary>
		/// Check if a Controller is registered or not
		/// </summary>
		/// <param name="controllerName">The name of the <c>IController</c> instance to check</param>
		/// <returns>whether a Controller is registered with the given <c>controllerName</c>.</returns>
		bool HasController(string controllerName);

		#endregion

		#region Observer

		/// <summary>
		/// Notify the <c>IObservers</c> for a particular <c>INotification</c>.
		/// <para>All previously attached <c>IObservers</c> for this <c>INotification</c>'s list are notified and are passed a reference to the <c>INotification</c> in the order in which they were registered.</para>
		/// <para>NOTE: Use this method only if you are sending custom Notifications. Otherwise use the sendNotification method which does not require you to create the Notification instance.</para>
		/// </summary>
		/// <param name="note">the <c>INotification</c> to notify <c>IObservers</c> of.</param>
		void NotifyObservers(INotification note);

		#endregion
	}
}
