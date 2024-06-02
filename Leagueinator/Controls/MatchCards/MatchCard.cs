using Leagueinator.Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Leagueinator.Controls {
    public abstract class MatchCard : UserControl {
        public abstract MatchRow MatchRow { get; set; }
        protected MatchRow? _matchRow = default;
        internal CardTarget? CardTarget { get; set; }
    }
}
