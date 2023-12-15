using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Utility {
    public static class EnumerableExtensions {
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> input) {
            return (IEnumerable<T>)input.Where(item => item is not null);
        }
    }
}
