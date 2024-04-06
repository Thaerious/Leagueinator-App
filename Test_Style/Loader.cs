using Leagueinator.CSSParser;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer;
using System.Reflection;

namespace Test_Style {
    public static class Loader {
        public static Element LoadResources(string xmlName, string cssName) {
            var element = LoadXMLResource(xmlName);
            LoadedStyles ss = LoadSSResource(cssName);
            ss.ApplyTo(element);
            return element;
        }
        public static void LoadResources(string xmlName, string cssName, out Element element, out LoadedStyles styles) {
            element = LoadXMLResource(xmlName);
            styles = LoadSSResource(cssName);
            styles.ApplyTo(element);
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
    }
}
