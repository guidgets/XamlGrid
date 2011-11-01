/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;

#endregion

namespace Guidgets.XamlGrid.Core
{
	/// <summary>
	/// The interface definition for a PureMVC MainCommand
	/// </summary>
	/// <remarks>
	///     <para>In PureMVC, an <c>IMainCommand</c> implementor follows the 'Controller and Controller' strategy, and assumes these responsibilities:</para>
	///     <list type="bullet">
	///         <item>Remembering which <c>IMainCommand</c>s are intended to handle which <c>INotifications</c></item>
	///         <item>Registering itself as an <c>IObserver</c> with the <c>Controller</c> for each <c>INotification</c> that it has an <c>IMainCommand</c> mapping for</item>
	///         <item>Creating a new instance of the proper <c>IMainCommand</c> to handle a given <c>INotification</c> when notified by the <c>Controller</c></item>
	///         <item>Calling the <c>IMainCommand</c>'s <c>execute</c> method, passing in the <c>INotification</c></item>
	///     </list>
	/// </remarks>
	/// <see cref="INotification"/>
	/// <see cref="IMainCommand"/>
	public interface IMainCommand
	{
		/// <summary>
		/// Register a particular <c>IMainCommand</c> class as the handler for a particular <c>INotification</c>
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotification</c></param>
		/// <param name="commandType">The <c>Type</c> of the <c>IMainCommand</c></param>
		void RegisterCommand(int notificationName, Type commandType);

		/// <summary>
		/// Execute the <c>IMainCommand</c> previously registered as the handler for <c>INotification</c>s with the given notification name
		/// </summary>
		/// <param name="notification">The <c>INotification</c> to execute the associated <c>IMainCommand</c> for</param>
		void ExecuteCommand(INotification notification);

		/// <summary>
		/// Remove a previously registered <c>IMainCommand</c> to <c>INotification</c> mapping.
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotification</c> to remove the <c>IMainCommand</c> mapping for</param>
		void RemoveCommand(int notificationName);

		/// <summary>
		/// Check if a Controller is registered for a given Notification.
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotification</c> to check the <c>IMainCommand</c> mapping for</param>
		/// <returns>whether a Controller is currently registered for the given <c>notificationName</c>.</returns>
		bool HasCommand(int notificationName);
	}
}
