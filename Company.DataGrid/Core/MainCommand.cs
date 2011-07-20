/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using System.Collections.Generic;

#endregion

namespace Company.Widgets.Core
{
	/// <summary>
	/// A Singleton <c>IMainCommand</c> implementation.
	/// </summary>
	/// <remarks>
	/// 	<para>In PureMVC, the <c>MainCommand</c> class follows the 'Controller and Controller' strategy, and assumes these responsibilities:</para>
	/// 	<list type="bullet">
	/// 		<item>Remembering which <c>IMainCommand</c>s are intended to handle which <c>INotifications</c>.</item>
	/// 		<item>Registering itself as an <c>IObserver</c> with the <c>Controller</c> for each <c>INotification</c> that it has an <c>IMainCommand</c> mapping for.</item>
	/// 		<item>Creating a new instance of the proper <c>IMainCommand</c> to handle a given <c>INotification</c> when notified by the <c>Controller</c>.</item>
	/// 		<item>Calling the <c>IMainCommand</c>'s <c>execute</c> method, passing in the <c>INotification</c>.</item>
	/// 	</list>
	/// 	<para>Your application must register <c>IMainCommands</c> with the <c>Controller</c>.</para>
	/// 	<para>The simplest way is to subclass <c>Facade</c>, and use its <c>initializeController</c> method to add your registrations.</para>
	/// </remarks>
	/// <see cref="MainController"/>
	/// <see cref="Observer"/>
	/// <see cref="Notification"/>
	/// <see cref="SimpleCommand"/>
	/// <see cref="MacroCommand"/>
	public class MainCommand : IMainCommand
	{
		#region Constructors

		/// <summary>
		/// Constructs and initializes a new main command
		/// </summary>
		/// <remarks>
		///     <para>
		///         This <c>IMainCommand</c> implementation is a Singleton, 
		///         so you should not call the constructor 
		///         directly, but instead call the static Singleton
		///         Factory method <c>Controller.getInstance()</c>
		///     </para>
		/// </remarks>
		protected MainCommand()
		{
			this.commandMap = new Dictionary<int, Type>();
			this.InitializeController();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// If an <c>IMainCommand</c> has previously been registered
		/// to handle a the given <c>INotification</c>, then it is executed.
		/// </summary>
		/// <param name="note">An <c>INotification</c></param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void ExecuteCommand(INotification note)
		{
			Type commandType;

			lock (this.m_syncRoot)
			{
				if (!this.commandMap.ContainsKey(note.Code)) return;
				commandType = this.commandMap[note.Code];
			}

			object commandInstance = Activator.CreateInstance(commandType);

			ICommand command = commandInstance as ICommand;
			if (command != null)
			{
				command.Execute(note);
			}
		}

		/// <summary>
		/// Register a particular <c>IMainCommand</c> class as the handler
		/// for a particular <c>INotification</c>.
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotification</c></param>
		/// <param name="commandType">The <c>Type</c> of the <c>IMainCommand</c></param>
		/// <remarks>
		///     <para>
		///         If an <c>IMainCommand</c> has already been registered to 
		///         handle <c>INotification</c>s with this name, it is no longer
		///         used, the new <c>IMainCommand</c> is used instead.
		///     </para>
		/// </remarks> 
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RegisterCommand(int notificationName, Type commandType)
		{
			lock (this.m_syncRoot)
			{
				if (!this.commandMap.ContainsKey(notificationName))
				{
					// This call needs to be monitored carefully. Have to make sure that RegisterObserver
					// doesn't call back into the controller, or a dead lock could happen.
					this.mMainController.RegisterObserver(notificationName, new Observer(this.ExecuteCommand, this));
				}

				this.commandMap[notificationName] = commandType;
			}
		}

		/// <summary>
		/// Check if a Controller is registered for a given Notification 
		/// </summary>
		/// <param name="notificationName"></param>
		/// <returns>whether a Controller is currently registered for the given <c>notificationName</c>.</returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual bool HasCommand(int notificationName)
		{
			lock (this.m_syncRoot)
			{
				return this.commandMap.ContainsKey(notificationName);
			}
		}

		/// <summary>
		/// Remove a previously registered <c>IMainCommand</c> to <c>INotification</c> mapping.
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotification</c> to remove the <c>IMainCommand</c> mapping for</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RemoveCommand(int notificationName)
		{
			lock (this.m_syncRoot)
			{
				if (this.commandMap.ContainsKey(notificationName))
				{
					// remove the observer

					// This call needs to be monitored carefully. Have to make sure that RemoveObserver
					// doesn't call back into the controller, or a dead lock could happen.
					this.mMainController.RemoveObserver(notificationName, this);
					this.commandMap.Remove(notificationName);
				}
			}
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Singleton Factory method.  This method is thread safe.
		/// </summary>
		public static IMainCommand Instance
		{
			get
			{
				if (m_instance == null)
				{
					lock (m_staticSyncRoot)
					{
						if (m_instance == null) m_instance = new MainCommand();
					}
				}

				return m_instance;
			}
		}

		#endregion

		#region Protected & Internal Methods

		/// <summary>
		/// Explicit static constructor to tell C# compiler
		/// not to mark type as beforefieldinit
		/// </summary>
		static MainCommand()
		{
		}

		/// <summary>
		/// Initialize the Singleton <c>Controller</c> instance
		/// </summary>
		/// <remarks>
		///     <para>Called automatically by the constructor</para>
		///     
		///     <para>
		///         Note that if you are using a subclass of <c>Controller</c>
		///         in your application, you should also subclass <c>Controller</c>
		///         and override the <c>initializeController</c> method in the following way:
		///     </para>
		/// 
		///     <c>
		///         // ensure that the Controller is talking to my IController implementation
		///         public override void initializeController()
		///         {
		///             Controller = MyController.Instance;
		///         }
		///     </c>
		/// </remarks>
		protected virtual void InitializeController()
		{
			this.mMainController = MainController.Instance;
		}

		#endregion

		#region Members

		/// <summary>
		/// Used for locking the instance calls
		/// </summary>
		protected static readonly object m_staticSyncRoot = new object();

		/// <summary>
		/// Singleton instance, can be sublcassed though....
		/// </summary>
		protected static volatile IMainCommand m_instance;

		/// <summary>
		/// Used for locking
		/// </summary>
		protected readonly object m_syncRoot = new object();

		/// <summary>
		/// Mapping of Notification names to Controller Class references
		/// </summary>
		protected IDictionary<int, Type> commandMap;

		/// <summary>
		/// Local reference to Controller
		/// </summary>
		protected IMainController mMainController;

		#endregion
	}
}
