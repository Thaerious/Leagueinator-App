using Leagueinator.Model;

namespace LLeagueinator.App.Forms.Main {

    /// <summary>
    /// Converts arrays where the pairings are next to each other, to 
    /// the round robin encoding where pairings are opposite each other.
    /// 
    /// Adjacent: 1, 1, 2, 2, 3, 3
    /// Opposite: 1, 2, 3, 3, 2, 1
    /// 
    /// </summary>
    public static class RRArrayExtensions {
        /// <summary>
        /// Create an opposite pairing array from an adjacent array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_before"></param>
        /// <returns></returns>
        public static List<T> ToOpposite<T>(this List<T> _before) {            
            T[] after = new T[_before.Count];
            T[] before = _before.ToArray();

            int i = 0;
            int j = after.Length - 1;
            int idx = 0;
            while (idx < before.Length) {
                if (i == j) {
                    after[i] = _before[before.Length - 1];
                    break;
                }
                after[i++] = _before[idx++];
                after[j--] = _before[idx++];
            }

            return after.ToList();
        }


        /// <summary>
        /// Create an adjacent pairing array from an opposite array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_before"></param>
        /// <returns></returns>
        public static List<T> ToAdjacent<T>(this List<T> _before) {
            T[] after = new T[_before.Count];
            T[] before = _before.ToArray();

            int i = 0;
            int j = after.Length - 1;
            int idx = 0;
            while (idx < before.Length) {
                if (i == j) {
                    after[after.Length - 1] = _before[i];
                    break;
                }
                after[idx++] = _before[i++];
                after[idx++] = _before[j--];
            }
            return after.ToList();
        }

        public static List<T> Shift<T>(this List<T> before) {
            List<T> after = new List<T>(before);
            if (after.Count < 2) return after;

            T removed = after[1];
            after.RemoveAt(1);
            after.Add(removed);

            return after;
        }
    }
}
