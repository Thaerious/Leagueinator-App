using Leagueinator.CSSParser;
using System;
using System.IO;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Leagueinator.Printer {

    public class Document {
        public Element Root { get; private set; }
        public Element Body { get; private set; }
        public readonly LoadedStyles Styles = [];

        private readonly Assembly? SourceAssembly;

        public static Document LoadAsset(string xmlName, Assembly assembly) {
            assembly ??= Assembly.GetEntryAssembly() ?? throw new NullReferenceException("Assembly is NULL");

            using Stream? xmlStream = assembly.GetManifestResourceStream(xmlName) ?? throw new NullReferenceException($"Resource Not Found: {xmlName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();

            return new Document(xmlText, assembly);
        }

        public static Document LoadFile(string filepath) {
            return new Document(
                File.ReadAllText(filepath),
                Path.GetDirectoryName(filepath) ?? "./"
            );
        }

        public Document(string source, Assembly assembly) {
            this.SourceAssembly = assembly;
            this.Load(source);
            this.Root ??= new("");
            this.Body ??= new("");
        }

        public Document(string source, string relativePath = "./") {
            this.SourceAssembly = Assembly.GetEntryAssembly();
            this.Load(source);
            this.Root ??= new("");
            this.Body ??= new("");
        }

        private void Load(string source, string relativePath = "./") {
            this.Root = XMLLoader.Load(source);
            this.Body = this.Root["body"][0];
            this.Root.Detach(this.Body);

            foreach (Element e in this.Root["head > stylesheet"]) {
                if (e.Attributes.TryGetValue("asset", out string? value1)) {
                    this.LoadStyleAsset(value1);
                }

                if (e.Attributes.TryGetValue("file", out string? value2)) {
                    var loadedStyles = this.LoadStyleFile(Path.Combine(relativePath, value2));

                    foreach (Style style in loadedStyles.AsList()) {
                        this.Styles.Add(style);
                    }
                }
            }

            foreach (Element e in this.Root["head > style"]) {
                foreach (Style style in StyleLoader.Load(e.InnerText ?? "").AsList()) {
                    this.Styles.Add(style);
                }
            }
        }

        public void ApplyStyles() {
            this.Styles.ApplyTo(this.Body);
        }

        private LoadedStyles LoadStyleFile(string path) {
            string text = File.ReadAllText(path);
            return StyleLoader.Load(text);
        }

        private LoadedStyles LoadStyleAsset(string path) {
            using Stream? xmlStream = this.SourceAssembly.GetManifestResourceStream(path) ?? throw new NullReferenceException($"Resource Not Found: {path}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return StyleLoader.Load(xmlText);
        }
    }
}
