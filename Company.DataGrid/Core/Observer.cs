/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using



#endregion

namespace Company.Widgets.Core
{
	/// <summary>
	/// A base <c>IObserver</c> implementation
	/// </summary>
	/// <remarks>
	///     <para>An <c>Observer</c> is an object that encapsulates information about an interested object with a method that should be called when a particular <c>INotification</c> is broadcast</para>
	///     <para>In PureMVC, the <c>Observer</c> class assumes these responsibilities:</para>
	///     <list type="bullet">
	///         <item>Encapsulate the notification (callback) method of the interested object</item>
	///         <item>Encapsulate the notification context (this) of the interested object</item>
	///         <item>Provide methods for setting the notification method and context</item>
	///         <item>Provide a method for notifying the interested object</item>
	///     </list>
	/// </remarks>
	/// <see cref="MainController"/>
	/// <see cref="Notification"/>
	public class Observer : IObserver
	{
		private HandleNotification notifyMethod;
		private object notifyContext;

		#region Constructors

		/// <summary>
		/// Constructs a new observer with the specified notification method and context
		/// </summary>
		/// <param name="notifyMethod">The notification method of the interested object</param>
		/// <param name="notifyContext">The notification context of the interested object</param>
		/// <remarks>
		///     <para>The notification method on the interested object should take on parameter of type <c>INotification</c></para>
		/// </remarks>
		public Observer(HandleNotification notifyMethod, object notifyContext)
		{
			this.notifyMethod = notifyMethod;
			this.notifyContext = notifyContext;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Notify the interested object
		/// </summary>
		/// <remarks>This method is thread safe</remarks>
		/// <param name="notification">The <c>INotification</c> to pass to the interested object's notification method</param>
		public virtual void NotifyObserver(INotification notification)
		{
			HandleNotification method;

			// Retrieve the current state of the object, then notify outside of our thread safe block
			lock (this.syncRoot)
			{
				method = this.NotifyMethod;
			}
			method(notification);
		}

		/// <summary>
		/// Compare an object to the notification context
		/// </summary>
		/// <remarks>This method is thread safe</remarks>
		/// <param name="obj">The object to compare</param>
		/// <returns>Indicating if the object and the notification context are the same</returns>
		public virtual bool CompareNotifyContext(object obj)
		{
			lock (this.syncRoot)
			{
				// Compare on the current state
				return this.NotifyContext.Equals(obj);
			}
		}

		#endregion

		#region Accessors


		/// <summary>
		/// The notification (callback) method of the interested object
		/// </summary>
		/// <remarks>The notification method should take one parameter of type <c>INotification</c></remarks>
		/// <remarks>This accessor is thread safe</remarks>
		public virtual HandleNotification NotifyMethod
		{
			get
			{
				return this.notifyMethod;
			}
			set
			{
				this.notifyMethod = value;
			}
		}

		/// <summary>
		/// The notification context (this) of the interested object
		/// </summary>
		/// <remarks>This accessor is thread safe</remarks>
		public virtual object NotifyContext
		{
			get
			{
				return this.notifyContext;
			}
			set
			{
				this.notifyContext = value;
			}
		}

		#endregion

		#region Members

		/// <summary>
		/// Used for locking
		/// </summary>
		protected readonly object syncRoot = new object();

		#endregion
	}
}
