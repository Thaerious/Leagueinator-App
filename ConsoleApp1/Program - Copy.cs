//using Model;
//using Model.Tables;
//using System.Data;
//using System.Diagnostics;
//using System.Text;

//Debug.WriteLine("Part 1");

//DataTable table1 = new();

//table1.Columns.Add(new DataColumn {
//    DataType = typeof(int),
//    ColumnName = "UID",
//    Unique = true,
//    AutoIncrement = true
//});

//table1.Columns.Add(new DataColumn {
//    DataType = typeof(int),
//    ColumnName = "FK_UID",
//    Unique = false,
//    AutoIncrement = false
//});

//table1.Columns.Add(new DataColumn {
//    DataType = typeof(int),
//    ColumnName = "ROUND",
//    Unique = false,
//    AutoIncrement = false
//});

//table1.Columns.Add(new DataColumn {
//    DataType = typeof(string),
//    ColumnName = "NAME",
//    Unique = false,
//    AutoIncrement = false
//});

//var row1 = table1.NewRow();
//row1["FK_UID"] = 0;
//row1["ROUND"] = 0;
//row1["NAME"] = "ima_name_1";
//table1.Rows.Add(row1);
//Console.WriteLine(table1.PrettyPrint());

//Debug.WriteLine("Part 2");

//var table2 = new MyTable();
//table2.AddRow(0, 0, "ima_name_2");
//Console.WriteLine(table2.PrettyPrint());

//Debug.WriteLine("Part 3");
//var table3 = new IdleTable();
//table3.AddRow(0, 0, "ima_name_3");
//Console.WriteLine(table3.PrettyPrint());

//Debug.WriteLine("Part 4");
//Wrapper league = new();
//LeagueEvent leagueEvent = league.NewLeagueEvent("myEvent");
//Round round = leagueEvent.NewRound();
//round.IdlePlayers.Add("ima_name_4");

//Debug.WriteLine("Part 5");
//Debug.WriteLine(league.IdleTable.GetType() == table3.GetType());
//league.IdleTable.AddRow(0, 0, "ima_name_5");

//Console.WriteLine("Press any key to continue.");
//Console.ReadKey();
//Debug.WriteLine("Done");

//public class MyTable : DataTable {
//    public static readonly string TABLE_NAME = "idle";

//    public static class COL {
//        public static readonly string UID = "uid";
//        public static readonly string DIR_UID = "event_dir_uid";
//        public static readonly string ROUND = "round";
//        public static readonly string PLAYER_NAME = "player_name";
//    }

//    public MyTable() : base(TABLE_NAME) {
//        MakeTable(this);
//    }

//    public DataRow AddRow(int eventUID, int round, string playerName) {
//        Debug.WriteLine($"ADDROW {eventUID}:{eventUID.GetType()} {round}:{round.GetType()} {playerName}:{playerName.GetType()} ");

//        var row = this.NewRow();

//        row[COL.DIR_UID] = eventUID;
//        row[COL.ROUND] = round;
//        row[COL.PLAYER_NAME] = playerName;

//        this.Rows.Add(row);
//        return row;
//    }

//    public DataRow? GetRow(int eventDirUID, int round, string playerName) {
//        var rows = this.AsEnumerable()
//                       .Where(row => row.Field<int>(COL.DIR_UID) == eventDirUID)
//                       .Where(row => row.Field<int>(COL.ROUND) == round)
//                       .Where(row => row.Field<string>(COL.PLAYER_NAME) == playerName)
//                       .ToList();

//        if (rows.Count == 0) return null;
//        return rows[0];
//    }

//    public void RemoveRows(int eventUID, int round, string playerName) {

//        var rowsToDelete = this.AsEnumerable()
//                           .Where(row => row.Field<int>(COL.DIR_UID) == eventUID)
//                           .Where(row => row.Field<int>(COL.ROUND) == round)
//                           .Where(row => row.Field<string>(COL.PLAYER_NAME) == playerName)
//                           .ToList()
//                           ;

//        foreach (DataRow row in rowsToDelete) {
//            this.Rows.Remove(row);
//        }
//    }

//    public static MyTable MakeTable(MyTable? table = null) {
//        table ??= new();

//        table.Columns.Add(new DataColumn {
//            DataType = typeof(int),
//            ColumnName = COL.UID,
//            Unique = true,
//            AutoIncrement = true
//        });

//        table.Columns.Add(new DataColumn {
//            DataType = typeof(int),
//            ColumnName = COL.DIR_UID,
//            Unique = false,
//            AutoIncrement = false
//        });

//        table.Columns.Add(new DataColumn {
//            DataType = typeof(int),
//            ColumnName = COL.ROUND,
//            Unique = false,
//            AutoIncrement = false
//        });

//        table.Columns.Add(new DataColumn {
//            DataType = typeof(string),
//            ColumnName = COL.PLAYER_NAME,
//            Unique = false,
//            AutoIncrement = false
//        });

//        return table;
//    }
//}
