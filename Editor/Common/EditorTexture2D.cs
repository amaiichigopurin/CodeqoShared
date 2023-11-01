
using System;
using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    [Serializable]
    public class EditorTexture2D
    {
        public Texture2D light;
        public Texture2D dark;

        public EditorTexture2D(Texture2D light, Texture2D dark)
        {
            this.light = light;
            this.dark = dark;
        }

        public static implicit operator Texture2D(EditorTexture2D editorTexture2D)
        {
            if (EditorGUIUtility.isProSkin) return editorTexture2D.dark;
            else return editorTexture2D.light;
        }
    }    
}
