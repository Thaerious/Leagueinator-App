using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.App {

    /// <summary>
    /// Throw this exception when the user should not be able to access a component.
    /// </summary>
    public class AppStateException : Exception{
        public AppStateException(string message) : base(message){ }
                public AppStateException() : base() { }
    }
}
