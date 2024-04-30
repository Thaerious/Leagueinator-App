using RaisedEventExample.Controls;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using static RaisedEventExample.Controls.FooButton;

namespace RaisedEventExample {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        /// <summary>
        /// Passing FooClick as an arg to new FooEventHandler creates an instance of FooEventHandler.
        /// Without the delegate instantiation the method #FooClick is an Action[1] delegate with the
        /// same signature of the FooEventHandler delegate.
        /// 
        /// The ActionButton registered an Action delegate instead of a custom delegate so that it matches
        /// the signature of a method; because the signature of the ActionClick method is an Action delegate.
        /// 
        /// [1] System.Action`2[System.Object,RaisedEventExample.Controls.FooButtonArgs]
        /// </summary>

        public MainWindow() {
            InitializeComponent();

            // The button click is handler registered on the this window, the routed event handler is the default
            // handler type.  Because it is a generic button nothing else need be done.
            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(VanillaClick));

            // The CounterButton CountEvent is a custom event using the standard delegate signatrue. It is
            // registered on the stack panel for demonstration. The event handler is the default type.
            // Because the RoutedEventHandler is used inside the CounterClick method RoutedEventArgs needs
            // to checked and cast to CounterButtonArgs.
            this.StackPanel1.AddHandler(CounterButton.CountEvent, new RoutedEventHandler(CounterClick));

            // The FooButton CountEvent was registered with a non-standard delegate.  Because of this in the FooClick
            // method no casting is required.
            this.AddHandler(FooButton.CountEvent, new FooEventHandler(FooClick));

            // This was registered as an Action delegate instead of a generic delegate.  As such the new delgate() is
            // not needed.  This is only to demonstrate the type of a method is not the same as a delegate, and as
            // such needs to be wrapped with new delegate().
            this.AddHandler(ActionButton.CountEvent, ActionClick);
        }

        private void VanillaClick(object sender, RoutedEventArgs e) {
            Debug.WriteLine($"VanillaClick {sender}");
        }

        private void FooClick(object sender, FooButtonArgs e) {
            Debug.WriteLine($"FooClick {sender} {e.Count}");
        }

        private void ActionClick(object sender, ActionButtonArgs e) {
            Debug.WriteLine($"ActionClick {sender} {e.Count}");
        }

        private void CounterClick(object sender, RoutedEventArgs e) {
            if (e is CounterButtonArgs args1) {
                Debug.WriteLine($"CounterClick {sender} {args1.Count}");
            }
        }
    }
}
