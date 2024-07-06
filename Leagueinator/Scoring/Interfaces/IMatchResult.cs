using Leagueinator.Model.Tables;
namespace Leagueinator.Scoring {
    /// <summary>
    /// The results of a single match for a single team.
    /// </summary>
    /// <param name="teamRow"></param>    
    public interface IMatchResult<SELF> : IComparable<SELF> where SELF: IMatchResult<SELF> {

        public TeamRow TeamRow { get; }
    }
}
