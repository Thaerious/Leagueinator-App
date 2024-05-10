﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Leagueinator.Controls {
    public enum Cause { EnterPressed, LostFocus }

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
            MemoryTextBoxArgs newEventArgs = new (RegisteredUpdateEvent, this, before, after, cause);
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

        public new void Clear() {
            this.Text = "";
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
                string prevMem = this.Memory;
                this.Memory = this.Text;      

                if (!this.Compare(prevMem, this.Text)) {
                    RaiseUpdateTextEvent(prevMem, this.Text, Cause.EnterPressed);
                }
            }
        }
        private void OnLostFocus(object sender, System.Windows.RoutedEventArgs e) {
            string prevMem = this.Memory;
            this.Memory = this.Text;

            
            if (!this.Compare(prevMem, this.Text)) {
                Debug.WriteLine($"RaiseUpdateTextEvent {this.GetHashCode()} '{prevMem}' '{this.Text}' '{Cause.LostFocus}'");
                RaiseUpdateTextEvent(prevMem, this.Text, Cause.LostFocus);
            }
        }

        private bool Compare(string prevMem, string Text) {
            if (this.CaseSensitive) return prevMem.Equals(Text);
            return prevMem.ToLower().Equals(Text.ToLower());
        }
    }
}