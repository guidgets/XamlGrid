using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace Company.DataGrid.Base.Model.Collections
{
    public abstract class BindableList<T> : Collection<T>, IList<T>, IEventListener, IItemNotifyPropertyChanged where T : INotifyPropertyChanged
    {
        private List<T> items;
        public event PropertyChangedEventHandler ItemPropertyChanged;
        public BindableList()
        {
            this.EventManager = new WeakEventManager(this);
            this.items = new List<T>();
        }

        public WeakEventManager EventManager
        {
            get;
            protected set;
        }

        public bool ReceiveWeakEvent(object sender, EventArgs e)
        {
            PropertyChangedEventArgs args = e as PropertyChangedEventArgs;
            ItemPropertyChangedOverride(sender, args);
            if (this.ItemPropertyChanged != null)
                this.ItemPropertyChanged(sender, args);
            return true;
        }

        public virtual void ItemPropertyChangedOverride(object item, PropertyChangedEventArgs e)
        {

        }

        public int IndexOf(T item)
        {
            return this.items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.items.Insert(index, item);
            this.EventManager.StartListening(item);
        }

        public void RemoveAt(int index)
        {
            this.EventManager.StopListening(this.items[index]);
            this.items.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return this.items[index];
            }
            set
            {
                this.EventManager.StopListening(this.items[index]);
                this.items[index] = value;
                this.EventManager.StartListening(value);
            }
        }

        public void Add(T item)
        {
            this.items.Add(item);
            this.EventManager.StartListening(item);
        }

        public void Clear()
        {
            foreach (var item in this.items)
            {
                this.EventManager.StopListening(item);
            }
            this.items.Clear();
        }

        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.items.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<T>)this.items).IsReadOnly; }
        }

        public bool Remove(T item)
        {
            this.EventManager.StopListening(item);
            return this.items.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public interface IEventListener
    {
        WeakEventManager EventManager { get; }
        bool ReceiveWeakEvent(object sender, EventArgs e);
    }

    public interface IItemNotifyPropertyChanged
    {
        event PropertyChangedEventHandler ItemPropertyChanged;
    }

    public class WeakEventManager
    {
        WeakReference listener;

        public WeakEventManager(IEventListener listener)
        {
            this.listener = new WeakReference(listener);
        }

        public void StartListening(INotifyPropertyChanged source)
        {
            source.PropertyChanged += new PropertyChangedEventHandler(DispatchEvent);
        }

        public void StopListening(INotifyPropertyChanged source)
        {
            source.PropertyChanged -= new PropertyChangedEventHandler(DispatchEvent);
        }

        protected void DispatchEvent(object sender, PropertyChangedEventArgs e)
        {
            if (listener.IsAlive)
                ((IEventListener)this.listener.Target).ReceiveWeakEvent(sender, e);
        }
    }
}
