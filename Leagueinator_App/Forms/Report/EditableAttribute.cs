namespace Leagueinator.App.Forms.Report {
    internal class EditableAttribute : Attribute {
        public bool Value { get; private set; }

        public EditableAttribute(bool v) {
            this.Value = v;
        }
    }
}
