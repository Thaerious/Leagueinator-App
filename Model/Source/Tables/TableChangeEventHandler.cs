
namespace Leagueinator.Model.Tables {
    /// <summary>
    /// Represents the method that will handle the CustomRow update events in a LeagueTable.
    /// </summary>
    public delegate void TableChangeEventHandler<T>(object sender, CustomRowAddEventArgs<T> args) 
        where T : CustomRow;
}
