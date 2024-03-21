using Leagueinator.CSSParser;
using Leagueinator.Printer;
using System.Diagnostics;
using System.Reflection;

namespace Time.Style {

    public class StyleTimeTest {

        public static Element LoadResources(string xmlName, string cssName) {
            var element = LoadXMLResource(xmlName);
            LoadedStyles ss = LoadSSResource(cssName);
            ss.ApplyTo(element);
            return element;
        }

        public static Element LoadXMLResource(string xmlName) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            xmlName = $"Time_Style.Assets.{xmlName}";
            using Stream? xmlStream = assembly.GetManifestResourceStream(xmlName) ?? throw new NullReferenceException($"Resource Not Found: {xmlName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return XMLLoader.Load(xmlText);
        }

        public static LoadedStyles LoadSSResource(string cssName) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            cssName = $"Time_Style.Assets.{cssName}";
            using Stream? xmlStream = assembly.GetManifestResourceStream(cssName) ?? throw new NullReferenceException($"Resource Not Found: {cssName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return StyleLoader.Load(xmlText);
        }

        public static void Main() {
            long time = 0;
            long count = 1000;

            for (int i = 0; i < count; i++) {
                Stopwatch stopwatch = Stopwatch.StartNew();
                Element root = LoadResources("layout.xml", "style.css");
                stopwatch.Stop();
                time += stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"{i} : Run Time {stopwatch.ElapsedMilliseconds} ms");
            }

            Console.WriteLine($"Average Run Time {time / count} ms");            
            Console.WriteLine($"Press <Enter>");
            Console.ReadLine();
        }
    }
}
