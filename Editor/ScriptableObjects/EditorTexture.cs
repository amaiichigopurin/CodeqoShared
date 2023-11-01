using UnityEngine;

namespace CodeqoEditor
{
    [CreateAssetMenu(fileName = "EditorTexture", menuName = "Codeqo/Editor/EditorTexture")]
    public class EditorTexture : ScriptableObject
    {
        [Header("Box")]
        public EditorTexture2D boxDefault;
        public Texture2D boxGreen;
        public Texture2D boxYellow;
        public Texture2D boxOrange;
        public Texture2D boxPuple;
        public Texture2D boxBlue;

        [Header("TextField")]
        public EditorTexture2D textFieldDefault;
        public Texture2D textFieldGreen;
        public Texture2D textFieldYellow;
        public Texture2D textFieldOrange;
        public Texture2D textFieldPuple;
        public Texture2D textFieldBlue;

        [Header("Border")]
        public EditorTexture2D borderTop;
        public EditorTexture2D borderBottom;

        [Header("ToolBar Button")]
        public Texture2D toolBarButtonOn;
        public Texture2D toolBarButtonOff;

        [Header("Extra")]
        public Texture2D config;
        public Texture2D background;
        public Texture2D noImageHighRes;
        public Texture2D android12Circle;


        private static EditorTexture instance;
        public static EditorTexture Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<EditorTexture>("EditorTexture");
                }
                return instance;
            }
        }

        public static Texture2D ToolBarButtonOn => Instance.toolBarButtonOn;
        public static Texture2D ToolBarButtonOff => Instance.toolBarButtonOff;
        public static Texture2D BorderTop => Instance.borderTop;
        public static Texture2D BorderBottom => Instance.borderBottom;
        public static Texture2D Config => Instance.config;
        public static Texture2D Background => Instance.background;
        public static Texture2D NoImageHighRes => Instance.noImageHighRes;
        public static Texture2D Android12Circle => Instance.android12Circle;

        public static Texture2D Box(CUIColor color = 0)
        {
            return color switch
            {
                CUIColor.Green => Instance.boxGreen,
                CUIColor.Yellow => Instance.boxYellow,
                CUIColor.Orange => Instance.boxOrange,
                CUIColor.Purple => Instance.boxPuple,
                CUIColor.Blue => Instance.boxBlue,
                _ => (Texture2D)Instance.boxDefault,
            };
        }

        public static Texture2D TextField(CUIColor color = 0)
        {
            return color switch
            {
                CUIColor.Green => Instance.textFieldGreen,
                CUIColor.Yellow => Instance.textFieldYellow,
                CUIColor.Orange => Instance.textFieldOrange,
                CUIColor.Purple => Instance.textFieldPuple,
                CUIColor.Blue => Instance.textFieldBlue,
                _ => Instance.textFieldDefault,
            };
        }
    }
}
