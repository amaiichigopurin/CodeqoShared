using Codeqo.NativeMediaPlayer.UI;
using UnityEngine;

namespace CodeqoEditor
{
    [CreateAssetMenu(fileName = "EditorIcon", menuName = "Codeqo/Editor/EditorIcon")]
    public class EditorIcon : ScriptableObject
    {
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
        public Texture2D close;

        [Header("Text")]
        public Texture2D textDefault;
        public Texture2D textDescription;
        public Texture2D textTitle;

        [Header("Mobile")]
        public Texture2D phone;

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

        public static Texture2D GetMediaIcon(MediaInterface type)
        {
            return type switch
            {
                MediaInterface.Play => Play,
                MediaInterface.Pause => Pause,
                MediaInterface.Stop => Stop,
                MediaInterface.Next => Next,
                MediaInterface.Previous => Previous,
                MediaInterface.FastForward => FastForward,
                MediaInterface.Rewind => Rewind,
                MediaInterface.Shuffle => Shuffle,
                _ => Play,
            };
        }
    

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
        public static Texture2D Close => Instance.close;
        public static Texture2D Phone => Instance.phone;
        public static Texture2D TextDefault => Instance.textDefault;
        public static Texture2D TextDescription => Instance.textDescription;
        public static Texture2D TextTitle => Instance.textTitle;
        public static Texture2D Enter => Instance.enter;
    }
}
