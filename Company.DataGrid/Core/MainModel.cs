/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System.Collections.Generic;

#endregion

namespace Company.DataGrid.Core
{
	/// <summary>
	/// A Singleton <c>IModel</c> implementation
	/// </summary>
	/// <remarks>
	///     <para>In PureMVC, the <c>Model</c> class provides access to model objects (Proxies) by named lookup</para>
	///     <para>The <c>Model</c> assumes these responsibilities:</para>
	///     <list type="bullet">
	///         <item>Maintain a cache of <c>IModel</c> instances</item>
	///         <item>Provide methods for registering, retrieving, and removing <c>IModel</c> instances</item>
	///     </list>
	///     <para>
	///         Your application must register <c>IModel</c> instances
	///         with the <c>Model</c>. Typically, you use an 
	///         <c>IMainCommand</c> to create and register <c>IModel</c> 
	///         instances once the <c>Facade</c> has initialized the Core actors
	///     </para>
	/// </remarks>
	/// <seealso cref="IModel"/>
	public class MainModel : IMainModel
	{
		#region Constructors

		/// <summary>
		/// Constructs and initializes a new model
		/// </summary>
		/// <remarks>
		///     <para>This <c>IModel</c> implementation is a Singleton, so you should not call the constructor directly, but instead call the static Singleton Factory method <c>Model.getInstance()</c></para>
		/// </remarks>
		protected MainModel()
		{
			this.m_ModelMap = new Dictionary<string, IModel>();
			this.InitializeModel();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Register an <c>IModel</c> with the <c>Model</c>
		/// </summary>
		/// <param name="model">An <c>IModel</c> to be held by the <c>Model</c></param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual void RegisterModel(IModel model)
		{
			lock (this.m_syncRoot)
			{
				this.m_ModelMap[model.ModelName] = model;
			}

			model.OnRegister();
		}

		/// <summary>
		/// Retrieve an <c>IModel</c> from the <c>Model</c>
		/// </summary>
		/// <param name="ModelName">The name of the <c>IModel</c> to retrieve</param>
		/// <returns>The <c>IModel</c> instance previously registered with the given <c>ModelName</c></returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual IModel RetrieveModel(string ModelName)
		{
			lock (this.m_syncRoot)
			{
				if (!this.m_ModelMap.ContainsKey(ModelName)) return null;
				return this.m_ModelMap[ModelName];
			}
		}

		/// <summary>
		/// Check if a Model is registered
		/// </summary>
		/// <param name="ModelName"></param>
		/// <returns>whether a Model is currently registered with the given <c>ModelName</c>.</returns>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual bool HasModel(string ModelName)
		{
			lock (this.m_syncRoot)
			{
				return this.m_ModelMap.ContainsKey(ModelName);
			}
		}

		/// <summary>
		/// Remove an <c>IModel</c> from the <c>Model</c>
		/// </summary>
		/// <param name="ModelName">The name of the <c>IModel</c> instance to be removed</param>
		/// <remarks>This method is thread safe and needs to be thread safe in all implementations.</remarks>
		public virtual IModel RemoveModel(string ModelName)
		{
			IModel model = null;

			lock (this.m_syncRoot)
			{
				if (this.m_ModelMap.ContainsKey(ModelName))
				{
					model = this.RetrieveModel(ModelName);
					this.m_ModelMap.Remove(ModelName);
				}
			}

			if (model != null) model.OnRemove();
			return model;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// <c>Model</c> Singleton Factory method.  This method is thread safe.
		/// </summary>
		public static IMainModel Instance
		{
			get
			{
				if (m_instance == null)
				{
					lock (m_staticSyncRoot)
					{
						if (m_instance == null) m_instance = new MainModel();
					}
				}

				return m_instance;
			}
		}

		#endregion

		#region Protected & Internal Methods

		/// <summary>
		/// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
		/// </summary>
		static MainModel()
		{
		}

		/// <summary>
		/// Initialize the Singleton <c>Model</c> instance.
		/// </summary>
		/// <remarks>
		///     <para>Called automatically by the constructor, this is your opportunity to initialize the Singleton instance in your subclass without overriding the constructor</para>
		/// </remarks>
		protected virtual void InitializeModel()
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
		protected static volatile IMainModel m_instance;

		/// <summary>
		/// Used for locking
		/// </summary>
		protected readonly object m_syncRoot = new object();

		/// <summary>
		/// Mapping of ModelNames to <c>IModel</c> instances
		/// </summary>
		protected IDictionary<string, IModel> m_ModelMap;

		#endregion
	}
}