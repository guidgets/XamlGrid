/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using System.Reflection;

#endregion

namespace Company.DataGrid.Core
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
	/// <see cref="Company.DataGrid.Core.Notification"/>
	public class Observer : IObserver
	{
		#region Constructors

		/// <summary>
		/// Constructs a new observer with the specified notification method and context
		/// </summary>
		/// <param name="notifyMethod">The notification method of the interested object</param>
		/// <param name="notifyContext">The notification context of the interested object</param>
		/// <remarks>
		///     <para>The notification method on the interested object should take on parameter of type <c>INotification</c></para>
		/// </remarks>
		public Observer(string notifyMethod, object notifyContext)
		{
			this.NotifyMethod = notifyMethod;
			this.NotifyContext = notifyContext;
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
			object context;
			string method;

			// Retrieve the current state of the object, then notify outside of our thread safe block
			lock (this.m_syncRoot)
			{
				context = this.NotifyContext;
				method = this.NotifyMethod;
			}

			Type t = context.GetType();
			MethodInfo mi = t.GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
			mi.Invoke(context, new object[] { notification });
		}

		/// <summary>
		/// Compare an object to the notification context
		/// </summary>
		/// <remarks>This method is thread safe</remarks>
		/// <param name="obj">The object to compare</param>
		/// <returns>Indicating if the object and the notification context are the same</returns>
		public virtual bool CompareNotifyContext(object obj)
		{
			lock (this.m_syncRoot)
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
		public virtual string NotifyMethod
		{
			private get; 
			set;
		}

		/// <summary>
		/// The notification context (this) of the interested object
		/// </summary>
		/// <remarks>This accessor is thread safe</remarks>
		public virtual object NotifyContext
		{
			private get; 
			set;
		}

		#endregion

		#region Members

		/// <summary>
		/// Used for locking
		/// </summary>
		protected readonly object m_syncRoot = new object();

		#endregion
	}
}
