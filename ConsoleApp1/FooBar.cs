using Leagueinator.Printer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    internal class FooBar {
        public int AnInt { get; set; } = 0;
        public float AFloat { get; set; } = 0;
        public string AString { get; set; } = "";
        public Cardinal<int> ManyInts { get; set; } = new(0);

        public override string ToString() {
            return $"'{AnInt}' '{AFloat}' '{AString}' '{ManyInts}'";
        }
    }
}
