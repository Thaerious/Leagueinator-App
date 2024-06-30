using System.Data;
using System.Xml.Serialization;

namespace Leagueinator.Model.Tables {
    public class LeagueSettingsTable : TableBase{
        internal LeagueSettingsTable() : base(tableName: "league_settings") { }

        internal static class COL {
            public static readonly string KEY = "key";
            public static readonly string VALUE = "value";
            public static readonly string TYPE = "type";
        }

        public void Set(string key, object value) {
            if (this.Has(key)) {
                this.Reset(key, value);
                return;
            }

            var row = this.NewRow();

            XmlSerializer xmlSerializer = new XmlSerializer(value.GetType());
            using StringWriter sw = new StringWriter();
            xmlSerializer.Serialize(sw, value);

            row[COL.KEY] = key;
            row[COL.VALUE] = sw.ToString();
            row[COL.TYPE] = $"{value.GetType().FullName}, {value.GetType().Assembly.GetName().Name}";

            this.Rows.Add(row);
        }

        private void Reset(string key, object value) {            
            string query = $"{COL.KEY} = '{key}'";
            DataRow[] rows = this.Select(query);
            if (rows.Length == 0) throw new KeyNotFoundException(query);
            var row = rows[0];

            XmlSerializer xmlSerializer = new XmlSerializer(value.GetType());
            using StringWriter sw = new StringWriter();
            xmlSerializer.Serialize(sw, value);

            row[COL.VALUE] = sw.ToString();
            row[COL.TYPE] = $"{value.GetType().FullName}, {value.GetType().Assembly.GetName().Name}";
        }

        public bool UnSet(string key) {
            string query = $"{COL.KEY} = '{key}'";
            DataRow[] rows = this.Select(query);
            if (rows.Length == 0) return false;
            rows[0].Delete();
            return true;
        }

        public bool Has(string key) {
            string query = $"{COL.KEY} = '{key}'";
            DataRow[] rows = this.Select(query);
            return rows.Length > 0;
        }

        public T Get<T>(string key) {
            string query = $"{COL.KEY} = '{key}'";
            DataRow[] rows = this.Select(query);
            if (rows.Length == 0) throw new KeyNotFoundException(key);

            string valueString = (string)rows[0][COL.VALUE];
            string typeString = (string)rows[0][COL.TYPE];

            Type type = Type.GetType(typeString) ?? throw new NullReferenceException($"Error referencing type '{typeString}'");
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            using StringReader sr = new StringReader(valueString);
            return (T)(xmlSerializer.Deserialize(sr) ?? throw new NullReferenceException());
        }

        internal override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.KEY,
                Unique = true
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.VALUE
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.TYPE
            });
        }
    }
}
