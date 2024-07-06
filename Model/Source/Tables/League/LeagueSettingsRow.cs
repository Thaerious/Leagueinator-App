using Leagueinator.Model;
using Leagueinator.Model.Tables;
using System.Data;

namespace Model.Source.Tables.League {
    public class LeagueSettingsRow : CustomRow {
        public LeagueSettingsRow(DataRow dataRow) : base(dataRow) {}

        public string Key {
            get => (string)this[LeagueSettingsTable.COL.KEY];
        }

        public object Value {
            get => this.League.LeagueSettingsTable.Get<object>(this.Key);
        }

        public Type? Type {
            get {
                string typeName = (string)this[LeagueSettingsTable.COL.TYPE];
                return Type.GetType(typeName); 
            }
        }
    }
}
