namespace Leagueinator.Utility {
    public static class ArrayExtensions {
        public static T[] Populate<T>(this T[] array, Func<T> provider) {
            for (int i = 0; i < array.Length; i++) {
                array[i] = provider();
            }
            return array;
        }

        public static T[] Fill<T>(this T[] array, T value) {
            for (int i = 0; i < array.Length; i++) {
                array[i] = value;
            }
            return array;
        }

        public static int IndexOf<T>(this T[] array, T value) {
            for (int i = 0; i < array.Length; i++) {
                if (Equals(array[i], value)) return i;
            }
            return -1;
        }

        public static bool Has<T>(this T[] array, T value) {
            return array.IndexOf(value) >= 0;
        }

        public static T? Last<T>(this T[] array) {
            return array.Length == 0 ? default : array[^1];
        }

        public static T? First<T>(this T[] array) {
            return array.Length == 0 ? default : array[0];
        }

        public static List<T> NotNull<T>(this T[] array) {
            var list = new List<T>();
            foreach (T t in array) {
                if (t == null) continue;
                if (t.Equals(default(T)) == false) list.Add(t);
            }
            return list;
        }

        public static void Shuffle<T>(this Random rng, T[] array) {
            int n = array.Length;
            while (n > 1) {
                int k = rng.Next(n--);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }

        public static bool Contains<T>(this T[] array, T target) {
            return array.IndexOf(target) >= 0;
        }

        public static bool IsFull<T>(this T[] array) {
            return array.NotNull().Count == array.Length;
        }

        public static bool IsEmpty<T>(this T[] array) {
            return array.NotNull().Count == 0;
        }

        public static bool IsNotEmpty<T>(this T[] array) {
            return array.NotNull().Count > 0;
        }

        public static bool IsNotFull<T>(this T[] array) {
            return array.NotNull().Count < array.Length;
        }
    }
}
