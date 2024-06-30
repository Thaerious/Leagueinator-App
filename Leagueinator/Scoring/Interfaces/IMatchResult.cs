using Leagueinator.Model.Tables;
namespace Leagueinator.Scoring {
    /// <summary>
    /// The results of a single match for a single team.
    /// </summary>
    /// <param name="teamRow"></param>    
    public interface IMatchResult<T> : IComparable<T> where T: IMatchResult<T> {

        public TeamRow TeamRow { get; }
    }
}
