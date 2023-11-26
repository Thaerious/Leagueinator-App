namespace Leagueinator_Utility.Utility {
    public static class WinFormsExt {

        public static Control? FindControlByName(this Control parent, string name) {
            foreach (Control child in parent.Controls) {
                if (child.Name == name) return child;

                // Recursive call for Container controls like Panel, GroupBox, etc.
                Control? foundControl = FindControlByName(child, name);
                if (foundControl != null) return foundControl;
            }

            return default; // Control not found
        }

    }
}
