/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System.Collections.Generic;
using XamlGrid.Aspects;

#endregion

namespace XamlGrid.Core
{
	/// <summary>
	/// A base <c>IController</c> implementation
	/// </summary>
	/// <see cref="MainController"/>
	public class Controller<T> : Notifier, IController<T> where T : class
	{
		#region Constants

		/// <summary>
		/// The name of the <c>Controller</c>
		/// </summary>
		/// <remarks>
		///     <para>Typically, a <c>Controller</c> will be written to serve one specific control or group controls and so, will not have a need to be dynamically named</para>
		/// </remarks>
		public const string NAME = "Controller";

		#endregion

		private T view;

		#region Constructors

		/// <summary>
		/// Constructs a new Controller with the default name and no view component.
		/// </summary>
		public Controller()
			: this(NAME, null)
		{
		}

		/// <summary>
		/// Constructs a new Controller with the specified name and no view component.
		/// </summary>
		/// <param name="controllerName">The name of the Controller.</param>
		public Controller(string controllerName)
			: this(controllerName, null)
		{
		}

		/// <summary>
		/// Constructs a new Controller with the specified name and view component.
		/// </summary>
		/// <param name="controllerName">The name of the Controller.</param>
		/// <param name="view">The view component to be controlled.</param>
		public Controller(string controllerName, T view)
		{
			this.mName = controllerName ?? NAME;
			this.view = view;
		}

		#endregion

		#region Public Methods

		object IController.View
		{
			get { return this.View; }
			set { this.View = (T) value; }
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names.</returns>
		public virtual IList<int> ListNotificationInterests()
		{
			return new List<int>();
		}

		/// <summary>
		/// Handle <c>INotification</c>s.
		/// </summary>
		/// <param name="notification">The <c>INotification</c> instance to handle</param>
		/// <remarks>
		///     <para>
		///        Typically this will be handled in a switch statement, with one 'case' entry per <c>INotification</c> the <c>Controller</c> is interested in. 
		///     </para>
		/// </remarks>
		[Validate]
		public virtual void HandleNotification([NotNull] INotification notification)
		{
		}

		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is registered.
		/// </summary>
		public virtual void OnRegister()
		{
		}

		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is removed.
		/// </summary>
		public virtual void OnRemove()
		{
		}

		#endregion

		#region Accessors

		/// <summary>
		/// The name of the <c>Controller</c>
		/// </summary>
		/// <remarks><para>You should override this in your subclass</para></remarks>
		public virtual string Name
		{
			get { return this.mName; }
			set { this.mName = value; }
		}

		/// <summary>
		/// The <code>IController</code>'s view component.
		/// </summary>
		/// <remarks>
		///     <para>Additionally, an implicit getter will usually be defined in the subclass that casts the Controller object to a type, like this:</para>
		///     <example>
		///         <code>
		///             private System.Windows.Form.ComboBox comboBox {
		///                 get { return View as ComboBox; }
		///             }
		///         </code>
		///     </example>
		/// </remarks>
		public virtual T View
		{
			get
			{
				return this.view;
			}
			set
			{
				this.view = value;
			}
		}

		#endregion

		#region Members

		/// <summary>
		/// The Controller name
		/// </summary>
		protected string mName;

		#endregion
	}
}
