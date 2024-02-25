using Model;
using Model.Tables;
using System.ComponentModel;
using System.Data;

namespace Leagueinator {
    public class Controller : Component {
        public delegate void UpdateMatch(MatchRow matchRow);
        public delegate void UpdateTeam(TeamRow teamRow);
        public readonly League League = new MockModel();

        public event UpdateTeam OnUpdateTeam {
            add {
                if (value is null) return;
                this._onUpdateTeam += value;

                foreach (DataRow dataRow in League.TeamTable.Rows) {
                    TeamRow teamRow = new(dataRow);
                    _onUpdateTeam.Invoke(teamRow);
                };
            }
            remove {                
                if (value is null) return;
                this._onUpdateTeam -= value;
            }
        }

        public event UpdateMatch OnUpdateMatch {            
            add {                
                if (value is null) return;
                this._onUpdateMatch += value;

                foreach (DataRow dataRow in League.MatchTable.Rows) {
                    MatchRow teamRow = new(dataRow);
                    _onUpdateMatch.Invoke(teamRow);
                };
            }
            remove {
                if (value is null) return;
                this._onUpdateMatch -= value;
            }
        }

        private UpdateMatch _onUpdateMatch = delegate { };
        private UpdateTeam _onUpdateTeam = delegate { };
    }
}
