using Leagueinator.CSSParser;
using Leagueinator.Printer;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Leagueinator.Printer.Styles.Enums;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using static Test_Style.Loader;

namespace Test_Style {
    [TestClass]
    public class Style_Test {

        [TestMethod]
        public void Sanity() {
            LoadedStyles styles = LoadSSResource("style.css");
            Assert.IsNotNull(styles);

            foreach (Style style in styles) Console.WriteLine(style);
        }

        /// <summary>
        /// Elements without a selector (class, id) receive the wildcard styles.
        /// Background color is set, border color is not.
        /// </summary>
        [TestMethod]
        public void Apply_Styles_WildCard_Only() {
            Element xml = LoadResources("layout.xml", "style.css");
            Assert.AreEqual(System.Drawing.Color.WhiteSmoke, xml.Style.BackgroundColor);
            Assert.AreEqual(null, xml.Style.BorderColor);
        }

        ///// <summary>
        ///// Parameters can have a default value set in the CSS annotation, 
        ///// they will be used if the parameter is null after other propagation methods.
        ///// The default Style only exists on a Style that is on an xml element.
        ///// </summary>
        //[TestMethod]
        //public void Default_Values_Location() {
        //    Assert.AreEqual(new Point(0, 0), StyleSheet.Default.Translate);
        //}

        ///// <summary>
        ///// AsList elements inherit default values.
        ///// </summary>
        //[TestMethod]
        //public void Inherited_Default_Value() {
        //    Elements xml = LoadResources("layout.xml", "Style.css");
        //    Assert.AreEqual(new Point(0, 0), xml.StyleSheet.Translate);
        //}

        /// <summary>
        /// CSS properties marked 'inherited' will cause elements to inherit from their parent
        /// elements.
        /// </summary>
        [TestMethod]
        public void Inherited_Value() {
            Element xml = LoadResources("layout.xml", "style.css");
            Assert.AreEqual("arial", xml[".parent"][0].Style.FontFamily);
            Assert.AreEqual("arial", xml[".child"][0].Style.FontFamily);
            Assert.AreEqual("arial", xml["#deepchild"][0].Style.FontFamily);
        }

        [TestMethod]
        public void Width_Height() {
            Element xml = LoadResources("layout.xml", "style.css");
            Assert.AreEqual("50px", xml[".child"][0].Style.Width.ToString());
            Assert.AreEqual("75px", xml[".child"][0].Style.Height.ToString());
        }

        /// <summary>
        /// ID > Class
        /// </summary>
        [TestMethod]
        public void Specificity_ID_Class() {
            Element xml = LoadResources("specificity.xml", "specificity.css");
            Assert.AreEqual(Color.Green, xml["t1"][0].Style.BackgroundColor);
        }

        /// <summary>
        /// Class > Name
        /// </summary>
        [TestMethod]
        public void Specificity_Class_Name() {
            Element xml = LoadResources("specificity.xml", "specificity.css");
            Assert.AreEqual("10px", xml["t1"][0].Style.Height?.ToString());
        }

        /// <summary>
        /// Name > *
        /// </summary>
        [TestMethod]
        public void Specificity_Name_WildCard() {
            Element xml = LoadResources("specificity.xml", "specificity.css");
            Assert.AreEqual("50px", xml["t1"][0].Style.Width?.ToString());
        }

        /// <summary>
        /// Name.Class > Name
        /// </summary>
        [TestMethod]
        public void Specificity_NameClass_Name() {
            Element xml = LoadResources("specificity.xml", "specificity.css");

            LoadedStyles styles = LoadSSResource("specificity.css");
            //var sortedKeys = styles
            //                .OrderBy(pair => pair.Factor)
            //                .Select(pair => pair.Key)
            //                .ToList();

            int i = 0;
            foreach (Style style in styles) {
                Debug.WriteLine($"{i++}\t\"{style.Selector}\"\t[{style.Specificity.DelString()}]");
            }

            Assert.AreEqual(Flex_Axis.Column, xml["t1"][0].Style.Flex_Axis);
        }

        /// <summary>
        /// Name.Class > Class
        /// </summary>
        [TestMethod]
        public void Specificity_NameClass_Class() {
            Element xml = LoadResources("specificity.xml", "specificity.css");
            Assert.AreEqual("10px", xml["t1"][0].Style.Bottom?.ToString());
        }

        [TestMethod]
        public void Specificity_Order_Of_Appearance() {
            Element xml = LoadResources("specificity.xml", "specificity.css");
            Assert.AreEqual("30px", xml["t1"][0].Style.Left?.ToString());
        }

        /// <summary>
        /// Class > ElementName
        /// </summary>
        [TestMethod]
        public void Specificity() {
            LoadedStyles styles = LoadSSResource("specificity.css");

            int i = 0;
            foreach (Style style in styles) {
                Debug.WriteLine($"{i++}\t\"{style.Selector}\"\t[{style.Specificity.DelString()}]");
            }
        }


        /// <summary>
        /// Class > ElementName
        /// </summary>
        [TestMethod]
        public void Apply_Deep_Style() {
            LoadResources("layout.xml", "applyDeepStyle.css");
        }

        /// <summary>
        /// Class > ElementName
        /// </summary>
        [TestMethod]
        public void Default_Position() {
            LoadedStyles styles = LoadSSResource("empty.css");
            Element element = new("element", []);
            styles.ApplyTo(element);

            Assert.AreEqual(Position.Flex, element.Style.Position);
        }

        /// <summary>
        /// Selector: root *
        /// </summary>
        [TestMethod]
        public void Selector_Hierarchal_Wildcard() {
            LoadResources("selector.xml", "selector.css", out Element xml, out LoadedStyles styles);

            foreach (Style style in styles) {
                Debug.WriteLine(style);
            }

            Assert.AreEqual("50px", xml["t1"][0].Style.Width?.ToString());
            Assert.AreEqual("50px", xml["t2"][0].Style.Width?.ToString());
            Assert.AreEqual("50px", xml["t3"][0].Style.Width?.ToString());
        }
    }
}
