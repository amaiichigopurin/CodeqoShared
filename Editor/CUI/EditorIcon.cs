using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    /// <summary>
    /// 과거에는 Texture2D를 리턴하지만 GUIContent를 리턴하도록 수정한다
    /// GUIContent의 사이즈를 미리 정해놓는다
    /// Icon의 사이즈는 16x16
    /// </summary>
    public class EditorIcon
    {
        private const string ASSET_PATH = "Shared/Gizmos/Icons/";
        private const string DIR_ARROW = "Arrow/";
        private const string DIR_FILE = "File/";
        private const string DIR_EXTRA = "Extra/";

        private const int ICON_SIZE = 16;
        private static string GetPath(string assetName) => CUIUtility.GetCodeqoPath() + "/" + ASSET_PATH + assetName;

        private static Dictionary<string, GUIContent> _cache = new Dictionary<string, GUIContent>();
        private static GUIContent GetCached(string directory, string assetName)
        {
            string key = directory + "_" + assetName;
            if (!_cache.TryGetValue(key, out var guiContent))
            {
                Texture2D originalTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetPath(directory + assetName));

                // Create a RenderTexture with 16x16 size
                RenderTexture rt = RenderTexture.GetTemporary(ICON_SIZE, ICON_SIZE);
                RenderTexture.active = rt;

                // Blit the original texture to the RenderTexture, effectively resizing it
                Graphics.Blit(originalTexture, rt);

                // Create a new Texture2D and read the resized pixels from the RenderTexture
                Texture2D resizedTexture = new Texture2D(ICON_SIZE, ICON_SIZE);
                resizedTexture.ReadPixels(new Rect(0, 0, ICON_SIZE, ICON_SIZE), 0, 0);
                resizedTexture.Apply();

                // Clean up the RenderTexture
                RenderTexture.active = null;
                RenderTexture.ReleaseTemporary(rt);

                guiContent = new GUIContent(resizedTexture);
                _cache.Add(key, guiContent);
            }
            return guiContent;
        }


        private static GUIContent moveUp => GetCached(DIR_ARROW, "16_up.png");
        private static GUIContent moveDown => GetCached(DIR_ARROW, "16_down.png");        

        private static GUIContent addFile => GetCached(DIR_FILE, "add-file.png");
        private static GUIContent deleteFile => GetCached(DIR_FILE, "delete-file.png");
        private static GUIContent checkFile => GetCached(DIR_FILE, "check-file.png");
        private static GUIContent editFile => GetCached(DIR_FILE, "edit-file.png");
        private static GUIContent exportCSV => GetCached(DIR_FILE, "export-csv.png");
        private static GUIContent importCSV => GetCached(DIR_FILE, "import-csv.png");

        private static GUIContent enter => GetCached(DIR_EXTRA, "ic_enter.png");
        private static GUIContent phone => GetCached(DIR_EXTRA, "ic_phone.psd");
        private static GUIContent config => GetCached(DIR_EXTRA, "ic_config.psd");
        private static GUIContent openFolder => GetCached(DIR_EXTRA, "ic_open_folder.png");
        private static GUIContent list => GetCached(DIR_EXTRA, "ic_list.png");


        // Arrow
        public static GUIContent MoveUp => moveUp;
        public static GUIContent MoveDown => moveDown;

        // File
        public static GUIContent AddFile => addFile;
        public static GUIContent DeleteFile => deleteFile;
        public static GUIContent CheckFile => checkFile;
        public static GUIContent EditFile => editFile;
        public static GUIContent ExportCSV => exportCSV;
        public static GUIContent ImportCSV => importCSV;

        // Extra
        public static GUIContent Phone => phone;
        public static GUIContent Config => config;
        public static GUIContent Enter => enter;
        public static GUIContent OpenFolder => openFolder;
        public static GUIContent List => list;
    }
}
