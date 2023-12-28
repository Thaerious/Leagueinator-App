using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.App {
    public static class ControlExtensions {

        public static T? Parent<T>(this Control? control) where T : Control {
            while (control != null) {
                if (control is T parent) return parent;
                control = control.Parent;
            }
            return null; // No parent found of the specified type
        }

    }
}
