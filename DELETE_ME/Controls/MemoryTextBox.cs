using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace RaisedEventExample.Controls {
    // Define the custom event arguments class
    public class UpdateTextEventArgs : RoutedEventArgs {
        public string Before { get; private set; }
        public string After { get; private set; }
        public string Cause { get; private set; }

        public UpdateTextEventArgs(RoutedEvent routedEvent, string before, string after, string cause)
            : base(routedEvent) {
            Before = before;
            After = after;
            Cause = cause;
        }
    }

    // Define the MemoryTextBox class
    public class MemoryTextBox : TextBox {
        public MemoryTextBox() {
            this.KeyDown += this.OnKeyDown;
        }

        private void OnKeyDown(object sender, System.Windows.RoutedEventArgs e) {
            if (e is not KeyEventArgs keyArgs) return;
            if (keyArgs.Key == Key.Enter) {
                Debug.WriteLine("OnKeyDown <ENTER>");
                RaiseUpdateTextEvent("OLD_TEXT", this.Text, "keydown");
            }
        }

        // Declare the routed event
        public static readonly RoutedEvent UpdateTextEvent = EventManager.RegisterRoutedEvent(
            "UpdateText", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MemoryTextBox));

        // .NET event wrapper
        public event RoutedEventHandler UpdateText {
            add { AddHandler(UpdateTextEvent, value); }
            remove { RemoveHandler(UpdateTextEvent, value); }
        }

        // Method to raise the UpdateText event
        protected void RaiseUpdateTextEvent(string before, string after, string cause) {
            Debug.WriteLine($"RaiseUpdateTextEvent {before} {after} {cause}");
            UpdateTextEventArgs newEventArgs = new UpdateTextEventArgs(UpdateTextEvent, before, after, cause);
            RaiseEvent(newEventArgs);
        }

        // Example method that could trigger the UpdateText event
        public void ModifyText(string newText, string cause) {
            string oldText = this.Text;
            this.Text = newText;
            RaiseUpdateTextEvent(oldText, newText, cause);
        }
    }
}
