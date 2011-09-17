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
	/// A base Singleton <c>IFacade</c> implementation
	/// </summary>
	/// <remarks>
	///     <para>In PureMVC, the <c>Facade</c> class assumes these responsibilities:</para>
	///     <list type="bullet">
	///         <item>Initializing the <c>Model</c>, <c>Controller</c> and <c>Controller</c> Singletons</item>
	///         <item>Providing all the methods defined by the <c>IModel, IController, &amp; IMainCommand</c> interfaces</item>
	///         <item>Providing the ability to override the specific <c>Model</c>, <c>Controller</c> and <c>Controller</c> Singletons created</item>
	///         <item>Providing a single point of contact to the application for registering <c>Controllers</c> and notifying <c>Observers</c></item>
	///     </list>
	///     <example>
	///         <code>
	///	using PureMVC.Patterns;
	/// 
	///	using com.me.myapp.model;
	///	using com.me.myapp.Controller;
	///	using com.me.myapp.controller;
	/// 
	///	public class MyFacade : Facade
	///	{
	///		// Notification constants. The Facade is the ideal
	///		// location for these constants, since any part
	///		// of the application participating in PureMVC 
	///		// Observer Notification will know the Facade.
	///		public static const string GO_Controller = "go";
	/// 
	///     // we aren't allowed to initialize new instances from outside this class
	///     protected MyFacade() {}
	/// 
	///     // we must specify the type of instance
	///     static MyFacade()
	///     {
	///         instance = new MyFacade();
	///     }
	/// 
	///		// Override Singleton Factory method 
	///		public new static MyFacade getInstance() {
	///			return instance as MyFacade;
	///		}
	/// 		
	///		// optional initialization hook for Facade
	///		public override void initializeFacade() {
	///			base.initializeFacade();
	///			// do any special subclass initialization here
	///		}
	///	
	///		// optional initialization hook for Command
	///		public override void initializeController() {
	///			// call base to use the PureMVC Controller Singleton. 
	///			base.initializeController();
	/// 
	///			// Otherwise, if you're implmenting your own
	///			// IMainCommand, then instead do:
	///			// if ( controller != null ) return;
	///			// controller = MyAppController.getInstance();
	/// 		
	///			// do any special subclass initialization here
	///			// such as registering Controllers
	///			registerController( GO_Controller, com.me.myapp.controller.GoController )
	///		}
	///	
	///		// optional initialization hook for Model
	///		public override void initializeModel() {
	///			// call base to use the PureMVC Model Singleton. 
	///			base.initializeModel();
	/// 
	///			// Otherwise, if you're implmenting your own
	///			// IModel, then instead do:
	///			// if ( model != null ) return;
	///			// model = MyAppModel.getInstance();
	/// 		
	///			// do any special subclass initialization here
	///			// such as creating and registering Model Models
	///			// that don't require a facade reference at
	///			// construction time, such as fixed type lists
	///			// that never need to send Notifications.
	///			regsiterModel( new USStateNamesModel() );
	/// 			
	///			// CAREFUL: Can't reference Facade instance in constructor 
	///			// of new Models from here, since this step is part of
	///			// Facade construction!  Usually, Models needing to send 
	///			// notifications are registered elsewhere in the app 
	///			// for this reason.
	///		}
	///	
	///		// optional initialization hook for Controller
	///		public override void initializeController() {
	///			// call base to use the PureMVC Controller Singleton. 
	///			base.initializeController();
	/// 
	///			// Otherwise, if you're implmenting your own
	///			// IController, then instead do:
	///			// if ( Controller != null ) return;
	///			// Controller = MyAppController.Instance;
	/// 		
	///			// do any special subclass initialization here
	///			// such as creating and registering Views
	///			// that do not need a Facade reference at construction
	///			// time.
	///			registerController( new LoginController() ); 
	/// 
	///			// CAREFUL: Can't reference Facade instance in constructor 
	///			// of new Views from here, since this is a step
	///			// in Facade construction! Usually, all Views need 
	///			// receive notifications, and are registered elsewhere in 
	///			// the app for this reason.
	///		}
	///	}
	///         </code>
	///     </example>
	/// </remarks>
	/// <see cref="MainCommand"/>
	/// <see cref="MacroCommand"/>
	/// <see cref="Controller"/>
	/// <see cref="Notification"/>
	/// <see cref="Model"/>
	/// <see cref="MainController"/>
	/// <see cref="MainModel"/>
	/// <see cref="Controller"/>
	public class Facade : IFacade
	{
		#region Constructors

		/// <summary>
		/// Constructor that initializes the Facade
		/// </summary>
		/// <remarks>
		///     <para>This <c>IFacade</c> implementation is a Singleton, so you should not call the constructor directly, but instead call the static Singleton Factory method <c>Facade.Instance</c></para>
		/// </remarks>
		protected Facade()
		{
			this.InitializeFacade();
		}

		#endregion

		#region Public Methods

		#region Model

		/// <summary>
		/// Register an <c>IModel</c> with the <c>Model</c> by name
		/// </summary>
		/// <param name="model">The <c>IModel</c> to be registered with the <c>Model</c></param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RegisterModel(IModel model)
		{
			// The model is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the model.
			this.mMainModel.RegisterModel(model);
		}

		/// <summary>
		/// Retrieve a <c>IModel</c> from the <c>Model</c> by name
		/// </summary>
		/// <param name="ModelName">The name of the <c>IModel</c> instance to be retrieved</param>
		/// <returns>The <c>IModel</c> previously regisetered by <c>ModelName</c> with the <c>Model</c></returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual IModel RetrieveModel(string ModelName)
		{
			// The model is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the model.
			return this.mMainModel.RetrieveModel(ModelName);
		}

		/// <summary>
		/// Remove an <c>IModel</c> instance from the <c>Model</c> by name
		/// </summary>
		/// <param name="ModelName">The <c>IModel</c> to remove from the <c>Model</c></param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual IModel RemoveModel(string ModelName)
		{
			// The model is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the model.
			return this.mMainModel.RemoveModel(ModelName);
		}

		/// <summary>
		/// Check if a Model is registered
		/// </summary>
		/// <param name="ModelName">The name of the <c>IModel</c> instance to check for</param>
		/// <returns>whether a Model is currently registered with the given <c>ModelName</c>.</returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual bool HasModel(string ModelName)
		{
			// The model is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the model.
			return this.mMainModel.HasModel(ModelName);
		}

		#endregion

		#region Command

		/// <summary>
		/// Register an <c>IMainCommand</c> with the <c>Controller</c>
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotification</c> to associate the <c>IMainCommand</c> with.</param>
		/// <param name="commandType">A reference to the <c>Type</c> of the <c>IMainCommand</c></param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RegisterCommand(int notificationName, Type commandType)
		{
			// The controller is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the controller.
			this.mainCommand.RegisterCommand(notificationName, commandType);
		}

		/// <summary>
		/// Remove a previously registered <c>IMainCommand</c> to <c>INotification</c> mapping from the Controller.
		/// </summary>
		/// <param name="notificationName">TRemove a previously registered <c>IMainCommand</c> to <c>INotification</c> mapping from the Controller.</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RemoveCommand(int notificationName)
		{
			// The controller is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the controller.
			this.mainCommand.RemoveCommand(notificationName);
		}

		/// <summary>
		/// Check if a Controller is registered for a given Notification 
		/// </summary>
		/// <param name="notificationName">The name of the <c>INotification</c> to check for.</param>
		/// <returns>whether a Controller is currently registered for the given <c>notificationName</c>.</returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual bool HasCommand(int notificationName)
		{
			// The controller is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the controller.
			return this.mainCommand.HasCommand(notificationName);
		}

		#endregion

		#region Controller

		/// <summary>
		/// Register an <c>IController</c> instance with the <c>Controller</c>
		/// </summary>
		/// <param name="controller">A reference to the <c>IController</c> instance</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RegisterController(IController controller)
		{
			// The Controller is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the Controller.
			this.mMainController.RegisterController(controller);
		}

		/// <summary>
		/// Retrieve an <c>IController</c> instance from the <c>Controller</c>
		/// </summary>
		/// <param name="controllerName">The name of the <c>IController</c> instance to retrieve</param>
		/// <returns>The <c>IController</c> previously registered with the given <c>ControllerName</c></returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual IController RetrieveController(string controllerName)
		{
			// The Controller is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the Controller.
			return this.mMainController.RetrieveController(controllerName);
		}

		/// <summary>
		/// Remove a <c>IController</c> instance from the <c>Controller</c>
		/// </summary>
		/// <param name="controllerName">The name of the <c>IController</c> instance to be removed</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual IController RemoveController(string controllerName)
		{
			// The Controller is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the Controller.
			return this.mMainController.RemoveController(controllerName);
		}

		/// <summary>
		/// Check if a Controller is registered or not
		/// </summary>
		/// <param name="controllerName">The name of the <c>IController</c> instance to check for</param>
		/// <returns>whether a Controller is registered with the given <code>ControllerName</code>.</returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual bool HasController(string controllerName)
		{
			// The Controller is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the Controller.
			return this.mMainController.HasController(controllerName);
		}

		#endregion

		#region Observer

		/// <summary>
		/// Notify <c>Observer</c>s of an <c>INotification</c>
		/// </summary>
		/// <remarks>This method is left public mostly for backward compatibility, and to allow you to send custom notification classes using the facade.</remarks>
		/// <remarks>Usually you should just call sendNotification and pass the parameters, never having to construct the notification yourself.</remarks>
		/// <param name="notification">The <c>INotification</c> to have the <c>Controller</c> notify observers of</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void NotifyObservers(INotification notification)
		{
			// The Controller is initialized in the constructor of the singleton, so this call should be thread safe.
			// This method is thread safe on the Controller.
			this.mMainController.NotifyObservers(notification);
		}

		#endregion

		/// <summary>
		/// Send an <c>INotification</c>
		/// </summary>
		/// <param name="notificationName">The name of the notiification to send</param>
		/// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void SendNotification(int notificationName)
		{
			this.NotifyObservers(new Notification(notificationName));
		}

		/// <summary>
		/// Send an <c>INotification</c>
		/// </summary>
		/// <param name="notificationName">The name of the notification to send</param>
		/// <param name="body">The body of the notification</param>
		/// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void SendNotification(int notificationName, object body)
		{
			this.NotifyObservers(new Notification(notificationName, body));
		}

		/// <summary>
		/// Send an <c>INotification</c>
		/// </summary>
		/// <param name="notificationName">The name of the notification to send</param>
		/// <param name="body">The body of the notification</param>
		/// <param name="type">The type of the notification</param>
		/// <remarks>Keeps us from having to construct new notification instances in our implementation code</remarks>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void SendNotification(int notificationName, object body, string type)
		{
			this.NotifyObservers(new Notification(notificationName, body, type));
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Facade Singleton Factory method.  This method is thread safe.
		/// </summary>
		public static IFacade Instance
		{
			get
			{
				if (m_instance == null)
				{
					lock (m_staticSyncRoot)
					{
						if (m_instance == null) m_instance = new Facade();
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
		///</summary>
		static Facade()
		{
		}

		/// <summary>
		/// Initialize the Singleton <c>Facade</c> instance
		/// </summary>
		/// <remarks>
		/// <para>Called automatically by the constructor. Override in your subclass to do any subclass specific initializations. Be sure to call <c>base.initializeFacade()</c>, though</para>
		/// </remarks>
		protected virtual void InitializeFacade()
		{
			this.InitializeMainModel();
			this.InitializeMainCommand();
			this.InitializeController();
		}

		/// <summary>
		/// Initialize the <c>Controller</c>
		/// </summary>
		/// <remarks>
		///     <para>Called by the <c>initializeFacade</c> method. Override this method in your subclass of <c>Facade</c> if one or both of the following are true:</para>
		///     <list type="bullet">
		///         <item>You wish to initialize a different <c>IMainCommand</c></item>
		///         <item>You have <c>Controllers</c> to register with the <c>Controller</c> at startup</item>
		///     </list>
		///     <para>If you don't want to initialize a different <c>IMainCommand</c>, call <c>base.initializeController()</c> at the beginning of your method, then register <c>Controller</c>s</para>
		/// </remarks>
		protected virtual void InitializeMainCommand()
		{
			if (this.mainCommand != null) return;
			this.mainCommand = MainCommand.Instance;
		}

		/// <summary>
		/// Initialize the <c>Model</c>
		/// </summary>
		/// <remarks>
		///     <para>Called by the <c>initializeFacade</c> method. Override this method in your subclass of <c>Facade</c> if one or both of the following are true:</para>
		///     <list type="bullet">
		///         <item>You wish to initialize a different <c>IModel</c></item>
		///         <item>You have <c>Model</c>s to register with the Model that do not retrieve a reference to the Facade at construction time</item>
		///     </list>
		///     <para>If you don't want to initialize a different <c>IModel</c>, call <c>base.initializeModel()</c> at the beginning of your method, then register <c>Model</c>s</para>
		///     <para>Note: This method is <i>rarely</i> overridden; in practice you are more likely to use a <c>Controller</c> to create and register <c>Model</c>s with the <c>Model</c>, since <c>Model</c>s with mutable data will likely need to send <c>INotification</c>s and thus will likely want to fetch a reference to the <c>Facade</c> during their construction</para>
		/// </remarks>
		protected virtual void InitializeMainModel()
		{
			if (this.mMainModel != null) return;
			this.mMainModel = MainModel.Instance;
		}

		/// <summary>
		/// Initialize the <c>Controller</c>
		/// </summary>
		/// <remarks>
		///     <para>Called by the <c>initializeFacade</c> method. Override this method in your subclass of <c>Facade</c> if one or both of the following are true:</para>
		///     <list type="bullet">
		///         <item>You wish to initialize a different <c>IController</c></item>
		///         <item>You have <c>Observers</c> to register with the <c>Controller</c></item>
		///     </list>
		///     <para>If you don't want to initialize a different <c>IController</c>, call <c>base.initializeController()</c> at the beginning of your method, then register <c>IController</c> instances</para>
		///     <para>Note: This method is <i>rarely</i> overridden; in practice you are more likely to use a <c>Controller</c> to create and register <c>Controller</c>s with the <c>Controller</c>, since <c>IController</c> instances will need to send <c>INotification</c>s and thus will likely want to fetch a reference to the <c>Facade</c> during their construction</para>
		/// </remarks>
		protected virtual void InitializeController()
		{
			if (this.mMainController != null) return;
			this.mMainController = MainController.Instance;
		}

		#endregion

		#region Members

		/// <summary>
		/// Used for locking the instance calls
		/// </summary>
		protected static readonly object m_staticSyncRoot = new object();

		/// <summary>
		/// The Singleton Facade Instance
		/// </summary>
		protected static volatile IFacade m_instance;

		/// <summary>
		/// Private reference to the Command
		/// </summary>
		protected IMainCommand mainCommand;

		/// <summary>
		/// Private reference to the Model
		/// </summary>
		protected IMainModel mMainModel;

		/// <summary>
		/// Private reference to the Controller
		/// </summary>
		protected IMainController mMainController;

		#endregion
	}
}
