/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using



#endregion

namespace Guidgets.XamlGrid.Core
{
	/// <summary>
	/// A base <c>INotification</c> implementation
	/// </summary>
	/// <remarks>
	///     <para>PureMVC does not rely upon underlying event models</para>
	///     <para>The Observer Pattern as implemented within PureMVC exists to support event-driven communication between the application and the actors of the MVC triad</para>
	///     <para>Notifications are not meant to be a replacement for Events. Generally, <c>IController</c> implementors place event handlers on their Controller components, which they then handle in the usual way. This may lead to the broadcast of <c>Notification</c>s to trigger <c>IMainCommand</c>s or to communicate with other <c>IViews</c>. <c>IModel</c> and <c>IMainCommand</c> instances communicate with each other and <c>IController</c>s by broadcasting <c>INotification</c>s</para>
	/// </remarks>
	/// <see cref="Observer"/>
	public class Notification : INotification
	{
		#region Constructors

		/// <summary>
		/// Constructs a new notification with the specified name, default body and type
		/// </summary>
		/// <param name="name">The name of the <c>Notification</c> instance</param>
		public Notification(int name)
			: this(name, null, null)
		{
		}

		/// <summary>
		/// Constructs a new notification with the specified name and body, with the default type
		/// </summary>
		/// <param name="name">The name of the <c>Notification</c> instance</param>
		/// <param name="body">The <c>Notification</c>s body</param>
		public Notification(int name, object body)
			: this(name, body, null)
		{
		}

		/// <summary>
		/// Constructs a new notification with the specified name, body and type
		/// </summary>
		/// <param name="code">The name of the <c>Notification</c> instance</param>
		/// <param name="body">The <c>Notification</c>s body</param>
		/// <param name="type">The type of the <c>Notification</c></param>
		public Notification(int code, object body, string type)
		{
			this.mCode = code;
			this.body = body;
			this.type = type;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Get the string representation of the <c>Notification instance</c>
		/// </summary>
		/// <returns>The string representation of the <c>Notification</c> instance</returns>
		public override string ToString()
		{
			string msg = "Notification Name: " + this.Code;
			msg += "\nBody:" + ((this.Body == null) ? "null" : this.Body.ToString());
			msg += "\nType:" + (this.Type ?? "null");
			return msg;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// The name of the <c>Notification</c> instance
		/// </summary>
		public virtual int Code
		{
			get
			{
				return this.mCode;
			}
		}

		/// <summary>
		/// The body of the <c>Notification</c> instance
		/// </summary>
		/// <remarks>This accessor is thread safe</remarks>
		public virtual object Body
		{
			get
			{
				return this.body;
			}
			set
			{
				this.body = value;
			}
		}

		/// <summary>
		/// The type of the <c>Notification</c> instance
		/// </summary>
		/// <remarks>This accessor is thread safe</remarks>
		public virtual string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		#endregion

		#region Members

		/// <summary>
		/// The name of the notification instance 
		/// </summary>
		private readonly int mCode;
		private object body;
		private string type;

		#endregion
	}
}
