using Leagueinator.Printer.Elements;
using Leagueinator.Printer;
using System.Reflection;
using Leagueinator.Printer.Styles;
using System.IO;

namespace Printer {
    public static class PrintLoader {
        /// <summary>
        /// Load an xml and style resource from the assembly.  Applies styles to the elements.
        /// Set the resource to 'embedded' in the resource properties, or in the csproj file (see below).
        /// The resource name must be prefixed with the root namespace, which is usually the project name; but may
        /// be set with <RootNamespace>MyProject</RootNamespace> in the csproj file.
        /// When debugging in code use the following to list all asset names:  assembly.GetManifestResourceNames()
        /// 
        /// Manually including embedded assets:
        ///   <ItemGroup>
        ///       <EmbeddedResource Include = "xmlName" />
        ///       <EmbeddedResource Include = "cssName" />
        ///   </ItemGroup>
        /// </summary>
        /// 
        /// Example:
        /// assembly.LoadResources("Project.Assets.xmlName", "Prject.Assets.cssName");
        /// 
        /// <param name="assembly"></param>
        /// <param name="xmlName"></param>
        /// <param name="cssName"></param>
        /// <returns>The root element</returns>
        public static T LoadResources<T>(this Assembly assembly, string xmlName, string cssName) where T : Element, new() {
            T element = assembly.LoadXMLResource<T>(xmlName);
            LoadedStyles ss = assembly.LoadStyleResource(cssName);
            ss.AssignTo(element);
            return element;
        }

        public static T LoadXMLResource<T>(this Assembly assembly, string xmlName) where T : Element, new() {
            using Stream? xmlStream = assembly.GetManifestResourceStream(xmlName) ?? throw new NullReferenceException($"Resource Not Found: {xmlName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            XMLLoader.Load(xmlText, out T t);
            return t;
        }

        public static LoadedStyles LoadStyleResource(this Assembly assembly, string cssName) {
            using Stream? xmlStream = assembly.GetManifestResourceStream(cssName) ?? throw new NullReferenceException($"Resource Not Found: {cssName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return LoadedStyles.LoadFromString("", xmlText);
        }
    }
}
