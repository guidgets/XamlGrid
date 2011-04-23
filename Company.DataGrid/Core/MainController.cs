/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System.Collections.Generic;

#endregion

namespace Company.Widgets.Core
{
	/// <summary>
	/// A Singleton <c>IController</c> implementation.
	/// </summary>
	/// <remarks>
	///     <para>In PureMVC, the <c>Controller</c> class assumes these responsibilities:</para>
	///     <list type="bullet">
	///         <item>Maintain a cache of <c>IController</c> instances</item>
	///         <item>Provide methods for registering, retrieving, and removing <c>IControllers</c></item>
	///         <item>Managing the observer lists for each <c>INotification</c> in the application</item>
	///         <item>Providing a method for attaching <c>IObservers</c> to an <c>INotification</c>'s observer list</item>
	///         <item>Providing a method for broadcasting an <c>INotification</c></item>
	///         <item>Notifying the <c>IObservers</c> of a given <c>INotification</c> when it broadcast</item>
	///     </list>
	/// </remarks>
	/// <see cref="Controller"/>
	/// <see cref="Notification"/>
	/// <see cref="Observer"/>
	public class MainController : IMainController
	{
		#region Constructors

		/// <summary>
		/// Constructs and initializes a new Controller
		/// </summary>
		/// <remarks>
		/// <para>This <c>IController</c> implementation is a Singleton, so you should not call the constructor directly, but instead call the static Singleton Factory method <c>Controller.Instance</c></para>
		/// </remarks>
		protected MainController()
		{
			this.m_ControllerMap = new Dictionary<string, IController>();
			this.m_observerMap = new Dictionary<int, IList<IObserver>>();
			this.InitializeController();
		}

		#endregion

		#region Public Methods

		#region Observer

		/// <summary>
		/// Register an <c>IObserver</c> to be notified of <c>INotifications</c> with a given name
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotifications</c> to notify this <c>IObserver</c> of</param>
		/// <param name="observer">The <c>IObserver</c> to register</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RegisterObserver(int notificationName, IObserver observer)
		{
			if (!this.m_observerMap.ContainsKey(notificationName))
			{
				this.m_observerMap[notificationName] = new List<IObserver>();
			}

			this.m_observerMap[notificationName].Add(observer);
		}

		/// <summary>
		/// Notify the <c>IObservers</c> for a particular <c>INotification</c>
		/// </summary>
		/// <param name="notification">The <c>INotification</c> to notify <c>IObservers</c> of</param>
		/// <remarks>
		/// <para>All previously attached <c>IObservers</c> for this <c>INotification</c>'s list are notified and are passed a reference to the <c>INotification</c> in the order in which they were registered</para>
		/// </remarks>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void NotifyObservers(INotification notification)
		{
			IList<IObserver> observers = null;

			if (this.m_observerMap.ContainsKey(notification.Code))
			{
				// Get a reference to the observers list for this notification name
				IList<IObserver> observers_ref = this.m_observerMap[notification.Code];
				// Copy observers from reference array to working array, 
				// since the reference array may change during the notification loop
				observers = new List<IObserver>(observers_ref);
			}

			// Notify outside of the lock
			if (observers != null)
			{
				// Notify Observers from the working array				
				for (int i = 0; i < observers.Count; i++)
				{
					IObserver observer = observers[i];
					observer.NotifyObserver(notification);
				}
			}
		}

		/// <summary>
		/// Remove the observer for a given notifyContext from an observer list for a given Notification name.
		/// </summary>
		/// <param name="notificationName">which observer list to remove from</param>
		/// <param name="notifyContext">remove the observer with this object as its notifyContext</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RemoveObserver(int notificationName, object notifyContext)
		{
			// the observer list for the notification under inspection
			if (this.m_observerMap.ContainsKey(notificationName))
			{
				IList<IObserver> observers = this.m_observerMap[notificationName];

				// find the observer for the notifyContext
				for (int i = 0; i < observers.Count; i++)
				{
					if (observers[i].CompareNotifyContext(notifyContext))
					{
						// there can only be one Observer for a given notifyContext 
						// in any given Observer list, so remove it and break
						observers.RemoveAt(i);
						break;
					}
				}

				// Also, when a Notification's Observer list length falls to 
				// zero, delete the notification key from the observer map
				if (observers.Count == 0)
				{
					this.m_observerMap.Remove(notificationName);
				}
			}
		}

		#endregion

		#region Controller

