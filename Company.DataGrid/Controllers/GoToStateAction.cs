// GoToState.cs
// Copyright (c) Nikhil Kothari, 2008. All Rights Reserved.
// http://www.nikhilk.net
//
// Silverlight.FX is an application framework for building RIAs with Silverlight.
// This project is licensed under the BSD license. See the accompanying License.txt
// file for more information.
// For updated project information please visit http://projects.nikhilk.net/SilverlightFX.
//

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Company.Widgets.Views;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// An action that transitions from one visual state to another.
	/// </summary>
	[ContentProperty("LockedStates")]
	public class GoToStateAction : TriggerAction<FrameworkElement>
	{
		/// <summary>
		/// Identifies the dependency property which gets or sets the name of the state to navigate to.
		/// </summary>
		public static readonly DependencyProperty StateNameProperty =
			DependencyProperty.Register("StateName", typeof(string), typeof(GoToStateAction), null);

		/// <summary>
		/// Identifies the dependency property which gets or sets whether the state navigation should be accompanied by a transition.
		/// </summary>
		public static readonly DependencyProperty UseTransitionProperty =
			DependencyProperty.Register("UseTransition", typeof(bool), typeof(GoToStateAction), new PropertyMetadata(true));

		private static readonly DependencyProperty lockedStatesProperty =
			DependencyProperty.Register("LockedStates", typeof(List<string>), typeof(GoToStateAction), null);

		private Control control;
		private IList<VisualStateGroup> visualStateGroups;


		/// <summary>
		/// An action that transitions from one visual state to another.
		/// </summary>
		public GoToStateAction()
		{
			this.SetValue(lockedStatesProperty, new List<string>());
		}


		/// <summary>
		/// Gets or sets the name of the state to navigate to.
		/// </summary>
		public virtual string StateName
		{
			get
			{
				return (string) this.GetValue(StateNameProperty);
			}
			set
			{
				this.SetValue(StateNameProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets whether the state navigation should be accompanied by a transition.
		/// </summary>
		public virtual bool UseTransition
		{
			get
			{
				return (bool) this.GetValue(UseTransitionProperty);
			}
			set
			{
				this.SetValue(UseTransitionProperty, value);
			}
		}

		/// <summary>
		/// Gets the locked <see cref="VisualState"/>s, that is, states that must not be left for another state in the same <see cref="VisualStateGroup"/>.
		/// </summary>
		public virtual List<string> LockedStates
		{
			get
			{
				return (List<string>) this.GetValue(lockedStatesProperty);
			}
		}


		/// <summary>
		/// Called when the action is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		protected override void OnDetach()
		{
			this.control = null;
			this.visualStateGroups = null;

			base.OnDetach();
		}

		protected override void InvokeAction(System.EventArgs e)
		{
			this.FindTargetControl();
			if (this.LockedStates.Count > 0)
			{
				this.FindVisualGroups();
				VisualStateGroup stateGroup = (from visualStateGroup in this.visualStateGroups
				                               where ((IList<VisualState>) visualStateGroup.States).Any(v => v.Name == this.StateName)
				                               select visualStateGroup).First();
				if ((from state in this.LockedStates
				     where stateGroup.CurrentState != null && stateGroup.CurrentState.Name == state
				     select state).Any())
				{
					return;
				}
			}
			VisualStateManager.GoToState(this.control, this.StateName, this.UseTransition);
		}


		private void FindTargetControl()
		{
			if (this.control != null)
			{
				return;
			}
			FrameworkElement parent = this.AssociatedObject;

			while (parent != null)
			{
				Control parentControl = parent as Control;
				if (parentControl != null)
				{
					this.control = parentControl;
					return;
				}
				parent = parent.GetParent() as FrameworkElement;
			}
		}

		private void FindVisualGroups()
		{
			if (this.visualStateGroups != null)
			{
				return;
			}
			FrameworkElement parent = this.AssociatedObject;

			while (parent != null)
			{
				if (this.visualStateGroups == null)
				{
					IList groups = VisualStateManager.GetVisualStateGroups(parent);
					if (groups.Count > 0)
					{
						this.visualStateGroups = (IList<VisualStateGroup>) groups;
						return;
					}
				}
				parent = parent.GetParent() as FrameworkElement;
			}
		}
	}
}
