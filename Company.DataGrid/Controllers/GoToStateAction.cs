// GoToState.cs
// Copyright (c) Nikhil Kothari, 2008. All Rights Reserved.
// http://www.nikhilk.net
//
// Silverlight.FX is an application framework for building RIAs with Silverlight.
// This project is licensed under the BSD license. See the accompanying License.txt
// file for more information.
// For updated project information please visit http://projects.nikhilk.net/SilverlightFX.
//

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// An action that transitions from one visual state to another.
	/// </summary>
	public sealed class GoToStateAction : TriggerAction<FrameworkElement>
	{

		/// <summary>
		/// Represents the State property.
		/// </summary>
		public static readonly DependencyProperty StateNameProperty =
			DependencyProperty.Register("StateName", typeof(string), typeof(GoToStateAction), null);

		/// <summary>
		/// Represents the UseTransition property.
		/// </summary>
		public static readonly DependencyProperty UseTransitionProperty =
			DependencyProperty.Register("UseTransition", typeof(bool), typeof(GoToStateAction), new PropertyMetadata(true));

		/// <summary>
		/// Gets or sets the name of the state to navigate to.
		/// </summary>
		public string StateName
		{
			get
			{
				return (string) GetValue(StateNameProperty);
			}
			set
			{
				SetValue(StateNameProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets whether the state navigation should be accompanied with a transition.
		/// </summary>
		public bool UseTransition
		{
			get
			{
				return (bool) GetValue(UseTransitionProperty);
			}
			set
			{
				SetValue(UseTransitionProperty, value);
			}
		}

		/// <internalonly />
		protected override void Invoke(object parameter)
		{
			string stateName = StateName;
			if (string.IsNullOrEmpty(stateName))
			{
				throw new InvalidOperationException("The StateName must be set on a GoToState action.");
			}

			bool hasState = VisualStateManager.GoToState((Control) parameter, stateName, UseTransition);
			if (hasState == false)
			{
				throw new InvalidOperationException("The state named '" + stateName + "' does not exist.");
			}
		}
	}
}
