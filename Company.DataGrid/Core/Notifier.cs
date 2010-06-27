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
	/// A Base <c>INotifier</c> implementation
	/// </summary>
	/// <remarks>
	///     <para><c>MacroCommand, Controller, Controller</c> and <c>Model</c> all have a need to send <c>Notifications</c></para>
	///     <para>The <c>INotifier</c> interface provides a common method called <c>sendNotification</c> that relieves implementation code of the necessity to actually construct <c>Notifications</c></para>
	///     <para>The <c>Notifier</c> class, which all of the above mentioned classes extend, provides an initialized reference to the <c>Facade</c> Singleton, which is required for the convienience method for sending <c>Notifications</c>, but also eases implementation as these classes have frequent <c>Facade</c> interactions and usually require access to the facade anyway</para>
	/// </remarks>
	/// <see cref="Core.Facade"/>
	/// <see cref="Model"/>
	/// <see cref="Company.Widgets.Core"/>
	/// <see cref="MacroCommand"/>
	/// <see cref="Controller"/>
	public class Notifier : INotifier
	{
		#region Public Methods

		/// <summary>
		/// Send an <c>INotification</c>
		/// </summary>
		/// <param name="notificationName">The name of the notiification to send</param>
		/// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
		/// <remarks>This method is thread safe</remarks>
		public virtual void SendNotification(int notificationName)
		{
			// The Facade SendNotification is thread safe, therefore this method is thread safe.
			this.m_facade.SendNotification(notificationName);
		}

		/// <summary>
		/// Send an <c>INotification</c>
		/// </summary>
		/// <param name="notificationName">The name of the notification to send</param>
		/// <param name="body">The body of the notification</param>
		/// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
		/// <remarks>This method is thread safe</remarks>
		public virtual void SendNotification(int notificationName, object body)
		{
			// The Facade SendNotification is thread safe, therefore this method is thread safe.
			this.m_facade.SendNotification(notificationName, body);
		}

		/// <summary>
		/// Send an <c>INotification</c>
		/// </summary>
		/// <param name="notificationName">The name of the notification to send</param>
		/// <param name="body">The body of the notification</param>
		/// <param name="type">The type of the notification</param>
		/// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
		/// <remarks>This method is thread safe</remarks>
		public virtual void SendNotification(int notificationName, object body, string type)
		{
			// The Facade SendNotification is thread safe, therefore this method is thread safe.
			this.m_facade.SendNotification(notificationName, body, type);
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Local reference to the Facade Singleton
		/// </summary>
		protected virtual IFacade Facade
		{
			get { return this.m_facade; }
		}

		#endregion

		#region Members

		/// <summary>
		/// Local reference to the Facade Singleton
		/// </summary>
		private readonly IFacade m_facade = Core.Facade.Instance;

		#endregion
	}
}
