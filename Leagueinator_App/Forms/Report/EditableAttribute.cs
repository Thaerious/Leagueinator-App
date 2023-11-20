namespace Leagueinator.Model {
    internal class EditableAttribute : Attribute {
        public bool Value { get; private set; }

        public EditableAttribute(bool v) {
            this.Value = v;
        }
    }
}
