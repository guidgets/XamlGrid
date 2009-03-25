using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Company.DataGrid.View
{
	/// <summary>
	/// Moves a cell between edited and unedited state.
	/// </summary>
	public class CellVSM : VisualStateManager
	{
		private object editor;

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
		protected override bool GoToStateCore(Control control, FrameworkElement templateRoot, string stateName,
		                                      VisualStateGroup group, VisualState state, bool useTransitions)
		{
			if (state == null)
			{
				return false;
			}
			if (state.Storyboard == null)
			{
				return base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
			}
			// TODO: lookful: counts on specific animations
			ObjectKeyFrame objectKeyFrame = ((ObjectAnimationUsingKeyFrames) state.Storyboard.Children[0]).KeyFrames[0];
			if (this.editor == null)
			{
				this.editor = objectKeyFrame.Value;
			}
			return base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
		}
	}
}