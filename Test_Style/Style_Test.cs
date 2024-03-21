using Leagueinator.CSSParser;
using Leagueinator.Printer;
using Leagueinator.Printer.Enums;
using System.Drawing;
using System.Reflection;

namespace Test_Style {
    [TestClass]
    public class Style_Test {

        public static Element LoadResources(string xmlName, string cssName) {
            var element = LoadXMLResource(xmlName);
            LoadedStyles ss = LoadSSResource(cssName);
            ss.ApplyTo(element);
            return element;
        }

        public static Element LoadXMLResource(string xmlName) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            xmlName = $"Test_Style.Assets.{xmlName}";
            Console.WriteLine(xmlName);
            using Stream? xmlStream = assembly.GetManifestResourceStream(xmlName) ?? throw new NullReferenceException($"Resource Not Found: {xmlName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return XMLLoader.Load(xmlText);
        }

        public static LoadedStyles LoadSSResource(string cssName) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            cssName = $"Test_Style.Assets.{cssName}";
            using Stream? xmlStream = assembly.GetManifestResourceStream(cssName) ?? throw new NullReferenceException($"Resource Not Found: {cssName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return StyleLoader.Load(xmlText);
        }

        [TestMethod]
        public void Sanity() {
            LoadedStyles styles = LoadSSResource("style.css");
            Assert.IsNotNull(styles);

            foreach (Style style in styles.Values) Console.WriteLine(style);
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
            Element xml = LoadResources("layout.xml", "style.css");
            Assert.AreEqual(new Point(0, 0), xml.Style.Location);
        }

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

        [TestMethod]
        public void Border() {
            Document document = Document.LoadAsset("Test_Style.Assets.border.xml", Assembly.GetExecutingAssembly());
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
        /// Name > WildCard
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

            styles.OrderBy(pair => pair.Value)
                  .ToList()
                  .ForEach(pair => Console.WriteLine(pair.Value));
        }

    }
}
