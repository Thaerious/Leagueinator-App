using System.Data;

DataTable table = new DataTable();
table.Columns.Add("Id", typeof(int));
table.Columns.Add("Name", typeof(string));

DataRow row = table.NewRow();
row["Id"] = 1;
row["Name"] = "Alice";
table.Rows.Add(row);
table.AcceptChanges(); // Accept changes to simulate a fully integrated row


table.RowDeleting += Table_RowDeleting;

void Table_RowDeleting(object sender, DataRowChangeEventArgs e) {
    Console.WriteLine("Deleting DataRow - Name: " + e.Row["Name"] + " " + e.Row.GetHashCode());
}

// Trying to access the DataRow data
try {
    Console.WriteLine("Row State before access: " + row.RowState);
    Console.WriteLine("Attached DataRow - Name: " + row["Name"] + " " + row.GetHashCode());

    // Remove the DataRow from the DataTable
    table.Rows.Remove(row);
    table.AcceptChanges(); // Finalize changes

    // This should be safe if the row is detached
    Console.WriteLine("Row State before access: " + row.RowState);
    Console.WriteLine("Detached DataRow - Name: " + row["Name"] + " " + row.GetHashCode());
}
catch (RowNotInTableException ex) {
    Console.WriteLine("Caught RowNotInTableException: \n" + ex.Message);
}

Console.ReadLine();
