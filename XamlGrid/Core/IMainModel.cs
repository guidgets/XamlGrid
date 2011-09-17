/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using



#endregion

namespace XamlGrid.Core
{
	/// <summary>
	/// The interface definition for a PureMVC Model
	/// </summary>
	/// <remarks>
	///     <para>In PureMVC, <c>IModel</c> implementors provide access to <c>IModel</c> objects by named lookup</para>
	///     <para>An <c>IModel</c> assumes these responsibilities:</para>
	///     <list type="bullet">
	///         <item>Maintain a cache of <c>IModel</c> instances</item>
	///         <item>Provide methods for registering, retrieving, and removing <c>IModel</c> instances</item>
	///     </list>
	/// </remarks>
	public interface IMainModel
	{
		/// <summary>
		/// Register an <c>IModel</c> instance with the <c>Model</c>
		/// </summary>
		/// <param name="model">A reference to the Model object to be held by the <c>Model</c></param>
		void RegisterModel(IModel model);

		/// <summary>
		/// Retrieve an <c>IModel</c> instance from the Model
		/// </summary>
		/// <param name="ModelName">The name of the Model to retrieve</param>
		/// <returns>The <c>IModel</c> instance previously registered with the given <c>ModelName</c></returns>
		IModel RetrieveModel(string ModelName);

		/// <summary>
		/// Remove an <c>IModel</c> instance from the Model
		/// </summary>
		/// <param name="ModelName">The name of the <c>IModel</c> instance to be removed</param>
		IModel RemoveModel(string ModelName);

		/// <summary>
		/// Check if a Model is registered
		/// </summary>
		/// <param name="ModelName">The name of the Model to check for</param>
		/// <returns>whether a Model is currently registered with the given <c>ModelName</c>.</returns>
		bool HasModel(string ModelName);
	}
}
