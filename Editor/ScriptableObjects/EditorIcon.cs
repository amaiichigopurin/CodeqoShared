using Codeqo.NativeMediaPlayer.UI;
using UnityEditor;
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


        // Unity Built-In Icons
        public static Texture2D Prefab => EditorGUIUtility.FindTexture("Prefab Icon");
        public static Texture2D Folder => EditorGUIUtility.FindTexture("Folder Icon");
        public static Texture2D Favorite => EditorGUIUtility.FindTexture("Favorite Icon");
        public static Texture2D Camera => EditorGUIUtility.FindTexture("Camera Icon");
        public static Texture2D Audio => EditorGUIUtility.FindTexture("AudioClip Icon");
        public static Texture2D RectTransform => EditorGUIUtility.FindTexture("RectTransform Icon");
        public static Texture2D Help => EditorGUIUtility.FindTexture("d_UnityEditor.InspectorWindow");
        public static Texture2D Menu => EditorGUIUtility.FindTexture("d_MenuBar");
        public static Texture2D Settings => EditorGUIUtility.FindTexture("d_Settings");
        public static Texture2D Console => EditorGUIUtility.FindTexture("d_UnityEditor.ConsoleWindow");
        public static Texture2D Scene => EditorGUIUtility.FindTexture("d_SceneView");
        public static Texture2D Game => EditorGUIUtility.FindTexture("d_GameView");
        public static Texture2D Inspector => EditorGUIUtility.FindTexture("d_UnityEditor.InspectorWindow");
        public static Texture2D Project => EditorGUIUtility.FindTexture("d_Project");
        public static Texture2D Hierarchy => EditorGUIUtility.FindTexture("d_HierarchyWindow");
        public static Texture2D Animation => EditorGUIUtility.FindTexture("d_AnimationWindow");
        public static Texture2D Animator => EditorGUIUtility.FindTexture("d_AnimatorController Icon");
        public static Texture2D Label => EditorGUIUtility.FindTexture("d_Label Icon");
        public static Texture2D Tag => EditorGUIUtility.FindTexture("d_FilterByLabel");
        public static Texture2D Layer => EditorGUIUtility.FindTexture("d_FilterByLayer");
        public static Texture2D Text => EditorGUIUtility.FindTexture("TextAsset Icon");
        public static Texture2D Android => EditorGUIUtility.FindTexture("BuildSettings.Android.Small");
        public static Texture2D iOS => EditorGUIUtility.FindTexture("BuildSettings.iPhone.Small");
        public static Texture2D Windows => EditorGUIUtility.FindTexture("BuildSettings.Standalone.Small");
        public static Texture2D Web => EditorGUIUtility.FindTexture("BuildSettings.Web.Small");
        public static Texture2D Xbox => EditorGUIUtility.FindTexture("BuildSettings.Xbox360.Small");
        public static Texture2D PS4 => EditorGUIUtility.FindTexture("BuildSettings.PS4.Small");
        public static Texture2D Switch => EditorGUIUtility.FindTexture("BuildSettings.Switch.Small");
        public static Texture2D WebGL => EditorGUIUtility.FindTexture("BuildSettings.WebGL.Small");


        public static Texture2D Status => EditorGUIUtility.FindTexture("d_UnityEditor.ConsoleWindow");
        public static Texture2D Confirmed => EditorGUIUtility.FindTexture("d_console.infoicon.sml");
        public static Texture2D Error => EditorGUIUtility.FindTexture("d_ConsoleErrorIcon");
        public static Texture2D Warning => EditorGUIUtility.FindTexture("d_ConsoleWarnIcon");
        public static Texture2D Loading => EditorGUIUtility.FindTexture("d_WaitSpin00");
    }
}
