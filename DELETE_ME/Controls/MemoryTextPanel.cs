using System.Windows;
using System.Windows.Controls;

namespace RaisedEventExample.Controls {
    public class MemoryTextPanel : StackPanel {
        public MemoryTextPanel() {
            // This constructor ensures that the panel listens for the UpdateText event
            // bubbled up from any MemoryTextBox children.
            this.AddHandler(MemoryTextBox.UpdateTextEvent, new RoutedEventHandler(OnUpdateText));
        }

        private void OnUpdateText(object sender, RoutedEventArgs e) {
            // Cast the RoutedEventArgs to UpdateTextEventArgs to access specific properties
            UpdateTextEventArgs args = e as UpdateTextEventArgs;
            if (args != null) {
                // Handle the event, e.g., log the change or update a status message
                HandleUpdateText(args.Before, args.After, args.Cause);
            }
        }

        private void HandleUpdateText(string before, string after, string cause) {
            // Implement your logic here to handle the update text event
            // For example, log the text changes or update UI elements accordingly
            Console.WriteLine($"Text changed from '{before}' to '{after}' due to '{cause}'.");
        }
    }
}
