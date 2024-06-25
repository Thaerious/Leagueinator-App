using Leagueinator.Model.Tables;
using Leagueinator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_Test {
    [TestClass]
    public class Idle_Test {
        /// <summary>
        /// Calling remove on an idle player removes it form the round's idle player list.
        /// </summary>
        [TestMethod]
        public void Idle_Remove_Contains() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            roundRow.IdlePlayers.Add("Zen");
            roundRow.IdlePlayers.Get("Zen")!.Remove();

            Assert.IsFalse(roundRow.IdlePlayers.Has("Zen"));
        }
    }
}
