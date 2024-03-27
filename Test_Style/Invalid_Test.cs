using Leagueinator.Printer;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Query;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using static Test_Style.Loader;

namespace Test_Style {
    [TestClass]
    public class Invalid_Test {
        /// <summary>
        /// New elements are _invalid on inception.
        /// </summary>
        [TestMethod]
        public void Sanity() {
            Element root = LoadResources("layout.xml", "style.css");
            Assert.IsTrue(root.Invalid);
        }

        /// <summary>
        /// Drawing an element will cause it (and it's decendents) to be layed out, and therefore be valid.
        /// </summary>
        [TestMethod]
        public void On_Draw() {
            Element root = LoadResources("layout.xml", "style.css");
            Bitmap bitmap = new Bitmap(850, 1100);

            using Graphics graphics = Graphics.FromImage(bitmap);
            root.Draw(graphics, 0);

            Assert.IsFalse(root.Invalid);            
            root.Draw(graphics, 0);
            Assert.IsFalse(root.Invalid);
        }

        /// <summary>
        /// Modifying a style will cause the document tree to become _invalid.
        /// </summary>
        [TestMethod]
        public void Modify_Tree() {
            Element root = LoadResources("layout.xml", "style.css");
            Bitmap bitmap = new Bitmap(850, 1100);

            using Graphics graphics = Graphics.FromImage(bitmap);
            root.Draw(graphics, 0);

            Assert.IsFalse(root.Invalid);

            Element child = root["#deepchild"][0];

            var didParse = Cardinal<UnitFloat>.TryParse("15px", out Cardinal<UnitFloat> target);
            child.Style.BorderSize = target;

            Assert.IsTrue(didParse);
            Assert.IsTrue(root.Invalid);
        }

        [TestMethod]
        public void Dummy() {
            Dummy dummy = new Dummy();
            dummy.HelloWorld();
            dummy.Foo = true;
            dummy.Bar = true;
        }
    }
}
