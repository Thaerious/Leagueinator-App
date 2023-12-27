using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leagueinator_App.Forms {
    public partial class MatchDevForm : Form {
        public MatchDevForm() {
            InitializeComponent();
            CustomInitialization();
        }

        public void CustomInitialization() {
            var league = new League();
            var lEvent = league.NewLeagueEvent("myEvent");

            lEvent.Settings["Lane_Count"] = 4.ToString();
            lEvent.Settings["Team_Size"] = 2.ToString();

            var round = lEvent.NewRound();
            var match = round.GetMatch(0);

            this.matchControl1.AddPlayer(0);

            Debug.WriteLine(league.PrettyPrint());
        }
    }
}
