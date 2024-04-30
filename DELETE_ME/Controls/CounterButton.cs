using System.Windows;
using System.Windows.Controls;

namespace RaisedEventExample.Controls {
    public class CounterButtonArgs(RoutedEvent e, int count) : RoutedEventArgs(e) {
        public int Count {get; init;} = count;
    }

    public class CounterButton : Button {
        // Register a custom routed event using the Bubble routing strategy.
        public static readonly RoutedEvent CountEvent = EventManager.RegisterRoutedEvent(
            name: "CountClick",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(CounterButton)
        );

        public event RoutedEventHandler CountClick {
            add { AddHandler(CountEvent, value); }
            remove { RemoveHandler(CountEvent, value); }
        }

        private int counter = 0;

        protected override void OnClick() {
            this.counter++;
            CounterButtonArgs args = new(CounterButton.CountEvent, this.counter);
            RaiseEvent(args);
        }
    }
}
