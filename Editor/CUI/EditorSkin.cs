using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    public class EditorSkin
    {
        private const string ASSET_PATH = "Shared/Gizmos/Skins/";
        private const string SKIN_NAME_LIGHT = "CSkin_Light.guiskin";
        private const string SKIN_NAME_DARK = "CSkin_Dark.guiskin";

        private static string GetPath(string assetName) => CUIUtility.GetCodeqoPath() + "/" + ASSET_PATH + assetName;

        private static GUISkin _skinLightDefault;
        private static GUISkin skinLightDefault
        {
            get
            {
                if (_skinLightDefault == null) _skinLightDefault = AssetDatabase.LoadAssetAtPath<GUISkin>(GetPath(SKIN_NAME_LIGHT));
                if (_skinLightDefault == null) return GUI.skin;
                return _skinLightDefault;
            }
        }
        private static GUISkin _skinDarkDefault;
        private static GUISkin skinDarkDefault
        {
            get
            {
                if (_skinDarkDefault == null) _skinDarkDefault = AssetDatabase.LoadAssetAtPath<GUISkin>(GetPath(SKIN_NAME_DARK));
                if (_skinLightDefault == null) return GUI.skin;
                return _skinDarkDefault;
            }
        }       


        public static GUISkin skin
        {
            get
            {
                if (EditorGUIUtility.isProSkin) return skinDarkDefault;
                else return skinLightDefault;
            }
        }
    }
}
