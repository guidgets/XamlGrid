/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using



#endregion

using Guidgets.XamlGrid.Aspects;

namespace Guidgets.XamlGrid.Core
{
	/// <summary>
	/// A base <c>ICommand</c> implementation
	/// </summary>
	/// <remarks>
	///     <para>Your subclass should override the <c>execute</c> method where your business logic will handle the <c>INotification</c></para>
	/// </remarks>
	/// <see cref="MainCommand"/>
	/// <see cref="Notification"/>
	/// <see cref="MacroCommand"/>
	public class SimpleCommand : Notifier, ICommand
	{
		#region Public Methods

		#region ICommand Members

		/// <summary>
		/// Fulfill the use-case initiated by the given <c>INotification</c>
		/// </summary>
		/// <param name="notification">The <c>INotification</c> to handle</param>
		/// <remarks>
		///     <para>In the Command Pattern, an application use-case typically begins with some user action, which results in an <c>INotification</c> being broadcast, which is handled by business logic in the <c>execute</c> method of an <c>ICommand</c></para>
		/// </remarks>
		[Validate]
		public virtual void Execute([NotNull] INotification notification)
		{
		}

		#endregion

		#endregion
	}
}
