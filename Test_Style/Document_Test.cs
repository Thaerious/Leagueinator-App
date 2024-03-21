using Leagueinator.Printer;
using System.Reflection;

namespace Test_Style {
    [TestClass]
    public class Document_Test {

        [TestMethod]
        public void Sanity() {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Document document = Document.LoadAsset("Test_Style.Assets.document.xml", assembly);
            Assert.IsNotNull(document);
        }
    }
}
