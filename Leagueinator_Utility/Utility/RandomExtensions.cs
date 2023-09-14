using System.Collections.ObjectModel;

namespace Leagueinator_Utility.Utility {
    internal static class RandomExtensions {
        private static readonly Random rng = new();

        public static T SelectRandom<T>(this Collection<T> collection) {
            int r = rng.Next(collection.Count);
            return collection[r];
        }

        public static T SelectRandom<T>(this IList<T> list) {
            int r = rng.Next(list.Count);
            return list[r];
        }

        public static T RemoveRandom<T>(this Collection<T> collection) {
            int r = rng.Next(collection.Count);
            T t = collection[r];
            _ = collection.Remove(t);
            return t;
        }

        public static T? RemoveFrom<T>(this Random rng, Collection<T> collection) {
            if (collection.Count == 0) return default;
            int r = rng.Next(collection.Count);
            T? team = collection[r];
            collection.RemoveAt(r);
            return team;
        }
    }
}
