using UnityEngine;

namespace CodeqoEditor
{
    [CreateAssetMenu(fileName = "EditorIcon", menuName = "Codeqo/Editor/EditorIcon")]
    public class EditorIcon : ScriptableObject
    {
        [Header("Basic")]
        public Texture2D moveUp;
        public Texture2D moveDown;

        [Header("Media")]
        public Texture2D play;
        public Texture2D pause;
        public Texture2D stop;
        public Texture2D next;
        public Texture2D previous;
        public Texture2D fastForward;
        public Texture2D rewind;
        public Texture2D volume;
        public Texture2D seekBar;
        public Texture2D shuffle;
        public Texture2D repeat;
        public Texture2D repeatOne;

        [Header("Text")]
        public Texture2D textDefault;
        public Texture2D textDescription;
        public Texture2D textTitle;

        [Header("Mobile")]
        public Texture2D phone;

        [Header("File")]
        public Texture2D addFile;
        public Texture2D deleteFile;
        public Texture2D checkFile;
        public Texture2D editFile;
        public Texture2D exportCSV;
        public Texture2D importCSV;

        [Header("Editor")]
        public Texture2D enter;


        private static EditorIcon instance;
        public static EditorIcon Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<EditorIcon>("EditorIcon");
                }
                return instance;
            }
        }

        // Basic
        public static Texture2D MoveUp => Instance.moveUp;
        public static Texture2D MoveDown => Instance.moveDown;

        // Media
        public static Texture2D Play => Instance.play;
        public static Texture2D Pause => Instance.pause;
        public static Texture2D Stop => Instance.stop;
        public static Texture2D Next => Instance.next;
        public static Texture2D Previous => Instance.previous;
        public static Texture2D FastForward => Instance.fastForward;
        public static Texture2D Rewind => Instance.rewind;
        public static Texture2D Volume => Instance.volume;
        public static Texture2D SeekBar => Instance.seekBar;
        public static Texture2D Shuffle => Instance.shuffle;
        public static Texture2D Repeat => Instance.repeat;
        public static Texture2D RepeatOne => Instance.repeatOne;

        // Text
        public static Texture2D TextDefault => Instance.textDefault;
        public static Texture2D TextDescription => Instance.textDescription;
        public static Texture2D TextTitle => Instance.textTitle;

        // Mobile
        public static Texture2D Phone => Instance.phone;

        // File
        public static Texture2D AddFile => Instance.addFile;
        public static Texture2D DeleteFile => Instance.deleteFile;
        public static Texture2D CheckFile => Instance.checkFile;
        public static Texture2D EditFile => Instance.editFile;
        public static Texture2D ExportCSV => Instance.exportCSV;
        public static Texture2D ImportCSV => Instance.importCSV;

        // Editor
        public static Texture2D Enter => Instance.enter;
    }
}
