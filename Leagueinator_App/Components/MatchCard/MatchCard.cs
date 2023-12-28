using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Leagueinator.App.Components {
    public class MatchCard : UserControl {

        new internal MatchCardPanel Parent {
            get {
                if (base.Parent == null) throw new NullReferenceException();
                return (base.Parent as MatchCardPanel)!;
            }

            set { base.Parent = value; }
        }

        public Match Match { get; init; }

        new public readonly static Color DefaultBackColor = Color.FromArgb(0, 120, 0);
        public readonly static Color FocusBackColor = Color.FromArgb(80, 200, 80);

        public MatchCard(Match match) {
            this.Match = match;
            InitializeComponents();
            this.DragEnter += new DragEventHandler(OnDragEnter);
            this.DragLeave += new EventHandler(OnDragLeave);
            this.DragDrop += new DragEventHandler(OnDragDrop);
        }

        private void OnMouseDown(object? sender, MouseEventArgs e) {
            this.Focus();

            this.BackColor = FocusBackColor;

            foreach (MemoryTextBox textBox in this.flowLeft.Controls) textBox.ForeColor = Color.Gray;
            foreach (MemoryTextBox textBox in this.flowRight.Controls) textBox.ForeColor = Color.Gray;

            this.Update();

            this.Parent.Dragging = this;
            this.DoDragDrop(this, DragDropEffects.Copy | DragDropEffects.Move);

            foreach (MemoryTextBox textBox in this.flowLeft.Controls) textBox.ForeColor = Color.Black;
            foreach (MemoryTextBox textBox in this.flowRight.Controls) textBox.ForeColor = Color.Black;
        }

        private void OnDragEnter(object? _sender, DragEventArgs e) {
            if (_sender is null) return;
            MatchCard sender = (MatchCard)_sender;

            this.BackColor = FocusBackColor;

            if (e.Data is not null && e.Data.GetDataPresent(typeof(MatchCard))) {
                e.Effect = DragDropEffects.Copy;
                foreach (MemoryTextBox textBox in sender.flowLeft.Controls) textBox.ForeColor = Color.Gray;
                foreach (MemoryTextBox textBox in sender.flowRight.Controls) textBox.ForeColor = Color.Gray;
            }
            else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OnDragDrop(object? _sender, DragEventArgs e) {
            if (_sender is null) return;
            MatchCard sender = (MatchCard)_sender;

            this.BackColor = MatchCard.DefaultBackColor;
            e.Effect = DragDropEffects.All;

            if (e.Data is null) return;
            if (e.Data.GetData(typeof(MatchCard)) is null) return;
            MatchCard? source = (MatchCard)e.Data.GetData(typeof(MatchCard));

            foreach (MemoryTextBox textBox in sender.flowLeft.Controls) textBox.ForeColor = Color.Black;
            foreach (MemoryTextBox textBox in sender.flowRight.Controls) textBox.ForeColor = Color.Black;

            var fromPlayersT0 = this.Parent.Dragging.Match.Teams[0].ClearPlayers();
            var fromPlayersT1 = this.Parent.Dragging.Match.Teams[1].ClearPlayers();
            var toPlayersT0 = sender.Match.Teams[0].ClearPlayers();
            var toPlayersT1 = sender.Match.Teams[1].ClearPlayers();

            this.Parent.Dragging.Match.Teams[0].AddPlayers(toPlayersT0);
            this.Parent.Dragging.Match.Teams[1].AddPlayers(toPlayersT1);
            sender.Match.Teams[0].AddPlayers(fromPlayersT0);
            sender.Match.Teams[1].AddPlayers(fromPlayersT1);
        }

        private void OnDragLeave(object? _sender, EventArgs e) {
            if (_sender is null) return;
            MatchCard sender = (MatchCard)_sender;
            this.BackColor = MatchCard.DefaultBackColor;
            if (sender == this.Parent.Dragging) return;

            foreach (MemoryTextBox textBox in sender.flowLeft.Controls) textBox.ForeColor = Color.Black;
            foreach (MemoryTextBox textBox in sender.flowRight.Controls) textBox.ForeColor = Color.Black;
        }

        public void AddTextBox() {
            this.flowLeft.AddTextBox();
            this.flowRight.AddTextBox();
        }

        private void InitializeComponents() {
            this.tableLayout = new TableLayoutPanel();

            while (this.Match.Teams.Count < 2) this.Match.NewTeam();
            this.flowLeft = new(this.Match.Teams[0]);
            this.flowRight = new(this.Match.Teams[1]);
            this.txtBowls1 = new(this.Match.Teams[0]);
            this.txtBowls2 = new(this.Match.Teams[1]);
            this.lblLane = new();
            this.AutoSize = true;

            this.SuspendLayout();
            this.tableLayout.SuspendLayout();
            this.txtBowls1.SuspendLayout();
            this.txtBowls2.SuspendLayout();
            this.lblLane.SuspendLayout();

            this.tableLayout.AutoSize = true;
            this.tableLayout.ColumnCount = 6;
            this.tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            this.tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            this.tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));

            this.tableLayout.Controls.Add(this.txtBowls1, 0, 0);
            this.tableLayout.Controls.Add(this.flowLeft, 1, 0);
            this.tableLayout.Controls.Add(this.lblLane, 2, 0);
            this.tableLayout.Controls.Add(this.flowRight, 3, 0);
            this.tableLayout.Controls.Add(this.txtBowls2, 4, 0);

            this.tableLayout.Location = new Point(0, 0);
            this.tableLayout.Margin = new Padding(0);
            this.tableLayout.Padding = new Padding(15, 5, 15, 5);
            this.tableLayout.RowCount = 1;
            this.tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayout.TabIndex = 0;
            this.tableLayout.Dock = DockStyle.Fill;
            this.tableLayout.MouseDown += OnMouseDown;

            this.txtBowls1.MouseDown += OnMouseDown;
            this.txtBowls2.MouseDown += OnMouseDown;

            this.lblLane.Text = $"{this.Match.Lane + 1}";
            this.lblLane.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblLane.Dock = DockStyle.Fill;
            this.lblLane.Location = new Point(3, 3);
            this.lblLane.Size = new Size(50, 55);
            this.lblLane.TextAlign = ContentAlignment.MiddleCenter;
            this.lblLane.MouseDown += OnMouseDown;

            this.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayout);
            this.MinimumSize = new Size(300, 64);
            this.Size = new Size(300, 64);
            this.BackColor = MatchCard.DefaultBackColor;
            this.AllowDrop = true;
            this.DoubleBuffered = true;

            this.tableLayout.ResumeLayout(false);
            this.txtBowls1.ResumeLayout(false);
            this.txtBowls2.ResumeLayout(false);
            this.lblLane.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private TableLayoutPanel tableLayout;
        private BowlsTextPanel txtBowls1;        
        private NamesTextFlow flowLeft;
        private Label lblLane;
        private NamesTextFlow flowRight;
        private BowlsTextPanel txtBowls2;
    }

    /// <summary>
    /// The flow panel target contains the players names.
    /// One for each team.
    /// </summary>
    public class NamesTextFlow : FlowLayoutPanel {

        public Team Team {get; private set;}

        public NamesTextFlow(Team team) : base() {
            this.InitializeComponents();

            int teamSize = int.Parse(team.LeagueEvent.Settings["team_size"]);
            for (int x = 0; x < teamSize; x++) this.AddTextBox();

            this.Team = team;
            int i = 0;

            foreach (MemoryTextBox textBox in this.Controls) {
                textBox.Text = "";
                if (team.Players.Count > i) textBox.Text = team.Players[i++];
            }

            team.League.TeamTable.RowDeleting += this.TeamTable_RowDeleted;
            team.League.TeamTable.RowChanged += this.TeamTable_RowChanged;
        }

        private void TeamTable_RowChanged(object sender, DataRowChangeEventArgs e) {            
            if ((int)e.Row[TeamTable.COL.EVENT_TABLE_UID] != this.Team.EventTableUID) return;
            string name = (string)e.Row[TeamTable.COL.PLAYER_NAME];

            Debug.WriteLine(e.Row.PrettyPrint("Row Changed"));
            if (!this.HasPlayerName(name)) this.AddPlayerName(name);
        }

        private void TeamTable_RowDeleted(object sender, System.Data.DataRowChangeEventArgs e) {
            if ((int)e.Row[TeamTable.COL.EVENT_TABLE_UID] != this.Team.EventTableUID) return;
            this.DeletePlayerName((string)e.Row[TeamTable.COL.PLAYER_NAME]);
        }

        private void DeletePlayerName(string name) {
            foreach (MemoryTextBox textBox in this.Controls) {
                if (textBox.Text == name) textBox.Text = "";
            }
        }

        private bool AddPlayerName(string name) {
            foreach (MemoryTextBox textBox in this.Controls) {
                if (textBox.Text == "") {
                    textBox.Text = name;
                    return true;
                }
            }
            return false;
        }

        public bool HasPlayerName(string name) {
            foreach(MemoryTextBox textBox in this.Controls) {
                if (textBox.Text == name) return true;
            }
            return false;
        }

        private void InitializeComponents() {
            this.SuspendLayout();

            this.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = Color.FromArgb(120, 120, 240);
            this.FlowDirection = FlowDirection.TopDown;
            this.MaximumSize = new Size(0, 0);

            this.ResumeLayout(false);
        }

        public void AddTextBox() {
            MemoryTextBox textBox = new();
            textBox.SuspendLayout();

            this.Controls.Add(textBox);
            textBox.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);

            this.Resize += new EventHandler((src, args) => {
                var parent = textBox.Parent!;
                textBox.Width = parent.Width - parent.Margin.Left - parent.Margin.Right;
            });

            textBox.MemoryUpdate += this.TextBox_MemoryUpdate;

            textBox.ResumeLayout(false);
        }

        private void TextBox_MemoryUpdate(object? sender, MemoryTextBox.MemoryUpdateArgs e) {
            if (e.TextBefore != "") this.Team.RemovePlayer(e.TextBefore);
            if (e.TextAfter != "") this.Team.AddPlayer(e.TextAfter);
        }
    }

    public class MemoryTextBox : TextBox {
        public class MemoryUpdateArgs {
            public required string TextBefore { get; init; }
            public required string TextAfter { get; init; }
        }

        public delegate void MemoryUpdateEvent(object? sender, MemoryUpdateArgs e);
        public event MemoryUpdateEvent MemoryUpdate = delegate { };

        public string Temp {
            set {
                base.Text = value;
            }
        }

        public new string Text {
            get => base.Text;
            set {
                base.Text = value;
                this.Memory = value;
            }
        }

        public string Memory {
            get; private set;
        }

        public MemoryTextBox() : base() {
            this.InitializeComponents();
            this.LostFocus += this.MemoryTextBox_LostFocus;
            this.KeyDown += this.MemoryTextBox_KeyDown;
            this.Memory = this.Text;
        }

        public void InitializeComponents() {
            this.SuspendLayout();
            this.BackColor = Color.WhiteSmoke;
            this.ResumeLayout(false);
        }

        private void MemoryTextBox_KeyDown(object? sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.InvokeMemoryEvent();
            }
        }

        public void Revert() {
            base.Text = this.Memory;
        }

        private void MemoryTextBox_LostFocus(object? sender, EventArgs e) {
            this.InvokeMemoryEvent();
        }

        private void InvokeMemoryEvent() {
            if (this.Text == this.Memory) return;

            MemoryUpdateArgs args = new() {
                TextBefore = this.Memory,
                TextAfter = this.Text
            };
            this.Memory = this.Text;

            MemoryUpdate.Invoke(this, args);
        }
    }

    public class BowlsTextPanel : Panel {
        private int Memory = 0;
        private Team Team { get; init; }

        public BowlsTextPanel(Team team) : base() {
            this.Team = team;
            this.txtTeam = new TextBox();
            this.InitializeComponents();
            this.Resize += OnResize;            
        }

        private void InitializeComponents() {
            this.SuspendLayout();
            this.txtTeam.SuspendLayout();

            this.Controls.Add(this.txtTeam);
            this.Dock = DockStyle.Fill;
            this.Location = new Point(3, 3);
            this.Size = new Size(50, 55);

            this.txtTeam.Anchor = AnchorStyles.None;
            this.txtTeam.BorderStyle = BorderStyle.None;
            this.txtTeam.Font = new Font("Segoe UI Black", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.txtTeam.Size = new Size(50, 40);
            this.txtTeam.Text = "0";
            this.txtTeam.TextAlign = HorizontalAlignment.Center;
            this.txtTeam.TextChanged += this.TxtTeam_TextChanged;
            this.txtTeam.GotFocus += this.TxtTeam_GotFocus;

            this.txtTeam.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void TxtTeam_GotFocus(object? sender, EventArgs e) {
            this.txtTeam.BeginInvoke(new Action(this.txtTeam.SelectAll));
        }

        private void TxtTeam_TextChanged(object? sender, EventArgs e) {

            if (this.txtTeam.Text == "") {
                this.txtTeam.Text = "0";
                return;
            }

            var success = int.TryParse(this.txtTeam.Text, out int value);

            if (!success) {
                this.txtTeam.Text = $"{this.Memory}";
            }
            else {
                if (value < 0) value = 0;
                if (value > 99) value = 99;

                this.txtTeam.Text = $"{value}";
                this.Memory = value;
                this.Team.Bowls = value;
            }
        }

        private void OnResize(object? sender, EventArgs e) {
            this.Center(this.txtTeam);
        }

        private readonly TextBox txtTeam;
    }

    static class Extensions {
        public static void Center(this Control target, Control control) {
            control.Top = (target.Height - control.Height) / 2;
            control.Left = (target.Width - control.Width) / 2;
        }
    }
}
