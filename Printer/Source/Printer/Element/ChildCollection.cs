using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Printer.Elements {
    public class ChildCollection(IList<Element> list) : ReadOnlyCollection<Element>(list) {
    }
}
