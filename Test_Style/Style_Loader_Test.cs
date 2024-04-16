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

            foreach (Style style in styles) {
                Debug.WriteLine($"{style.Selector} [{style.Specificity.DelString()}]");
            }
            Debug.WriteLine($"--------------------\n{styles.Count} Style{(styles.Count == 1 ? "" : "s")}");

            Assert.IsTrue(styles.Any(style => style.Selector.Equals(".parent")));
            Assert.IsTrue(styles.Any(style => style.Selector.Equals(".child")));
            Assert.IsTrue(styles.Any(style => style.Selector.Equals("parent > child > deepchild")));
            Assert.IsTrue(styles.Any(style => style.Selector.Equals("*")));

        }

        [TestMethod]
        public void Comma_Seperated_Selectors() {
            LoadedStyles styles = LoadSSResource("style_commas.css");

            Assert.IsTrue(styles.Any(style => style.Selector.Equals("#b")));

            Assert.IsTrue(styles.Any(style => style.Selector.Equals("#b")));    // #b is first because ids have the highest precedence
            Assert.IsTrue(styles.Any(style => style.Selector.Equals(".c")));    // .c is second because classes have the second highest precedence  
            Assert.IsTrue(styles.Any(style => style.Selector.Equals("red")));   // red, green blue appears before 'a'
            Assert.IsTrue(styles.Any(style => style.Selector.Equals("green")));
            Assert.IsTrue(styles.Any(style => style.Selector.Equals("blue")));
            Assert.IsTrue(styles.Any(style => style.Selector.Equals("a")));


            foreach (Style style in styles) {
                Debug.WriteLine($"{style.Selector} [{style.Specificity.DelString()}]");
            }
            Debug.WriteLine($"--------------------\n{styles.Count} Style{(styles.Count == 1 ? "" : "s")}");
        }
    }
}
