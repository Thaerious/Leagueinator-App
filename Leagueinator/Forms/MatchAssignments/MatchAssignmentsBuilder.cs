using Leagueinator.Model.Tables;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Printer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Forms.MatchAssignments {
    internal class MatchAssignmentsBuilder(RoundRow roundRow) : IXMLBuilder {
        
        private readonly RoundRow RoundRow = roundRow;
        
        public Element BuildElement() {
            LoadedStyles styles = Assembly.GetExecutingAssembly().LoadStyleResource("Leagueinator.Assets.MatchAssignments.Root.style");
            Element docroot = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.MatchAssignments.Root.xml");
            Element matchSrc = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.MatchAssignments.Match.xml");

            List<MatchRow> matchRows = this.RoundRow.Matches.ToList();
            matchRows.Sort((a, b) => {
                return a.Lane > b.Lane ? 1 : -1;
            });

            foreach (MatchRow matchRow in matchRows) {
                Element matchCard = matchSrc.Clone();
                matchCard["lane"][0].InnerText = $"{matchRow.Lane + 1}";
                docroot["page"][0].AddChild(matchCard);

                switch (matchRow.MatchFormat) {
                    case MatchFormat.VS1:
                        matchCard["format"][0].InnerText = "1 v 1";
                        this.BuildVS(matchCard, matchRow);
                        break;
                    case MatchFormat.VS2:
                        matchCard["format"][0].InnerText = "2 v 2";
                        this.BuildVS(matchCard, matchRow);
                        break;
                    case MatchFormat.VS3:
                        matchCard["format"][0].InnerText = "3 v 3"; 
                        this.BuildVS(matchCard, matchRow);
                        break;
                    case MatchFormat.VS4:
                        matchCard["format"][0].InnerText = "4 v 4";
                        this.BuildVS(matchCard, matchRow);
                        break;
                    case MatchFormat.A4321:
                        matchCard["format"][0].InnerText = "4321";
                        this.Build4321(matchCard, matchRow);
                        break;
                }
            }

            styles.AssignTo(docroot);
            return docroot;
        }

        private void BuildVS(Element card, MatchRow matchRow) {
            foreach (TeamRow teamRow in matchRow.Teams) {
                Element teamElement = new("team", []);

                foreach (MemberRow memberRow in teamRow.Members) {
                    teamElement.AddChild(new("player", []) {
                        InnerText = memberRow.Player
                    });
                }
                
                card["teams"][0].AddChild(teamElement);
                if (teamRow.Index >= 2) break;
            }
        }

        private void Build4321(Element card, MatchRow matchRow) {
            Element teamElement = new("team", []);
            foreach (TeamRow teamRow in matchRow.Teams) {
                if (teamRow.Members.Count == 0) continue;
                MemberRow memberRow = teamRow.Members[0] ?? throw new NullReferenceException();

                teamElement.AddChild(new("player", []) {
                    InnerText = memberRow.Player
                });
            }
            card["teams"][0].AddChild(teamElement);
        }
    }
}
