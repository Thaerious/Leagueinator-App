using Leagueinator.CSSParser;
using Leagueinator.Printer;
using System.Drawing;
using System.Reflection;

namespace Test_Style {
    [TestClass]
    public class Style_Test {

        public static LoadedStyles LoadStyleResource(Assembly assembly, string resourceName) {
            using Stream? styleStream = assembly.GetManifestResourceStream(resourceName) ?? throw new NullReferenceException($"Resource Not Found: {resourceName}");
            using StreamReader styleReader = new StreamReader(styleStream);
            string styleText = styleReader.ReadToEnd();
            return StyleLoader.Load(styleText);
        }

        public Element LoadXMLResource(Assembly assembly, string resourceName) {
            using Stream? xmlStream = assembly.GetManifestResourceStream(resourceName) ?? throw new NullReferenceException($"Resource Not Found: {resourceName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return XMLLoader.Load(xmlText);
        }

        [TestMethod]
        public void Sanity() {
            LoadedStyles styles = LoadStyleResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.style.css");
            Assert.IsNotNull(styles);
        }

        [TestMethod]
        public void WildCard() {
            LoadedStyles styles = LoadStyleResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.style.css");
            Console.WriteLine(styles["*"]);
            Assert.IsNotNull(styles["*"]);
        }

        [TestMethod]
        public void WildCard_Background_Color() {
            LoadedStyles styles = LoadStyleResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.style.css");
            Console.WriteLine(styles["*"]);
            Assert.AreEqual(System.Drawing.Color.WhiteSmoke, styles["*"].BackgroundColor);
        }

        /// <summary>
        /// Elements without a selector (class, id) receive the wildcard styles.
        /// Background color is set, border color is not.
        /// </summary>
        [TestMethod]
        public void Apply_Styles_WildCard_Only() {
            LoadedStyles styles = LoadStyleResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.style.css");
            Element xml = this.LoadXMLResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.layout.xml");
            styles.ApplyTo(xml);

            Console.WriteLine(styles["*"]);
            Console.WriteLine(xml.Style);
            Assert.AreEqual(System.Drawing.Color.WhiteSmoke, xml.Style.BackgroundColor);
            Assert.AreEqual(null, xml.Style.BorderColor);
        }

        /// <summary>
        /// Parameters can have a default value set in the CSS annotation, 
        /// they will be used if the parameter is null after other propagation methods.
        /// The default style only exists on a style that is on an xml element.
        /// </summary>
        [TestMethod]
        public void Default_Values_Location() {
            Assert.AreEqual(new Point(0, 0), Style.Default.Location);
        }

        /// <summary>
        /// All elements inherit default values.
        /// </summary>
        [TestMethod]
        public void Inherited_Default_Value() {
            LoadedStyles styles = LoadStyleResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.style.css");
            Element xml = this.LoadXMLResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.layout.xml");
            styles.ApplyTo(xml);

            Console.WriteLine(xml.Style);
            Assert.AreEqual(new Point(0, 0), xml.Style.Location);
        }

        /// <summary>
        /// CSS properties marked 'inherited' will cause elements to inherit from their parent
        /// elements.
        /// </summary>
        [TestMethod]
        public void Inherited_Value() {
            LoadedStyles styles = LoadStyleResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.style.css");
            Element xml = this.LoadXMLResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.layout.xml");

            styles.ApplyTo(xml);

            //Console.WriteLine(styles[".parent"]);
            //Console.WriteLine(xml[".parent"][0].Style);
            //Console.WriteLine(xml[".child"][0].Style);
            //Console.WriteLine(xml["#deepchild"][0].Style);

            //Assert.AreEqual("arial", xml[".parent"][0].Style.FontFamily);
            Assert.AreEqual("arial", xml[".child"][0].Style.FontFamily);
            //Assert.AreEqual("arial", xml["#deepchild"][0].Style.FontFamily);
        }

        [TestMethod]
        public void Width_Height() {
            LoadedStyles styles = LoadStyleResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.style.css");
            Element xml = this.LoadXMLResource(Assembly.GetExecutingAssembly(), "Test_Style.Assets.layout.xml");

            styles.ApplyTo(xml);
            Console.WriteLine(xml[".child"][0].Style);

            Assert.AreEqual("50px", xml[".child"][0].Style.Width.ToString());
            Assert.AreEqual("75px", xml[".child"][0].Style.Height.ToString());
        }
    }
}
