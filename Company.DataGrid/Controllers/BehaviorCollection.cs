// BehaviorCollection.cs
// Copyright (c) Nikhil Kothari, 2008. All Rights Reserved.
// http://www.nikhilk.net
//
// Silverlight.FX is an application framework for building RIAs with Silverlight.
// This project is licensed under the BSD license. See the accompanying License.txt
// file for more information.
// For updated project information please visit http://projects.nikhilk.net/SilverlightFX.
//

using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Company.Widgets.Controllers {

    /// <summary>
    /// Represents a collection of behaviors associated with the same DependencyObject.
    /// </summary>
    public sealed class BehaviorCollection : ObservableCollection<Company.Widgets.Controllers.Behavior> {

        private DependencyObject _associatedObject;

        internal BehaviorCollection(DependencyObject o) {
            _associatedObject = o;
        }

        /// <summary>
        /// Gets the specified type of behavior if it is attached to the element.
        /// </summary>
        /// <typeparam name="TBehavior">The type of behavior to lookup.</typeparam>
        /// <returns>The instance of the behavior if it is present.</returns>
        public TBehavior GetBehavior<TBehavior>() where TBehavior : Company.Widgets.Controllers.Behavior {
            Type behaviorType = typeof(TBehavior);
            foreach (Company.Widgets.Controllers.Behavior b in this) {
                if (behaviorType.IsAssignableFrom(b.GetType())) {
                    return (TBehavior)b;
                }
            }

            return null;
        }

        /// <internalonly />
        protected override void InsertItem(int index, Company.Widgets.Controllers.Behavior item) {
            base.InsertItem(index, item);

            if (item.Owner != null) {
                item.Owner.Remove(item);
            }

            item.Owner = this;
            ((IAttachedObject)item).Attach(_associatedObject);
        }

        /// <internalonly />
        protected override void RemoveItem(int index) {
            Company.Widgets.Controllers.Behavior behavior = this[index];

            ((IAttachedObject)behavior).Detach();
            behavior.Owner = null;

            base.RemoveItem(index);
        }
    }
}
