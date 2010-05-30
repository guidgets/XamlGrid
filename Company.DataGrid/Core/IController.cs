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
	/// The interface definition for a PureMVC Controller.
	/// </summary>
	/// <remarks>
	///     <para>In PureMVC, <c>IController</c> implementors assume these responsibilities:</para>
	///     <list type="bullet">
	///         <item>Implement a common method which returns a list of all <c>INotification</c>s the <c>IController</c> has interest in.</item>
	///         <item>Implement a common notification (callback) method</item>
	///     </list>
	///     <para>Additionally, <c>IController</c>s typically:</para>
	///     <list type="bullet">
	///         <item>Act as an intermediary between one or more Controller components such as text boxes or list controls, maintaining references and coordinating their behavior.</item>
	///         <item>In Flash-based apps, this is often the place where event listeners are added to Controller components, and their handlers implemented</item>
	///         <item>Respond to and generate <c>INotifications</c>, interacting with of the rest of the PureMVC app</item>
	///     </list>
	///     <para>When an <c>IController</c> is registered with the <c>IController</c>, the <c>IController</c> will call the <c>IController</c>'s <c>listNotificationInterests</c> method. The <c>IController</c> will return an <c>IList</c> of <c>INotification</c> names which it wishes to be notified about</para>
	///     <para>The <c>IController</c> will then create an <c>Observer</c> object encapsulating that <c>IController</c>'s (<c>handleNotification</c>) method and register it as an Observer for each <c>INotification</c> name returned by <c>listNotificationInterests</c></para>
	///     <para>A concrete IController implementor usually looks something like this:</para>
	///     <example>
	///         <code>
	///	using PureMVC.Patterns.~~;
	///	using PureMVC.Core.Controller.~~;
	/// 
	///	using com.me.myapp.model.~~;
	///	using com.me.myapp.Controller.~~;
	///	using com.me.myapp.controller.~~;
	/// 		
	/// using System.Windows.Forms; 
	/// using System.Data;
	/// 
	/// public class MyController : Controller, IController {
	/// 
	/// 		public MyController( ControllerComponent:object ) {
	/// 			base( ControllerComponent );
	///             combo.DataSourceChanged += new EventHandler(onChange);
	/// 		}
	/// 		
	/// 		public IList listNotificationInterests() {
	/// 				return new string[] {
	///                      MyFacade.SET_SELECTION, 
	///                      MyFacade.SET_DATAPROVIDER };
	/// 		}
	/// 
	/// 		public void handleNotification( notification:INotification ) {
	/// 				switch ( notification.getName() ) {
	/// 					case MyFacade.SET_SELECTION:
	///                         combo.SelectedItem = notification.getBody();
	/// 						break;
	///                     // set the data source of the combo box
	/// 					case MyFacade.SET_DATASOURCE:
	/// 						combo.DataSource = notification.getBody();
	/// 						break;
	/// 				}
	/// 		}
	/// 
	/// 		// Invoked when the combo box dispatches a change event, we send a
	///      // notification with the
	/// 		protected void onChange(object sender, EventArgs e) {
	/// 			sendNotification( MyFacade.MYCOMBO_CHANGED, sender );
	/// 		}
	/// 
	/// 		// A private getter for accessing the Controller object by class
	///      private ComboBox combo {
	///          get { return Controller as ComboBox; }
	///      }
	/// 
	/// }
	///         </code>
	///     </example>
	/// </remarks>
	/// <see cref="INotification"/>
	public interface IController
	{
		/// <summary>
		/// Tthe <c>IController</c> instance name
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The <c>IController</c>'s Controller component
		/// </summary>
		object ViewComponent { get; set; }

		/// <summary>
		/// List <c>INotification interests</c>
		/// </summary>
		/// <returns>An <c>IList</c> of the <c>INotification</c> names this <c>IController</c> has an interest in</returns>
		IList<string> ListNotificationInterests();

		/// <summary>
		/// Handle an <c>INotification</c>
		/// </summary>
		/// <param name="notification">The <c>INotification</c> to be handled</param>
		void HandleNotification(INotification notification);

		/// <summary>
		/// Called by the Controller when the Controller is registered
		/// </summary>
		void OnRegister();

		/// <summary>
		/// Called by the Controller when the Controller is removed
		/// </summary>
		void OnRemove();
	}
}