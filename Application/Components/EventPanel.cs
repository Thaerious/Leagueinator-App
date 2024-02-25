using System.ComponentModel;

namespace Leagueinator.Components {
    public partial class EventPanel : UserControl {
        private Controller? _controller;

        public EventPanel() {
            InitializeComponent();            
        }

        [Category("Custom Properties")]
        [Description("Model Controller.")]
        public Controller? Controller {
            get => _controller;
            set {
                if (_controller == value) return;
                _controller = value;
                if (value == null) return;

                this.matchCard1.Controller = this.Controller;
            }
        }

        private void LayoutEventHander(object sender, LayoutEventArgs e) {
            foreach (Control child in this.flowLayoutPanel1.Controls) {
                child.Width = this.flowLayoutPanel1.ClientSize.Width
                            - this.flowLayoutPanel1.Padding.Horizontal
                            - child.Margin.Horizontal;
            }
        }
    }
}
