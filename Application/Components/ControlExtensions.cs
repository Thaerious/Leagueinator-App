using System.Reflection;
using System.Windows.Forms;

namespace Leagueinator.Components {
    public static class ControlExtensions {
        public static IEnumerable<T> GetControls<T>(this Control parent) where T : Control {
            // Check each control in parent
            foreach (Control control in parent.Controls) {
                // If it is of the specified type, yield return
                if (control is T t) {
                    yield return t;
                }

                // If the control has child controls, recursively call this method
                if (control.HasChildren) {
                    foreach (T childControl in control.GetControls<T>()) {
                        yield return childControl;
                    }
                }
            }
        }

        public static IEnumerable<T> GetControls<T>(this Control parent, string prop, object value) where T : Control {
            return parent.GetControls<T>()
                .Where(control => control.GetType().GetProperty(prop) != null)
                .Where(control => {
                    PropertyInfo pinfo = control.GetType().GetProperty(prop)!;
                    return pinfo.GetValue(control)!.Equals(value);
                });
        }
    }
}
