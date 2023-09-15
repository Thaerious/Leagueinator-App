using System.Windows.Forms;

namespace Leagueinator.App.Components.MatchCard {
    public partial class MatchLabel : Label {
        private ContextMenuStrip contextMenu;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem menuDelete;
        private ToolStripMenuItem menuRename;

        public int Team { get; set; } = 0;
        public int Position { get; set; } = 0;

        public MatchLabel() : base() {
            this.InitializeComponent();
        }

        public override string ToString() {
            return $"MatchLabel [team = {this?.Team}, pos = {this?.Position}]";
        }

        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRename = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDelete,
            this.menuRename});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(200, 68);
            this.contextMenu.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.ContextClosing);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextOpening);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(199, 32);
            this.menuDelete.Text = "Delete Player";
            this.menuDelete.Click += new System.EventHandler(this.Menu_Delete);
            // 
            // menuRename
            // 
            this.menuRename.Name = "menuRename";
            this.menuRename.Size = new System.Drawing.Size(199, 32);
            this.menuRename.Text = "Rename Player";
            this.menuRename.Click += new System.EventHandler(this.Menu_Rename);
            // 
            // MatchLabel
            // 
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void ContextOpening(object sender, System.ComponentModel.CancelEventArgs e) {
            this.ForeColor = Color.Blue;
        }

        private void ContextClosing(object sender, ToolStripDropDownClosingEventArgs e) {
            this.ForeColor = Color.Black;
        }

        private void Menu_Delete(object sender, EventArgs e) {
            //this.DeletePlayer?.Invoke(this, new PlayerInfoArgs {
            //    PlayerInfo = this.PlayerInfo
            //});
        }

        private void Menu_Rename(object sender, EventArgs e) {
            //this.RenamePlayer?.Invoke(this, new PlayerInfoArgs {
            //    PlayerInfo = this.PlayerInfo
            //});
        }
    }
}
