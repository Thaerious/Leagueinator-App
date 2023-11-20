using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.CSSParser {

    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class CSS : Attribute{
        public readonly string Key;

        public CSS() { 
            this.Key = "";
        }

        public CSS(string key) {
            this.Key = key;
        }
    }
}
