using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables {
    public abstract class CustomTable : DataTable {
        public readonly League League;

        public CustomTable(League league, string tableName) : base(tableName) {
            this.League = league;
        }

        abstract public void BuildColumns();
    }    
}
