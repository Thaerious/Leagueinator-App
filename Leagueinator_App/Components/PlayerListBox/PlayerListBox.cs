using Leagueinator.Components;
using System.Collections.Specialized;
using System.ComponentModel;
using Model;
using System.Data;

namespace Leagueinator.App.Components {
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

                foreach (string pi in value.IdlePlayers) {
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

            new ControlDragHandlers<string>(this,
                () => { // [getData] called when this is the source
                    if (this.Round is null) return null;
                    if (this.SelectedItem == null) return null;
                    var pInfo = (string)this.SelectedItem;
                    return pInfo;
                },
                (pi, src) => { // [sendData] called when this is the destination, receives value from [getData]
                    if (src == this) return null;
                    if (this.Round is null) return null;
                    if (pi is not null) this.Round.IdlePlayers.Add(pi);
                    return null;
                },
                (pi, dest) => { // [hndResponse] called when this is the source, receives value from [sendData]
                    if (dest == this) return;  // don't drop on source
                    if (this.Round is null) return;
                    string? pInfo = (string?)this.SelectedItem;
                    if (pInfo == null) return;  // do nothing is no item is selected
                    this.Round.IdlePlayers.Remove(pInfo);

                    if (pi is null) return;  // do nothing if is no item is returned
                    this.Round.IdlePlayers.Add(pi);
                }
            );
        }

        /// <summary>
        /// Event handler for when the round model changes idle players.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void IdlePlayersCollectionChanged(object? sender, DataRowChangeEventArgs args) {
            throw new NotImplementedException();
            //switch (args.Action) {
            //case DataRowAction.Delete:
            //    if (args.NewItems != null) {
            //        foreach (var pi in args.NewItems) this.Items.Add(pi);
            //    }
            //    break;
            //case NotifyCollectionChangedAction.Remove:
            //    if (args.OldItems != null) {
            //        foreach (var pi in args.OldItems) this.Items.Remove(pi);
            //    }
            //    break;
            //case NotifyCollectionChangedAction.Reset:
            //    this.Items.Clear();
            //    break;
            //}
        }
        private void Context_Opening(object sender, CancelEventArgs e) {
            if (this.SelectedItems.Count == 0) {
                e.Cancel = true;
            }
        }

        private Round? _round = null;
    }
}
