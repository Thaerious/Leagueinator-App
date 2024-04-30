using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Leagueinator.Controls {
    public class MemoryTextBoxArgs(RoutedEvent routedEvent, string before, string after, string cause) : RoutedEventArgs(routedEvent) {
        public string Before { get; private set; } = before;
        public string After { get; private set; } = after;
        public string Cause { get; private set; } = cause;
    }

    public class MemoryTextBox : TextBox {
        public delegate void MemoryEventHandler(object sender, MemoryTextBoxArgs e);

        public static readonly RoutedEvent RegisteredEvent = EventManager.RegisterRoutedEvent(
            "UpdateText",                 // Event name
            RoutingStrategy.Bubble,       // Routing strategy (Bubble, Tunnel, or Direct)
            typeof(MemoryEventHandler),   // Delegate type
            typeof(MemoryTextBox)         // Owner type
        );

        public event MemoryEventHandler UpdateText {
            add { AddHandler(RegisteredEvent, value); }
            remove { RemoveHandler(RegisteredEvent, value); }
        }

        private void RaiseUpdateTextEvent(string before, string after, string cause) {
            MemoryTextBoxArgs newEventArgs = new (RegisteredEvent, before, after, cause);
            RaiseEvent(newEventArgs);
        }

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
                RaiseUpdateTextEvent(prevMem, this.Text, "KeyDown");
            }
        }
        private void OnLostFocus(object sender, System.Windows.RoutedEventArgs e) {
            var prevMem = this.Memory;
            this.Memory = this.Text;

            RaiseUpdateTextEvent(prevMem, this.Text, "LostFocus");
        }

        public new void Clear() {
            this.Text = "";
            this.Memory = "";
        }
    }
}
