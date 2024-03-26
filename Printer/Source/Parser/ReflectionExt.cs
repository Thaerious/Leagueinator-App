using System.Collections.ObjectModel;
using System.Reflection;
using Leagueinator.Utility;

namespace Leagueinator.CSSParser {
    public static class ReflectionExt {
        /// <summary>
        /// ToDictionary - Flat Case Dictionary
        /// 
        /// A dictionary with string keys, in which each key is stored in flat case.
        /// 
        /// When there is no specific casing convention applied to a sequence of words, 
        /// and the words are concatenated together without any spaces, hyphens, or 
        /// capitalization to differentiate them, it's often referred to as "flat case" 
        /// or "nocase". In this convention, all letters are in lowercase, and words are
        /// directly joined together without any additional characters or capitalization
        /// to mark word boundaries.
        /// </summary>
        /// <param name="fields">Array source, the TagName field of FieldInfo is used for
        /// the key.
        /// </param>
        /// <returns></returns>
        public static ReadOnlyDictionary<string, FieldInfo> ToDictionary(this FieldInfo[] fields) {
            Dictionary<string, FieldInfo> dictionary = new();

            foreach (var field in fields) {
                dictionary[field.Name.ToFlatCase()] = field;
            }

            return new(dictionary);
        }

        /// <summary>
        /// ToDictionary - Flat Case Dictionary
        /// 
        /// A dictionary with string keys, in which each key is stored in flat case.
        /// 
        /// When there is no specific casing convention applied to a sequence of words, 
        /// and the words are concatenated together without any spaces, hyphens, or 
        /// capitalization to differentiate them, it's often referred to as "flat case" 
        /// or "nocase". In this convention, all letters are in lowercase, and words are
        /// directly joined together without any additional characters or capitalization
        /// to mark word boundaries.
        /// </summary>
        /// <param name="fields">Array source, the TagName field of PropertyInfo is used for
        /// the key.
        /// </param>
        /// <returns></returns>
        public static ReadOnlyDictionary<string, PropertyInfo> ToDictionary(this PropertyInfo[] properties) {
            Dictionary<string, PropertyInfo> dictionary = new();

            foreach (var property in properties) {
                dictionary[property.Name.ToFlatCase()] = property;
            }

            return new(dictionary);
        }
    }
}
