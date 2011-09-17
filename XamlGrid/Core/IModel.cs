/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
namespace XamlGrid.Core
{
	/// <summary>
	/// The interface definition for a PureMVC Model
	/// </summary>
	/// <remarks>
	///     <para>In PureMVC, <c>IModel</c> implementors assume these responsibilities:</para>
	///     <list type="bullet">
	///         <item>Implement a common method which returns the name of the Model</item>
	///     </list>
	///     <para>Additionally, <c>IModel</c>s typically:</para>
	///     <list type="bullet">
	///         <item>Maintain references to one or more pieces of model data</item>
	///         <item>Provide methods for manipulating that data</item>
	///         <item>Generate <c>INotifications</c> when their model data changes</item>
	///         <item>Expose their name as a <c>public static const</c> called <c>NAME</c></item>
	///         <item>Encapsulate interaction with local or remote services used to fetch and persist model data</item>
	///     </list>
	/// </remarks>
	public interface IModel
	{
		/// <summary>
		/// The Model instance name
		/// </summary>
		string ModelName { get; }

		/// <summary>
		/// The data of the Model
		/// </summary>
		object Data { get; set; }

		/// <summary>
		/// Called by the Model when the Model is registered
		/// </summary>
		void OnRegister();

		/// <summary>
		/// Called by the Model when the Model is removed
		/// </summary>
		void OnRemove();
	}
}
