namespace System.Collections.Generic
{
    public static class ReadOnlyListExtensions
    {
        public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind)
        {
            int i = 0;
            foreach (T element in self)
            {
                if (Equals(element, elementToFind))
                    return i;
                i++;
            }
            return -1;
        }

        public static T Find<T>(this IReadOnlyList<T> self, Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            foreach (var item in self)
            {
                if (match(item))
                    return item;
            }

            return default;
        }

        public static int FindIndex<T>(this IReadOnlyList<T> self, Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            for (var index = 0; index < self.Count; ++index)
            {
                if (match(self[index]))
                    return index;
            }

            return -1;
        }
    }
}
