using System.Collections.ObjectModel;

namespace Leagueinator.Utility {
    public static class CollectionExtensions {

        public static List<T> AddUnique<T>(this List<T> list, T t) {
            if (t == null) return list;
            if (!list.Contains(t)) list.Add(t);
            return list;
        }

        public static List<T> AddUnique<T>(this List<T> list, ICollection<T> that) {
            foreach (T t in that) list.AddUnique(t);
            return list;
        }

        public static List<T> Unique<T>(this List<T> list) {
            var newList = new List<T>();
            foreach (T t in list) newList.AddUnique(t);
            list.Clear();
            list.AddRange(newList);
            return list;
        }

        public static List<T> NotNull<T>(this ICollection<T?> list) {
            var newList = new List<T>();
            foreach (T? t in list) if (t != null) newList.Add(t);
            return newList;
        }

        public static List<T> NotNull<T>(this IEnumerable<T?> list) {
            var newList = new List<T>();
            foreach (T? t in list) if (t != null) newList.Add(t);
            return newList;
        }

        public static T? Prev<T>(this ICollection<T> collection, T target) {
            var prev = default(T);
            foreach (T t in collection) {
                if (t == null) continue;
                if (t.Equals(target)) return prev;
                prev = t;
            }
            throw new NoSuchElementException();
        }

        public static T? Next<T>(this ICollection<T> list, T target) {
            var prev = default(T);
            foreach (T t in list) {
                if (prev != null && prev.Equals(target)) return t;
                prev = t;
            }
            return default;
        }

        public static bool IsEmpty<T>(this List<T> collection) {
            return collection.Count == 0;
        }

        public static bool IsNotEmpty<T>(this List<T> collection) {
            return collection.Count != 0;
        }

        public static void Replace<T>(this Collection<T> list, T target, T with) {
            int indexOfTarget = list.IndexOf(target);
            if (indexOfTarget == -1) throw new NullReferenceException();
            list[indexOfTarget] = with;
        }
    }
}
