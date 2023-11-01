using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    [CreateAssetMenu(fileName = "EditorSkin", menuName = "Codeqo/Editor/EditorSkin")]
    public class EditorSkin : ScriptableObject
    {
        public GUISkin skinLightDefault;
        public GUISkin skinDarkDefault;

        
        private static EditorSkin instance;
        public static EditorSkin Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<EditorSkin>("EditorSkin");
                }
                return instance;
            }
        }

        public static GUISkin skin
        {
            get
            {
                if (EditorGUIUtility.isProSkin) return Instance.skinDarkDefault;
                else return Instance.skinLightDefault;
            }
        }
    }
}
