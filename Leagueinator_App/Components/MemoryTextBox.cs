using System.Diagnostics;

namespace Leagueinator.App.Components {
    public class MemoryTextBox : TextBox {
        public class MemoryUpdateArgs {
            public required string TextBefore { get; init; }
            public required string TextAfter { get; init; }
        }

        public delegate void MemoryUpdateEvent(object? sender, MemoryUpdateArgs e);
        new public event MemoryUpdateEvent TextChanged = delegate { };

        /// <summary>
        /// Update the text and memory values without triggering an event.
        /// </summary>
        public new string Text {
            get => base.Text;
            set {
                this.Memory = value;
                base.Text = value;
            }
        }

        public string Memory {
            get; private set;
        }

        public MemoryTextBox() : base() {
            this.InitializeComponents();
            base.TextChanged += Base_TextChanged;
            this.Memory = this.Text;
        }

        private void Base_TextChanged(object? sender, EventArgs e) {
            if (this.Text == this.Memory) return;

            MemoryUpdateArgs args = new() {
                TextBefore = this.Memory,
                TextAfter = this.Text
            };
            this.Memory = this.Text;

            TextChanged.Invoke(this, args);
        }

        public void InitializeComponents() {
            this.SuspendLayout();
            this.BackColor = Color.WhiteSmoke;
            this.ResumeLayout(false);
        }

        public void Revert() {
            base.Text = this.Memory;
        }
    }
}