		/// <summary>
		/// Register an <c>IController</c> instance with the <c>Controller</c>
		/// </summary>
		/// <param name="controller">A reference to the <c>IController</c> instance</param>
		/// <remarks>
		///     <para>Registers the <c>IController</c> so that it can be retrieved by name, and further interrogates the <c>IController</c> for its <c>INotification</c> interests</para>
		///     <para>If the <c>IController</c> returns any <c>INotification</c> names to be notified about, an <c>Observer</c> is created encapsulating the <c>IController</c> instance's <c>handleNotification</c> method and registering it as an <c>Observer</c> for all <c>INotifications</c> the <c>IController</c> is interested in</para>
		/// </remarks>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RegisterController(IController controller)
		{
			// do not allow re-registration (you must to removeController fist)
			if (this.m_ControllerMap.ContainsKey(controller.Name)) return;

			// Register the Controller for retrieval by name
			this.m_ControllerMap[controller.Name] = controller;

			// Get Notification interests, if any.
			IList<int> interests = controller.ListNotificationInterests();

			// Register Controller as an observer for each of its notification interests
			if (interests.Count > 0)
			{
				// Create Observer
				IObserver observer = new Observer(controller.HandleNotification, controller);

				// Register Controller as Observer for its list of Notification interests
				for (int i = 0; i < interests.Count; i++)
				{
					this.RegisterObserver(interests[i], observer);
				}
			}

			// alert the Controller that it has been registered
			controller.OnRegister();
		}

		/// <summary>
		/// Retrieve an <c>IController</c> from the <c>Controller</c>
		/// </summary>
		/// <param name="controllerName">The name of the <c>IController</c> instance to retrieve</param>
		/// <returns>The <c>IController</c> instance previously registered with the given <c>controllerName</c></returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual IController RetrieveController(string controllerName)
		{
			if (!this.m_ControllerMap.ContainsKey(controllerName)) return null;
			return this.m_ControllerMap[controllerName];
		}

		/// <summary>
		/// Remove an <c>IController</c> from the <c>Controller</c>
		/// </summary>
		/// <param name="controllerName">The name of the <c>IController</c> instance to be removed</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual IController RemoveController(string controllerName)
		{
			IController controller;

			// Retrieve the named Controller
			if (!this.m_ControllerMap.ContainsKey(controllerName)) return null;
			controller = this.m_ControllerMap[controllerName];

			// for every notification this Controller is interested in...
			IList<int> interests = controller.ListNotificationInterests();

			for (int i = 0; i < interests.Count; i++)
			{
				// remove the observer linking the Controller 
				// to the notification interest
				this.RemoveObserver(interests[i], controller);
			}

			// remove the Controller from the map		
			this.m_ControllerMap.Remove(controllerName);

			// alert the Controller that it has been removed
			controller.OnRemove();
			return controller;
		}

		/// <summary>
		/// Check if a Controller is registered or not
		/// </summary>
		/// <param name="controllerName"></param>
		/// <returns>whether a Controller is registered with the given <code>controllerName</code>.</returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual bool HasController(string controllerName)
		{
			return this.m_ControllerMap.ContainsKey(controllerName);
		}

		#endregion

		#endregion

		#region Accessors

		/// <summary>
		/// Controller Singleton Factory method.  This method is thread safe.
		/// </summary>
		public static IMainController Instance
		{
			get
			{
				if (m_instance == null)
				{
					lock (m_staticSyncRoot)
					{
						if (m_instance == null) m_instance = new MainController();
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
		static MainController()
		{
		}

		/// <summary>
		/// Initialize the Singleton Controller instance
		/// </summary>
		/// <remarks>
		/// <para>Called automatically by the constructor, this is your opportunity to initialize the Singleton instance in your subclass without overriding the constructor</para>
		/// </remarks>
		protected virtual void InitializeController()
		{
		}

		#endregion

		#region Members

		/// <summary>
		/// Used for locking the instance calls
		/// </summary>
		protected static readonly object m_staticSyncRoot = new object();

		/// <summary>
		/// Singleton instance
		/// </summary>
		protected static volatile IMainController m_instance;

		/// <summary>
		/// Mapping of Controller names to Controller instances
		/// </summary>
		protected IDictionary<string, IController> m_ControllerMap;

		/// <summary>
		/// Mapping of Notification names to Observer lists
		/// </summary>
		protected IDictionary<int, IList<IObserver>> m_observerMap;

		#endregion
	}
}
