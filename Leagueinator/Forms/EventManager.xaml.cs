using Leagueinator.Model;
using Leagueinator.Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Leagueinator.Forms {
    /// <summary>
    /// Interaction logic for EventManager.xaml
    /// </summary>
    public partial class EventManager : Window {
        public EventManager(Model.League league) {
            InitializeComponent();
            this.League = league;
        }

        private void PopulateNamePanel() {
            foreach (EventRow eventRow in this.League.EventTable) {
            }
        }

        public League League { get; }
    }
}
