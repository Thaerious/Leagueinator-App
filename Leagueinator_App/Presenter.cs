using Model.Tables;


namespace Leagueinator_App {
    public class Presenter {
        public delegate void RoundEvent(RoundRow[] roundRows);
        public delegate void IdleEvent(IdleRow[] idleRows);
        public delegate void MemberEvent(MemberRow[] memberRows);

        public event RoundEvent OnRoundAdded = delegate { };
        public event RoundEvent OnRoundRemoved = delegate { };
        public event RoundEvent OnIdleAdded = delegate { };
        public event RoundEvent OnIdleRemoved = delegate { };
        public event RoundEvent OnMemberAdded = delegate { };
        public event RoundEvent OnMemberRemoved = delegate { };

        public void RemoveRound(int uid) {
        }
    }
}
