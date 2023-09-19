using Leagueinator.Model;
using System.Diagnostics;

namespace Leagueinator.App.Components.PlayerListBox {
    
    public class PlayerListBoxArgs {
        public readonly PlayerInfo PlayerInfo;

        public PlayerListBoxArgs(PlayerInfo playerInfo) {
            this.PlayerInfo = playerInfo;
        }
    }

    public partial class PlayerListBox : ListBox {
        public delegate void PlayerListBoxRename(PlayerListBox source, PlayerListBoxArgs args);
        public delegate void PlayerListBoxDelete(PlayerListBox source, PlayerListBoxArgs args);

        public event PlayerListBoxRename OnRename = delegate { };
        public event PlayerListBoxDelete OnDelete = delegate { };

        private void HndMouseDown(object? sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                if (this.SelectedItem is not PlayerInfo pInfo) return;
                this.contextMenu.Show(this, new Point(e.X, e.Y));
            }
        }

        private void HndMenuDelete(object? sender, EventArgs? e) {
            if (this.SelectedItem is not PlayerInfo pInfo) return;
            this.OnDelete?.Invoke(this, new PlayerListBoxArgs(pInfo));
        }

        private void HndMenuRename(object? sender, EventArgs? e) {
            if (this.SelectedItem is not PlayerInfo pInfo) return;
            this.OnRename?.Invoke(this, new PlayerListBoxArgs(pInfo));
        }
    }
}
