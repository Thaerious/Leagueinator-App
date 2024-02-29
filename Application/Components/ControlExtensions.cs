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
    }
}
