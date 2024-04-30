using System.Windows;
using System.Windows.Controls;

namespace RaisedEventExample.Controls {
    public class FooButtonArgs(RoutedEvent e, int count) : RoutedEventArgs(e) {
        public int Count {get; init;} = count;
    }

    public class FooButton : Button {
        // The "object sender" is whichever object is handling the event, not the object that generated it.
        // In MainWindow.cs it's the first argument passed into the AddHandler method.
        // Because you do not neccisarily know which type is doing the handling, it is of type 'object'.
        public delegate void FooEventHandler(object sender, FooButtonArgs e);

        // Register a custom routed event using the Bubble routing strategy.
        public static readonly RoutedEvent CountEvent = EventManager.RegisterRoutedEvent(
            name: "CountClick",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(FooEventHandler),
            ownerType: typeof(FooButton)
        );

        public event FooEventHandler CountClick {
            add { AddHandler(CountEvent, value); }
            remove { RemoveHandler(CountEvent, value); }
        }

        private int counter = 0;

        protected override void OnClick() {
            this.counter--;

            // The FooButton.CountEvent is the stored return value from registering the event.
            // The this.counter is the additional information we want to send.
            FooButtonArgs args = new(FooButton.CountEvent, this.counter);
            RaiseEvent(args);
        }
    }
}
