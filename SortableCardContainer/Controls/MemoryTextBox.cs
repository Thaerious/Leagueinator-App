using System.Windows.Controls;
using System.Windows.Input;

namespace Leagueinator.Controls {
    public class MemoryTextBox : TextBox {
        public record TextUpdateData (MemoryTextBox Source, string Before, string After, string EventName);
        public delegate void TextUpdateEvent(TextUpdateData data);
        public event TextUpdateEvent UpdateText = delegate { };

        private string Memory { get; set; } = "";

        public new string Text { 
            get => base.Text;
            set {
                base.Text = value;
                this.Memory = value;
            }
        }

        public MemoryTextBox() {
            this.KeyDown += this.OnKeyDown;
            this.LostFocus += this.OnLostFocus;
        }

        public MemoryTextBox(string initialValue) {
            this.KeyDown += this.OnKeyDown;
            this.LostFocus += this.OnLostFocus;
            this.Memory = initialValue;
            this.Text = initialValue;
        }

        private void OnKeyDown(object sender, System.Windows.RoutedEventArgs e) {
            if (e is not KeyEventArgs keyArgs) return;

            if (keyArgs.Key == Key.Enter) {
                var prevMem = this.Memory;
                this.Memory = this.Text;
                this.UpdateText.Invoke(new(this, prevMem, this.Text, "KeyDown"));
            }
        }
        private void OnLostFocus(object sender, System.Windows.RoutedEventArgs e) {
            var prevMem = this.Memory;
            this.Memory = this.Text;
            this.UpdateText.Invoke(new(this, prevMem, this.Text, "LostFocus"));
        }

        public new void Clear() {
            this.Text = "";
            this.Memory = "";
        }
    }
}
