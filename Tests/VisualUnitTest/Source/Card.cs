using System.ComponentModel;

namespace Leagueinator.VisualUnitTest {
    public partial class Card : UserControl {
        public new EventHandler Click = delegate { };
        public new MouseEventHandler MouseDown = delegate { };
        private int HoverCount = 0;

        public Card() {
            InitializeComponent();
            this.LabelDisplayText.BackColor = Color.Transparent;

            this.LabelDisplayText.Click += (s, e) => this.Click.Invoke(this, e);
            base.Click += (s, e) => this.Click.Invoke(this, e);

            this.LabelDisplayText.MouseDown += (s, e) => this.MouseDown.Invoke(this, e);
            base.MouseDown += (s, e) => this.MouseDown.Invoke(this, e);

            this.ToolTip.SetToolTip(this, "No description provided.");
            this.Text = "NOT SET";

            foreach (Control child in this.Controls) {
                child.MouseEnter += this.HndMouseEnter;
                child.MouseLeave += this.HndMouseLeave;
            }

            this.MouseEnter += this.HndMouseEnter;
            this.MouseLeave += this.HndMouseLeave;
        }

        public Color IdleColor {
            get => _idleColor;
            set {
                _idleColor = value;
                if (this.HoverCount == 0) this.BackColor = value;
            }
        }
        private void HndMouseEnter(object? sender, EventArgs e) {
            if (HoverCount++ == 0) this.BackColor = this.HoverColor;
        }

        private void HndMouseLeave(object? sender, EventArgs e) {
            if (--HoverCount == 0) this.BackColor = this.IdleColor;
        }

        [Category("Appearance")]
        public Color HoverColor { get; set; } = Color.FromArgb(210, 210, 230);

        public string ToolTipText {
            get => this.ToolTip.GetToolTip(this) ?? "";
            set => this.ToolTip.SetToolTip(this, value);
        }

        public new string Text {
            get => this.LabelDisplayText.Text;
            set => this.LabelDisplayText.Text = value;
        }

        private Color _idleColor = SystemColors.Control;
    }
}
