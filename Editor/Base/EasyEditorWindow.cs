using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    public abstract class EasyEditorWindow<WindowClass> : EditorWindow
        where WindowClass : EditorWindow
    {
        const float DEFAULT_IMAGE_HEIGHT = 74f;
        const float DEFAULT_IMAGE_WIDTH = 74f;
        const float DEFAULT_ICON_HEIGHT = 32f;
        const float DEFAULT_ICON_WIDTH = 32f;
        const float DEFAULT_MIN_WINDOW_HEIGHT = 400f;
        const float DEFAULT_MIN_WINDOW_WIDTH = 720f;
        const float DEFAULT_MAX_WINDOW_HEIGHT = 1200f;
        const float DEFAULT_MAX_WINDOW_WIDTH = 1800f;
        const float DEFAULT_BUTTON_HEIGHT = 30f;
        const float DEFAULT_BUTTON_WIDTH = 120f;
        const float DEFAULT_MINI_BUTTON_HEIGHT = 20f;
        const float DEFAULT_MINI_BUTTON_WIDTH = 80f;

        protected string WindowName
        {
            get => EditorPrefs.GetString(typeof(WindowClass).Name + "_windowName");
            set => EditorPrefs.SetString(typeof(WindowClass).Name + "_windowName", value);
        }
        protected bool DebugMode
        {
            get => EditorPrefs.GetBool(typeof(WindowClass).Name + "_debugMode", false);
            set => EditorPrefs.SetBool(typeof(WindowClass).Name + "_debugMode", value);
        }

        protected static List<string> _debugInfo = new List<string>();
        protected Vector2 ImageSize
        {
            get
            {
                float x = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_imageWidth", DEFAULT_IMAGE_WIDTH);
                float y = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_imageHeight", DEFAULT_IMAGE_HEIGHT);
                return new Vector2(x, y);
            }
            set
            {
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_imageWidth", value.x);
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_imageHeight", value.y);
            }            
        }
        protected Vector2 IconSize
        {
            get
            {
                float x = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_iconWidth", DEFAULT_ICON_WIDTH);
                float y = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_iconHeight", DEFAULT_ICON_HEIGHT);
                return new Vector2(x, y);
            }
            set
            {
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_iconWidth", value.x);
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_iconHeight", value.y);
            }
        }
        public static Vector2 MinWindowSize
        {
            get
            {
                float x = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_minWindowWidth", DEFAULT_MIN_WINDOW_WIDTH);
                float y = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_minWindowHeight", DEFAULT_MIN_WINDOW_HEIGHT);
                return new Vector2(x, y);
            }
            set
            {
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_minWindowWidth", value.x);
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_minWindowHeight", value.y);
            }
        }
        public static Vector2 MaxWindowSize
        {
            get
            {
                float x = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_maxWindowWidth", DEFAULT_MAX_WINDOW_WIDTH);
                float y = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_maxWindowHeight", DEFAULT_MAX_WINDOW_HEIGHT);
                return new Vector2(x, y);
            }
            set
            {
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_maxWindowWidth", value.x);
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_maxWindowHeight", value.y);
            }
        }

        protected Vector2 ButtonSize
        {
            get
            {
                float x = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_buttonWidth", DEFAULT_BUTTON_WIDTH);
                float y = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_buttonHeight", DEFAULT_BUTTON_HEIGHT);
                return new Vector2(x, y);
            }
            set
            {
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_buttonWidth", value.x);
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_buttonHeight", value.y);
            }
        }

        protected Vector2 MiniButtonSize
        {
            get
            {
                float x = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_miniButtonWidth", DEFAULT_MINI_BUTTON_WIDTH);
                float y = EditorPrefs.GetFloat(typeof(WindowClass).Name + "_miniButtonHeight", DEFAULT_MINI_BUTTON_HEIGHT);
                return new Vector2(x, y);
            }
            set
            {
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_miniButtonWidth", value.x);
                EditorPrefs.SetFloat(typeof(WindowClass).Name + "_miniButtonHeight", value.y);
            }
        }

        protected Vector2 minWindowSize;
        protected Vector2 maxWindowSize;
        
        protected Vector2 imageSize;
        protected Vector2 iconSize;
        
        protected Vector2 buttonSize;
        protected Vector2 miniButtonSize;

        protected string windowName;
        protected GUILayoutOption[] buttonOptions;
        protected GUILayoutOption[] innerButtonOptions;
        protected bool _isShowingSettings;

        protected static void Initialize()
        {
            Debug.Log("Opening " + typeof(WindowClass).Name);
            WindowClass window = (WindowClass)GetWindow(typeof(WindowClass), false, typeof(WindowClass).Name);
            window.Show();
            window.minSize = MinWindowSize;
            window.maxSize = MaxWindowSize;
            window.autoRepaintOnSceneChange = true;
        }

        protected virtual void OnEnable()
        {
            windowName = WindowName;
            minWindowSize = MinWindowSize;
            maxWindowSize = MaxWindowSize;

            imageSize = ImageSize;
            iconSize = IconSize;
            buttonSize = ButtonSize;
            miniButtonSize = MiniButtonSize;     
       
            buttonOptions = new GUILayoutOption[] { GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y), GUILayout.ExpandWidth(true) };
            innerButtonOptions = new GUILayoutOption[] { GUILayout.Width(miniButtonSize.x), GUILayout.Height(miniButtonSize.y), GUILayout.ExpandWidth(true) };
        }

        protected void InternalMenuHeader(string title)
        {
            /* centered button with width 100 */
            CUILayout.HorizontalLayout(() =>
            {
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Close"))
                {
                    _isShowingSettings = false;
                }
            });
            GUILayout.Space(5);
        }

        protected void OpenSettings()
        {
            CUILayout.VerticalLayout(CUI.Box(10, 10, 10, 10, CUIColor.None), (System.Action)(() =>
            {
                float currentWindowSizeX = position.width;
                float currentWindowSizeY = position.height;
                InternalMenuHeader("Window Settings");

                CUILayout.VerticalLayout(CUI.box, () =>
                {
                    EditorGUILayout.LabelField("Current Window Size : " + currentWindowSizeX + " x " + currentWindowSizeY);
                });

                GUILayout.Space(5);

                EditorGUILayout.LabelField("UI Window", EditorStyles.boldLabel);
                windowName = EditorGUILayout.TextField("Window Name", windowName);
                minWindowSize = EditorGUILayout.Vector2Field("Minimum Window Size", minWindowSize);
                maxWindowSize = EditorGUILayout.Vector2Field("Maximum Window Size", maxWindowSize);
                GUILayout.Space(5);

                EditorGUILayout.LabelField("UI Buttons", EditorStyles.boldLabel);
                buttonSize = EditorGUILayout.Vector2Field("Button Size", buttonSize);
                miniButtonSize = EditorGUILayout.Vector2Field("Inner Button Size", miniButtonSize);
                GUILayout.Space(5);

                EditorGUILayout.LabelField("UI Images (Textures/Icons)", EditorStyles.boldLabel);
                imageSize = EditorGUILayout.Vector2Field("Image Size", imageSize);
                iconSize = EditorGUILayout.Vector2Field("Icon Size", iconSize);
                GUILayout.Space(5);

                CUILayout.HorizontalLayout(() =>
                {
                    if (GUILayout.Button("Save Settings", innerButtonOptions))
                    {                        
                        WindowName = windowName;
                        MinWindowSize = minWindowSize;
                        MaxWindowSize = maxWindowSize;
                        ButtonSize = buttonSize;
                        MiniButtonSize = miniButtonSize;
                        ImageSize = imageSize;
                        IconSize = iconSize;
                        Initialize();
                    }
                });
            }));
        }
    }
}