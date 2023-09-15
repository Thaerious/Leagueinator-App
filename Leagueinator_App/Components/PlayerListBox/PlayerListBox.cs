using Leagueinator.Components;
using Leagueinator.Model;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace Leagueinator.App.Components.PlayerListBox {
    public partial class PlayerListBox : ListBox {

        public Round? Round {
            private get => this._round;

            set {
                if (this._round != null) {
                    this._round.IdlePlayers.CollectionChanged -= this.IdlePlayersCollectionChanged;
                }

                this._round = value;
                this.Items.Clear();
                if (value == null) return;

                foreach (PlayerInfo pi in value.IdlePlayers) {
                    this.Items.Add(pi);
                }

                value.IdlePlayers.CollectionChanged += this.IdlePlayersCollectionChanged;
            }
        }

        public PlayerListBox(IContainer container) {
            container.Add(this);
            this.InitializeComponent();
            this.AllowDrop = true;

            this.menuDelete.Click += new System.EventHandler(this.HndMenuDelete);
            this.menuRename.Click += new System.EventHandler(this.HndMenuRename);

            _ = new ControlDragHandlers<PlayerInfo>(this,
                () => { // get data from source
                    if (this.Round is null) return null;
                    if (this.SelectedItem == null) return null;
                    var pInfo = (PlayerInfo)this.SelectedItem;
                    this.Round.IdlePlayers.Remove(pInfo);
                    return pInfo;
                },
                (pi) => { // set data on target
                    if (this.Round is null) return null;
                    if (pi is not null) this.Round.IdlePlayers.Add(pi);
                    return null;
                },
                (pi) => { // response to source
                    if (this.Round is null) return;
                    if (pi is not null) this.Round.IdlePlayers.Add(pi);
                }
            );
        }

        /// <summary>
        /// Event handler for when the round model changes idle players.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void IdlePlayersCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args) {
            switch (args.Action) {
                case NotifyCollectionChangedAction.Add:
                    if (args.NewItems != null) {
                        foreach (var pi in args.NewItems) this.Items.Add(pi);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (args.OldItems != null) {
                        foreach (var pi in args.OldItems) this.Items.Remove(pi);
                    }
                    break;
            }
        }

        private void Context_Opening(object sender, CancelEventArgs e) {
            if (this.SelectedItems.Count == 0) {
                e.Cancel = true;
            }
        }

        private Round? _round = null;
    }
}
