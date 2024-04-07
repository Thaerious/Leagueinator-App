using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Leagueinator.VisualUnitTest {
    public partial class Card : UserControl {
        public new EventHandler Click = delegate { };

        public Card() {
            InitializeComponent();
            this.Label.BackColor = Color.Transparent;
            this.Label.Click += (s, e) => this.Click.Invoke(this, e);
            base.Click += (s, e) => this.Click.Invoke(this, e);
            this.ToolTip.SetToolTip(this, "No description provided.");
        }

        public string ToolTipText {
            get => this.ToolTip.GetToolTip(this) ?? "";
            set => this.ToolTip.SetToolTip(this, value);
        }

        public new string Text {
            get => this.Label.Text;
            set => this.Label.Text = value;
        }
    }
}
