using System.Collections;
using System.Collections.Generic;

namespace CardEngine.State.Events
{
    /// <summary>
    /// Special collection for message pump in the engine.
    /// </summary>
    internal class EventCollection : IEnumerable<BaseEvent>
    {
        private int index = 0;
        private int Active => 1 - index;
        private int Processing => index;
        private readonly List<BaseEvent>[] events = {new List<BaseEvent>(), new List<BaseEvent>()};

        public void Push(BaseEvent @event)
        {
            events[Active].Add(@event);
        }

        public void Clear()
        {
            events[Processing].Clear();
        }

        public void Swap()
        {
            index = 1 - index;
        }

        public IEnumerator<BaseEvent> GetEnumerator()
        {
            return events[Processing].GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
