using System.Collections;
using System.Collections.Generic;

namespace CardEngineConsole.UI
{
    // TODO: if T is Dirtyable, should call the IsDirty.
    public class DirtyableList<T> : IList<T>, IDirtyable
    {
        private readonly List<T> items = new List<T>();

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item)
        {
            items.Add(item);
            IsDirty = true;
        }

        public void Clear()
        {
            if (items.Count > 0)
                IsDirty = true;

            items.Clear();
        }

        public bool Contains(T item) => items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            if (items.Remove(item))
            {
                IsDirty = true;
                return true;
            }

            return false;
        }

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public int IndexOf(T item) => items.IndexOf(item);

        public void Insert(int index, T item)
        {
            items.Insert(index, item);
            IsDirty = true;
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            IsDirty = true;
        }

        public T this[int index]
        {
            get => items[index];
            set
            {
                if (!Equals(items[index], value))
                {
                    items[index] = value;
                    IsDirty = true;
                }
            }
        }

        public bool IsDirty { get; set; }
    }
}
