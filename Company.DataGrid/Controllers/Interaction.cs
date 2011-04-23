// Interaction.cs
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

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// A class providing various attached properties for creating interactivty.
	/// </summary>
	public static class Interaction
	{

		/// <summary>
		/// Represents the Action attached property.
		/// </summary>
		public static readonly DependencyProperty ActionProperty =
			DependencyProperty.RegisterAttached("Action", typeof(TriggerAction), typeof(Interaction), null);

		/// <summary>
		/// Represents the Behaviors attached property.
		/// </summary>
		public static readonly DependencyProperty BehaviorsProperty =
			DependencyProperty.RegisterAttached("Behaviors", typeof(BehaviorCollection), typeof(Interaction), null);

		/// <summary>
		/// Represents the Command attached property.
		/// </summary>
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.RegisterAttached("Command", typeof(string), typeof(Interaction), null);

		/// <summary>
		/// Represents the Triggers attached property.
		/// </summary>
		public static readonly DependencyProperty TriggersProperty =
			DependencyProperty.RegisterAttached("Triggers", typeof(TriggerCollection), typeof(Interaction), null);

		/// <summary>
		/// Represents the VisualState attached property.
		/// </summary>
		public static readonly DependencyProperty VisualStateProperty =
			DependencyProperty.RegisterAttached("VisualState", typeof(string), typeof(Interaction),
												new PropertyMetadata(OnVisualStateChanged));


		/// <summary>
		/// Gets the collection of behaviors for the specified DependencyObject.
		/// </summary>
		/// <param name="o">The DependencyObject to lookup.</param>
		/// <returns>The collection of associated behaviors.</returns>
		public static BehaviorCollection GetBehaviors(DependencyObject o)
		{
			BehaviorCollection behaviors = (BehaviorCollection) o.GetValue(BehaviorsProperty);
			if (behaviors == null)
			{
				behaviors = new BehaviorCollection(o);
				SetBehaviors(o, behaviors);
			}

			return behaviors;
		}

		/// <summary>
		/// Gets the collection of triggers for the specified DependencyObject.
		/// </summary>
		/// <param name="o">The DependencyObject to lookup.</param>
		/// <returns>The collection of associated triggers.</returns>
		public static TriggerCollection GetTriggers(DependencyObject o)
		{
			TriggerCollection triggers = (TriggerCollection) o.GetValue(TriggersProperty);
			if (triggers == null)
			{
				triggers = new TriggerCollection(o);
				SetTriggers(o, triggers);
			}

			return triggers;
		}

		/// <summary>
		/// Gets the visual state associated with the specified control.
		/// </summary>
		/// <param name="control">The control to lookup.</param>
		/// <returns>The current visual state as it was last set.</returns>
		public static string GetVisualState(Control control)
		{
			return (string) control.GetValue(VisualStateProperty);
		}

		private static void OnVisualStateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			string newState = (string) e.NewValue;
			if (String.IsNullOrEmpty(newState) == false)
			{
				VisualStateManager.GoToState((Control) o, newState, /* useTransitions */ true);
			}
		}

		/// <summary>
		/// Sets the collection of behaviors for the specified DependencyObject.
		/// </summary>
		/// <param name="o">The DependencyObject to set.</param>
		/// <param name="behaviors">The collection of behaviors to associate.</param>
		public static void SetBehaviors(DependencyObject o, BehaviorCollection behaviors)
		{
			o.SetValue(BehaviorsProperty, behaviors);
		}

		/// <summary>
		/// Sets the collection of triggers for the specified DependencyObject.
		/// </summary>
		/// <param name="o">The DependencyObject to set.</param>
		/// <param name="triggers">The collection of triggers to associate.</param>
		public static void SetTriggers(DependencyObject o, TriggerCollection triggers)
		{
			o.SetValue(TriggersProperty, triggers);
		}

		/// <summary>
		/// Sets the current visual state of the specified control.
		/// </summary>
		/// <param name="control">The control to set.</param>
		/// <param name="state">The name of the state that the control should be in.</param>
		public static void SetVisualState(Control control, string state)
		{
			control.SetValue(VisualStateProperty, state);
		}
	}
}
