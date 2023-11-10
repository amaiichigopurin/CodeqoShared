
using Glitch9.Localization;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace CodeqoEditor
{
    public class NationalFlag
    {
        private const string ASSET_PATH = "Shared/Gizmos/Flags/";
        private static string GetPath(string assetName) => CUIUtility.GetCodeqoPath() + "/" + ASSET_PATH + assetName;
        

        private static readonly Dictionary<SystemLanguage, List<string>> languageCountryMap = new Dictionary<SystemLanguage, List<string>>()
        {
            { SystemLanguage.English, new List<string> { "United States", "United Kingdom", "Canada", "Australia" } },
            { SystemLanguage.French, new List<string> { "France", "Belgium", "Canada" } },
            { SystemLanguage.Spanish, new List<string> { "Spain", "Mexico", "Argentina" } },
            // ... Add other languages and corresponding countries
        };

        public static List<string> GetCountriesFromLanguage(SystemLanguage language)
        {
            if (languageCountryMap.TryGetValue(language, out var countries))
            {
                return countries;
            }

            Debug.LogWarning($"No country mapping found for language: {language}");
            return new List<string>(); // Return an empty list or null, based on how you want to handle this case.
        }
        
        public static Texture2D GetFlag(SystemLanguage language)
        {
            GNLocaleIdentifier identifier = new GNLocaleIdentifier(language);
            string country = identifier.GetCountryName();
            Debug.Log(country);
            return AssetDatabase.LoadAssetAtPath<Texture2D>(GetPath(country));
        }
    }
}
