using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Printer {
    public interface IHasValue {
        bool HasValue { get; }
    }

    public interface IHasDefault<T> {
        public static readonly T? Default = default;
    }
}
