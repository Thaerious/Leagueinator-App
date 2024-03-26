using Leagueinator.Printer;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Query;
using System.Diagnostics;
using System.Reflection;

namespace Test_Style {
    [TestClass]
    public class QueryEngine_Test {
        public static Element LoadXMLResource(string resourceName) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            resourceName = $"Test_Style.Assets.{resourceName}";
            using Stream? xmlStream = assembly.GetManifestResourceStream(resourceName) ?? throw new NullReferenceException($"Resource Not Found: {resourceName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return XMLLoader.Load(xmlText);
        }

        [TestMethod]
        public void Sanity() {
            QueryEngine q = new();
            Assert.IsNotNull(q);
        }

        [TestMethod]
        public void AddElement_Contains() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_0.xml");
            q.Add(root);

            Assert.IsTrue(q.Contains(root));
            Assert.AreEqual(1, q.Count);
        }

        [TestMethod]
        public void Does_Not_Contain() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_0.xml");
            Assert.IsFalse(q.Contains(root));
        }

        [TestMethod]
        public void Add_All() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_0.xml");
            q.AddAll(root);
            Assert.AreEqual(9, q.Count);
        }

        [TestMethod]
        public void Lookup_Tag_Name() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_0.xml");
            q.AddAll(root);
            var r = q.Lookup("div");
            Assert.AreEqual(3, r.Count);
        }

        [TestMethod]
        public void Lookup_Class_Name_Multiple() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_0.xml");
            q.AddAll(root);
            var r = q.Lookup(".child");
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void Lookup_Class_Name_Single() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_0.xml");
            q.AddAll(root);
            var r = q.Lookup(".deep");
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void Lookup_ID_Single() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_0.xml");
            q.AddAll(root);
            var r = q.Lookup("#first");
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void Lookup_ID_Multiple() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_0.xml");
            q.AddAll(root);
            var r = q.Lookup("#third");
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void Lookup_Class_Name_None() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_0.xml");
            q.AddAll(root);
            var r = q.Lookup(".none");
            Assert.AreEqual(0, r.Count);
        }

        [TestMethod]
        public void Query_Single() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_0.xml");
            q.AddAll(root);
            var r = q["div"];
            Assert.AreEqual(3, r.Count);
        }

        [TestMethod]
        public void Query_Any_Child() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_1.xml");
            q.AddAll(root);
            var r = q["first_tier second_tier"];

            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(3, r.Count);
        }

        [TestMethod]
        public void Query_Immediate_Child() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_1.xml");
            q.AddAll(root);
            var r = q["first_tier > second_tier"];

            Debug.WriteLine("---------------------");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void Query_By_Class() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_1.xml");
            q.AddAll(root);
            var r = q["div .match_this"];

            Debug.WriteLine("---------------------");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void Query_By_ID() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_1.xml");
            q.AddAll(root);
            var r = q["div #2t4"];

            Debug.WriteLine("- matches -");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void Query_By_Name_And_Class() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_1.xml");
            q.AddAll(root);
            var r = q["first_tier.select_me"];

            Debug.WriteLine("- matches -");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void Deep_Query_By_Name_And_Class_Mid() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_3.xml");
            q.AddAll(root);
            var r = q["div mid.select target"];

            Debug.WriteLine("- matches -");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void Deep_Query_By_Name_And_Class() {
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_3.xml");
            q.AddAll(root);
            var r = q["mid.select target"];

            Debug.WriteLine("- matches -");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(2, r.Count);
        }


        [TestMethod]
        public void Empty_Query_By_ID() {
            // id exists, but parent branch does not match
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_1.xml");
            q.AddAll(root);
            var r = q["div #2t3"];

            Debug.WriteLine("- matches -");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(0, r.Count);
        }

        [TestMethod]
        public void Deep_Query() {
            // id exists, but parent branch does not match
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_1.xml");
            q.AddAll(root);
            var r = q["root first_tier fourth_tier"];

            Debug.WriteLine("- matches -");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(1, r.Count);
        }

        /// <summary>
        /// Select all elements with div as a direct parent.
        /// </summary>
        [TestMethod]
        public void Div_Direct_Parent() {
            // id exists, but parent branch does not match
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_2.xml");
            q.AddAll(root);
            var r = q["div > *"];

            Debug.WriteLine("- matches -");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void Multiple_Select_TagName() {
            // id exists, but parent branch does not match
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_1.xml");
            q.AddAll(root);
            var r = q["second_tier, third_tier"];

            Debug.WriteLine("- matches -");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(6, r.Count);
        }

        [TestMethod]
        public void Multiple_Select_Complex() {
            // id exists, but parent branch does not match
            QueryEngine q = new();
            Element root = LoadXMLResource("query_engine_1.xml");
            q.AddAll(root);
            var r = q["second_tier, third_tier.match_this"];

            Debug.WriteLine("- matches -");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(5, r.Count);
        }

        [TestMethod]
        public void Query_All() {
            // id exists, but parent branch does not match
            QueryEngine q = new();
            Element root = LoadXMLResource("layout.xml");
            q.AddAll(root);
            var r = q["*"];

            Debug.WriteLine("- matches -");
            r.ForEach(x => Debug.WriteLine(x.Identifier));

            Assert.AreEqual(4, r.Count);
        }

    }
}
