using System;
using System.Collections.Generic;
using System.Text;

namespace RosDBG
{
    /// <summary>
    /// A generic class to represent a variable that remembers its previous value.
    /// </summary>
    /// <typeparam name="T">Data type to encapsulate</typeparam>
    class StatefulVariable<T> where T: IComparable
    {
        public T PreviousValue { get; private set; }
        public T CurrentValue { get; private set; }
        public bool HasChanged { get { return PreviousValue.CompareTo(CurrentValue) != 0; } }

        public event EventHandler Updated; // Always invoked even if overwritten with same value
        public event EventHandler Modified; // Only invoked if new value different than before

        public StatefulVariable(): this(default(T)) {}

        public StatefulVariable(T initialValue)
        {
            PreviousValue = initialValue;
            CurrentValue = PreviousValue;
        }

        public void Set(T newValue)
        {
            PreviousValue = CurrentValue;
            CurrentValue = newValue;

            if (HasChanged && Modified != null)
                Modified.Invoke(this, EventArgs.Empty);

            if (Updated != null)
                Updated.Invoke(this, EventArgs.Empty);
        }
    }
}
