using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CardEngineConsole.UI
{
    class ConsoleBase : INotifyPropertyChanged, IDirtyable
    {
        private class Item
        {
            public object Value { get; set; }
            public virtual bool IsDirty { get; set; }
        }

        private class DirtyableItem : Item
        {
            public override bool IsDirty
            {
                get => ((IDirtyable) Value).IsDirty;
                set => ((IDirtyable) Value).IsDirty = value;
            }
        }

        private readonly Dictionary<string, Item> values = new Dictionary<string, Item>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsDirty
        {
            get { return values.Any(x => x.Value.IsDirty); }
            set
            {
                foreach (var item in values)
                    item.Value.IsDirty = value;
            }
        }

        protected void SetProperty<T>(T value, [CallerMemberName] string key = null)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (!values.ContainsKey(key))
            {
                values[key] = typeof(IDirtyable).IsAssignableFrom(typeof(T))
                    ? new DirtyableItem {Value = value}
                    : new Item {Value = value, IsDirty = true};

                OnPropertyChanged(key);
            }
            else if (!Equals(values[key].Value, value))
            {
                var item = values[key];
                item.Value = value;
                item.IsDirty = true;
                OnPropertyChanged(key);
            }
        }

        protected T GetProperty<T>([CallerMemberName] string key = null)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (values.TryGetValue(key, out var item))
                return (T)item.Value;

            return default;
        }
    }
}
