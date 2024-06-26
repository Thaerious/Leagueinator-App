﻿using Leagueinator.CSSParser;
using Leagueinator.Utility;
using System.Reflection;

namespace Leagueinator.Printer.Styles {
    public partial class Style {
        static Style() {

            PropertyInfo[] properties = typeof(Style).GetProperties();

            Dictionary<string, PropertyInfo> pDict = [];
            Dictionary<string, PropertyInfo> ipDict = [];

            foreach (var property in properties) {
                if (property.GetCustomAttribute<CSS>() == null) continue;
                if (property.CanWrite && property.CanRead) pDict[property.Name.ToFlatCase()] = property;

                if (property.GetCustomAttribute<InheritedAttribute>() == null) continue;
                ipDict[property.Name.ToLower()] = property;
            }

            Style.CSSProperties = pDict.AsReadOnly();
            Style.InheritedProperties = ipDict.AsReadOnly();
        }

        public static IReadOnlyDictionary<string, PropertyInfo> CSSProperties { get; private set; }
        public static IReadOnlyDictionary<string, PropertyInfo> InheritedProperties { get; private set; }
    }
}
