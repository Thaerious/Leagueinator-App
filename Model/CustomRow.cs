using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables {
    public class CustomRow(League league, DataRow row) {
        public readonly League League = league;
        public readonly DataRow DataRow = row;
    }
}
