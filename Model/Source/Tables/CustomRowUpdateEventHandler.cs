
namespace Leagueinator.Model.Tables {
    /// <summary>
    /// Represents the method that will handle the CustomRow update events in a LeagueTable.
    /// </summary>
    public delegate void CustomRowUpdateEventHandler<T>(object sender, CustomRowUpdateEventArgs<T> args) 
        where T : CustomRow;
}
