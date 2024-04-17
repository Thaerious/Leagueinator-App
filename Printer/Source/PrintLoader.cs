using Leagueinator.CSSParser;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Printer.Source {
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
        public static Element LoadResources(this Assembly assembly, string xmlName, string cssName) {
            var element = assembly.LoadXMLResource(xmlName);
            LoadedStyles ss = assembly.LoadSSResource(cssName);
            ss.ApplyTo(element);
            return element;
        }

        public static Element LoadXMLResource(this Assembly assembly, string xmlName) {
            Console.WriteLine(xmlName);
            using Stream? xmlStream = assembly.GetManifestResourceStream(xmlName) ?? throw new NullReferenceException($"Resource Not Found: {xmlName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return XMLLoader.Load(xmlText);
        }

        public static LoadedStyles LoadSSResource(this Assembly assembly, string cssName) {
            using Stream? xmlStream = assembly.GetManifestResourceStream(cssName) ?? throw new NullReferenceException($"Resource Not Found: {cssName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return LoadedStyles.LoadFromString("", xmlText);
        }
    }
}
