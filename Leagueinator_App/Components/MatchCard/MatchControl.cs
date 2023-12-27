using System.Diagnostics;

namespace Leagueinator_App.Components.MatchCard {
    public partial class MatchControl : UserControl {
        public MatchControl() {
            InitializeComponent();
            this.Layout += new LayoutEventHandler(SelfLayout);
        }

        public void AddPlayer(int teamIndex) {
            var panel = this.Controls[$"panel{teamIndex}"];
            panel.Controls.Add(new TextBox());
        }

        private void SelfLayout(object? sender, LayoutEventArgs e) {
            Debug.WriteLine($"{this.Name}.SelfLayout");
            int right = 0;
            int maxHeight = 0;

            foreach (Control control in this.Controls) {
                control.Left = right + control.Margin.Left;
                right = control.Right + control.Margin.Right;

                if (control.Height > maxHeight) { maxHeight = control.Height; }

                control.Top = this.Padding.Top;
            }

            this.Width = right;
            this.Height = maxHeight + Padding.Top + Padding.Bottom;
        }
    }

    public class PlayerPanel : Panel {

        public PlayerPanel() {
            this.Layout += new LayoutEventHandler(SelfLayout);
        }

        private void SelfLayout(object? sender, LayoutEventArgs e) {
            Debug.WriteLine($"{this.Name}.SelfLayout");

            foreach (Control control in this.Controls) {
                control.Width = this.Width;
                this.Height += control.Height;
            }
        }
    }
}
