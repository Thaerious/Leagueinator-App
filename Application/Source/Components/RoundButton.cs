using Leagueinator.Model.Tables;

namespace Leagueinator.Components {

    /// <summary>
    /// A forms Button coupled with a RoundRow
    /// </summary>
    /// <param name="round"></param>
    internal class RoundButton(RoundRow round) : Button {
        public RoundRow Round { get; } = round;
    }
}
