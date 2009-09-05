﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents a VisualStateManager that is able to directly affect the control in which template it is used.
	/// To employ this functionality, the Storyboard.TargetName property of an animation needs simply to be skipped or set to the empty string.
	/// </summary>
	public class CustomVSM : VisualStateManager
	{
		/// <summary>
		/// Transitions a control between states.
		/// </summary>
		/// <param name="control">The control to transition between states.</param>
		/// <param name="templateRoot">The root element of the control's <see cref="T:System.Windows.Controls.ControlTemplate"/>.</param>
		/// <param name="stateName">The name of the state to transition to.</param>
		/// <param name="group">The <see cref="T:System.Windows.VisualStateGroup"/> that the state belongs to.</param>
		/// <param name="state">The representation of the state to transition to.</param>
		/// <param name="useTransitions">true to use a <see cref="T:System.Windows.VisualTransition"/> to transition between states; otherwise, false.</param>
		/// <returns>
		/// true if the control successfully transitioned to the new state; otherwise, false.
		/// </returns>
		protected override bool GoToStateCore(Control control, FrameworkElement templateRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
		{
			if (state == null)
			{
				return false;
			}
			if (state.Storyboard == null || state.Storyboard.GetCurrentState() != ClockState.Stopped || 
				!string.IsNullOrEmpty(Storyboard.GetTargetName(state.Storyboard)))
			{
				return base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
			}
			foreach (Timeline timeline in state.Storyboard.Children)
			{
				if (string.IsNullOrEmpty(Storyboard.GetTargetName(timeline)))
				{
					Storyboard.SetTarget(timeline, control);
				}
			}
			return base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
		}
	}
}