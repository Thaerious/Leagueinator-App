
namespace Leagueinator.Model.Tables {
    /// <summary>
    /// Represents the method that will handle the CustomRow update events in a LeagueSettingsTable.
    /// </summary>
    public delegate void CustomRowEventHandler<T>(object sender, CustomRowAddEventArgs<T> args) 
        where T : CustomRow;
}
