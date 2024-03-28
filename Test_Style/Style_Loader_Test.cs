using Leagueinator.CSSParser;
using Leagueinator.Printer.Styles;
using Leagueinator.Utility;
using System.Diagnostics;
using static Test_Style.Loader;

namespace Test_Style {
    [TestClass]
    public class Style_Loader_Test {

        [TestMethod]
        public void Sanity() {
            LoadedStyles styles = LoadSSResource("style_loader.css");

            Assert.IsTrue(styles[0].Selector.Equals(".parent"));
            Assert.IsTrue(styles[1].Selector.Equals(".child"));
            Assert.IsTrue(styles[2].Selector.Equals("parent > child > deepchild"));
            Assert.IsTrue(styles[3].Selector.Equals("*"));

            foreach (Style style in styles.AsList()) {
                Debug.WriteLine($"{style.Selector} [{style.Specificity.DelString()}]");
            }
            Debug.WriteLine($"--------------------\n{styles.AsList().Count} Style{(styles.AsList().Count == 1 ? "" : "s")}");

        }

        [TestMethod]
        public void Comma_Seperated_Selectors() {
            LoadedStyles styles = LoadSSResource("style_commas.css");

            Assert.IsTrue(styles[0].Selector.Equals("#b"));    // #b is first because ids have the highest precedence
            Assert.IsTrue(styles[1].Selector.Equals(".c"));    // .c is second because classes have the second highest precedence  
            Assert.IsTrue(styles[2].Selector.Equals("red"));   // red, green blue appears before 'a'
            Assert.IsTrue(styles[3].Selector.Equals("green"));
            Assert.IsTrue(styles[4].Selector.Equals("blue"));
            Assert.IsTrue(styles[5].Selector.Equals("a"));


            foreach (Style style in styles.AsList()) {
                Debug.WriteLine($"{style.Selector} [{style.Specificity.DelString()}]");
            }
            Debug.WriteLine($"--------------------\n{styles.AsList().Count} Style{(styles.AsList().Count == 1 ? "" : "s")}");

        }
    }
}
