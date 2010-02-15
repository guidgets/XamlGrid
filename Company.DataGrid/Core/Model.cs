/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using



#endregion

namespace Company.DataGrid.Core
{
	/// <summary>
	/// A base <c>IModel</c> implementation
	/// </summary>
	/// <remarks>
	/// 	<para>In PureMVC, <c>Model</c> classes are used to manage parts of the application's data model</para>
	/// 	<para>A <c>Model</c> might simply manage a reference to a local data object, in which case interacting with it might involve setting and getting of its data in synchronous fashion</para>
	/// 	<para><c>Model</c> classes are also used to encapsulate the application's interaction with remote services to save or retrieve data, in which case, we adopt an asyncronous idiom; setting data (or calling a method) on the <c>Model</c> and listening for a <c>Notification</c> to be sent when the <c>Model</c> has retrieved the data from the service</para>
	/// </remarks>
	/// <see cref="MainModel"/>
	public class Model : Notifier, IModel
	{
		#region Constants

		/// <summary>
		/// The default Model name
		/// </summary>
		public static string NAME = "Model";

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new Model with the default name and no data
		/// </summary>
		public Model()
			: this(NAME, null)
		{
		}

		/// <summary>
		/// Constructs a new Model with the specified name and no data
		/// </summary>
		/// <param name="modelName">The name of the Model</param>
		public Model(string modelName)
			: this(modelName, null)
		{
		}

		/// <summary>
		/// Constructs a new Model with the specified name and data
		/// </summary>
		/// <param name="modelName">The name of the Model</param>
		/// <param name="data">The data to be managed</param>
		public Model(string modelName, object data)
		{
			this.m_ModelName = modelName ?? NAME;
			if (data != null) this.m_data = data;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Called by the Model when the Model is registered
		/// </summary>
		public virtual void OnRegister()
		{
		}

		/// <summary>
		/// Called by the Model when the Model is removed
		/// </summary>
		public virtual void OnRemove()
		{
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Get the Model name
		/// </summary>
		/// <returns></returns>
		public string ModelName
		{
			get { return this.m_ModelName; }
		}

		/// <summary>
		/// Set the data object
		/// </summary>
		public object Data
		{
			get { return this.m_data; }
			set { this.m_data = value; }
		}

		#endregion

		#region Members

		/// <summary>
		/// The data object to be managed
		/// </summary>
		protected object m_data;

		/// <summary>
		/// The name of the Model
		/// </summary>
		protected string m_ModelName;

		#endregion
	}
}