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
    public enum Status { PENDING, PASS, FAIL, UNTESTED, NO_TEST, NOT_SET }

    public partial class TestCard : Card {

        public new EventHandler Click = delegate { };

        public Status Status {
            get => _status;
            set {
                _status = value;
                switch (value) {
                    case Status.PASS:
                        this.ButtonPass.BackColor = Color.Lime;
                        this.ButtonPass.Text = "✓";
                        this.ButtonFail.BackColor = SystemColors.Control;
                        this.ButtonFail.Enabled = true;
                        break;
                    case Status.FAIL:
                        this.ButtonPass.BackColor = SystemColors.Control;
                        this.ButtonPass.Text = "✓";
                        this.ButtonFail.BackColor = Color.DeepPink;
                        this.ButtonFail.Enabled = true;
                        break;
                    case Status.UNTESTED:
                    case Status.NOT_SET:
                        this.ButtonPass.BackColor = SystemColors.Control;
                        this.ButtonPass.Text = "✓";
                        this.ButtonFail.BackColor = SystemColors.Control;
                        this.ButtonFail.Enabled = true;
                        break;
                    case Status.NO_TEST:
                        this.ButtonPass.BackColor = Color.SkyBlue;
                        this.ButtonPass.Text = "?";
                        this.ButtonFail.BackColor = SystemColors.ControlDark;
                        this.ButtonFail.Enabled = false;
                    break;
                    case Status.PENDING:
                        this.ButtonPass.BackColor = Color.SkyBlue;
                        this.ButtonPass.Text = "?";
                        this.ButtonFail.BackColor = Color.SkyBlue;
                        this.ButtonFail.Text = "?";
                        break;
                }
            }
        }

        public TestCard() {
            InitializeComponent();
            this.Label.BackColor = Color.Transparent;
            this.Label.Click += (s, e) => this.Click.Invoke(this, e);
            base.Click += (s, e) => this.Click.Invoke(this, e);

            this.ToolTip.SetToolTip(this, "No description provided.");
        }

        private Status _status = Status.NOT_SET;
    }
}
