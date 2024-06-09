using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Leagueinator.Controls {
    public enum Cause { EnterPressed, LostFocus, TextDropped }

    public class MemoryTextBoxArgs(RoutedEvent routedEvent, MemoryTextBox textBox, string before, string after, Cause cause) : RoutedEventArgs(routedEvent) {
        public MemoryTextBox TextBox { get; init; } = textBox;
        public string Before { get; init; } = before;
        public string After { get; init; } = after;
        public Cause Cause { get; init; } = cause;
    }

    public class MemoryTextBox : TextBox {
        public delegate void MemoryEventHandler(object sender, MemoryTextBoxArgs e);
        public bool CaseSensitive = false;

        public static readonly RoutedEvent RegisteredUpdateEvent = EventManager.RegisterRoutedEvent(
            "UpdateText",                 // Event name
            RoutingStrategy.Bubble,       // Routing strategy (Bubble, Tunnel, or Direct)
            typeof(MemoryEventHandler),   // Delegate type
            typeof(MemoryTextBox)         // Owner type
        );

        public event MemoryEventHandler UpdateText {
            add { AddHandler(RegisteredUpdateEvent, value); }
            remove { RemoveHandler(RegisteredUpdateEvent, value); }
        }


        private void RaiseUpdateTextEvent(string before, string after, Cause cause) {
            MemoryTextBoxArgs newEventArgs = new(RegisteredUpdateEvent, this, before, after, cause);
            RaiseEvent(newEventArgs);
        }

        public MemoryTextBox() {
            this.KeyDown += this.OnKeyDown;
            this.LostFocus += this.OnLostFocus;
            this.PreviewDrop += this.OnPreviewDrop;
            this.PreviewDragLeave += this.OnPreviewDragLeave;
        }

        public MemoryTextBox(string initialValue) : this() {
            this.Memory = initialValue;
            this.Text = initialValue;
        }

        private string Memory { get; set; } = "";

        public new string Text {
            get => base.Text;
            set {
                base.Text = value;
                this.Memory = value;
            }
        }

        public new void Clear() {
            this.Memory = "";
            this.Text = "";
        }

        private void OnKeyDown(object sender, System.Windows.RoutedEventArgs e) {
            if (e is not KeyEventArgs keyArgs) return;

            if (keyArgs.Key == Key.Enter) {
                string prevMem = this.Memory;
                this.Memory = this.Text;

                if (!this.Compare(prevMem, this.Text)) {
                    RaiseUpdateTextEvent(prevMem, this.Text, Cause.EnterPressed);
                }
            }
        }

        /// <summary>
        /// Set text to previous values to clarify incomplete entries.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewDragLeave(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.Text)) {
                string dragText = (string)e.Data.GetData(DataFormats.Text);
                Debug.WriteLine($"OnPreviewDragLeave {dragText}");
                this.Text = this.Memory;
            }
        }

        private void OnPreviewDrop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.Text)) {
                string droppedText = (string)e.Data.GetData(DataFormats.Text);

                string prevMem = this.Memory;
                this.Text = droppedText;
                this.Memory = droppedText;
                e.Handled = true;

                if (!this.Compare(prevMem, this.Text)) {
                    RaiseUpdateTextEvent(prevMem, this.Text, Cause.TextDropped);
                }
            }

        }

        private void OnLostFocus(object sender, System.Windows.RoutedEventArgs e) {
            string prevMem = this.Memory;
            this.Memory = this.Text;


            if (!this.Compare(prevMem, this.Text)) {
                RaiseUpdateTextEvent(prevMem, this.Text, Cause.LostFocus);
            }
        }

        private bool Compare(string prevMem, string Text) {
            if (this.CaseSensitive) return prevMem.Equals(Text);
            return prevMem.ToLower().Equals(Text.ToLower());
        }
    }
}
