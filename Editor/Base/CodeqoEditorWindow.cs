using CodeqoEditor;
using UnityEditor;
using UnityEngine;

public abstract class CodeqoEditorWindow<WindowClass> : EditorWindow
    where WindowClass : EditorWindow
{
    protected string WindowName 
    { 
        get => EditorPrefs.GetString(typeof(WindowClass).Name + "_name"); 
        set => EditorPrefs.SetString(typeof(WindowClass).Name + "_name", value); 
    }
    protected float ImageHeight 
    { 
        get => EditorPrefs.GetFloat(typeof(WindowClass).Name + "_imageHeight", 74f); 
        set => EditorPrefs.SetFloat(typeof(WindowClass).Name + "_imageHeight", value); 
    }
    protected float ImageWidth 
    { 
        get => EditorPrefs.GetFloat(typeof(WindowClass).Name + "_imageWidth", 74f); 
        set => EditorPrefs.SetFloat(typeof(WindowClass).Name + "_imageWidth", value); 
    }
    protected float SmallImageHeight 
    { 
        get => EditorPrefs.GetFloat(typeof(WindowClass).Name + "_smallImageHeight", 32f); 
        set => EditorPrefs.SetFloat(typeof(WindowClass).Name + "_smallImageHeight", value); 
    }
    protected float SmallImageWidth 
    { 
        get => EditorPrefs.GetFloat(typeof(WindowClass).Name + "_smallImageWidth", 32f); 
        set => EditorPrefs.SetFloat(typeof(WindowClass).Name + "_smallImageWidth", value); 
    }    
    protected bool DebugMode
    {
        get => EditorPrefs.GetBool(typeof(WindowClass).Name + "_debugMode", false);
        set => EditorPrefs.SetBool(typeof(WindowClass).Name + "_debugMode", value);
    }

    protected static Vector2 GetWindowMinSize()
    {
        int width = EditorPrefs.GetInt($"{typeof(WindowClass).Name}WindowWidth", 720);
        int height = EditorPrefs.GetInt($"{typeof(WindowClass).Name}WindowHeight", 400);
        return new Vector2(width, height);
    }

    protected static void SetWindowMinSize(Vector2 minSize)
    {
        EditorPrefs.SetInt($"{typeof(WindowClass).Name}WindowWidth", (int)minSize.x);
        EditorPrefs.SetInt($"{typeof(WindowClass).Name}WindowHeight", (int)minSize.y);
    }

    protected static Vector2 GetWindowMaxSize()
    {
        int width = EditorPrefs.GetInt($"{typeof(WindowClass).Name}WindowMaxWidth", 1200);
        int height = EditorPrefs.GetInt($"{typeof(WindowClass).Name}WindowMaxHeight", 1200);
        return new Vector2(width, height);
    }

    protected static void SetWindowMaxSize(Vector2 maxSize)
    {
        EditorPrefs.SetInt($"{typeof(WindowClass).Name}WindowMaxWidth", (int)maxSize.x);
        EditorPrefs.SetInt($"{typeof(WindowClass).Name}WindowMaxHeight", (int)maxSize.y);
    }

    protected static void SetButtonSize(Vector2 buttonSize)
    {
        EditorPrefs.SetInt($"{typeof(WindowClass).Name}ButtonSizeWidth", (int)buttonSize.x);
        EditorPrefs.SetInt($"{typeof(WindowClass).Name}ButtonSizeHeight", (int)buttonSize.y);
    }

    protected static Vector2 GetButtonSize()
    {
        int width = EditorPrefs.GetInt($"{typeof(WindowClass).Name}ButtonSizeWidth", 120);
        int height = EditorPrefs.GetInt($"{typeof(WindowClass).Name}ButtonSizeHeight", 30);
        return new Vector2(width, height);
    }

    protected static void SetInnerButtonSize(Vector2 buttonSize)
    {
        EditorPrefs.SetInt($"{typeof(WindowClass).Name}InnerButtonSizeWidth", (int)buttonSize.x);
        EditorPrefs.SetInt($"{typeof(WindowClass).Name}InnerButtonSizeHeight", (int)buttonSize.y);
    }

    protected static Vector2 GetInnerButtonSize()
    {
        int width = EditorPrefs.GetInt($"{typeof(WindowClass).Name}InnerButtonSizeWidth", 80);
        int height = EditorPrefs.GetInt($"{typeof(WindowClass).Name}InnerButtonSizeHeight", 20);
        return new Vector2(width, height);
    }

    protected Vector2 windowMinSize;
    protected Vector2 windowMaxSize;
    protected Vector2 buttonSize;
    protected Vector2 innerButtonSize;
    protected float imageHeight, imageWidth;
    protected float smallImageHeight, smallImageWidth;
    protected string windowName;
    protected GUILayoutOption[] buttonOptions;
    protected GUILayoutOption[] innerButtonOptions;
    protected bool _isShowingSettings;
    
    protected static void Initialize()
    {
        Debug.Log("Opening " + typeof(WindowClass).Name);
        WindowClass window = (WindowClass)GetWindow(typeof(WindowClass), false, typeof(WindowClass).Name);
        window.Show();
        window.minSize = GetWindowMinSize();
        window.maxSize = GetWindowMaxSize();
        window.autoRepaintOnSceneChange = true;
    }

    protected virtual void OnEnable()
    {
        windowMinSize = GetWindowMinSize();
        windowMaxSize = GetWindowMaxSize();
        buttonSize = GetButtonSize();
        innerButtonSize = GetInnerButtonSize();
        imageHeight = ImageHeight;
        imageWidth = ImageWidth;
        smallImageHeight = SmallImageHeight;
        smallImageWidth = SmallImageWidth;
        windowName = WindowName;
        buttonOptions = new GUILayoutOption[] { GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y), GUILayout.ExpandWidth(true) };
        innerButtonOptions = new GUILayoutOption[] { GUILayout.Width(innerButtonSize.x), GUILayout.Height(innerButtonSize.y), GUILayout.ExpandWidth(true) };
    }
    
    protected void InternalMenuHeader(string title)
    {
        /* centered button with width 100 */
        CUILayout.HorizontalLayout(() => {
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

            CUILayout.VerticalLayout(CUI.box, () => {
                EditorGUILayout.LabelField("Current Window Size : " + currentWindowSizeX + " x " + currentWindowSizeY);
            });

            GUILayout.Space(5);

            EditorGUILayout.LabelField("UI Window", EditorStyles.boldLabel);
            windowName = EditorGUILayout.TextField("Window Name", windowName);
            windowMinSize = EditorGUILayout.Vector2Field("Minimum Window Size", windowMinSize);
            windowMaxSize = EditorGUILayout.Vector2Field("Maximum Window Size", windowMaxSize);
            GUILayout.Space(5);

            EditorGUILayout.LabelField("UI Buttons", EditorStyles.boldLabel);
            buttonSize = EditorGUILayout.Vector2Field("Button Size", buttonSize);
            innerButtonSize = EditorGUILayout.Vector2Field("Inner Button Size", innerButtonSize);
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Big UI Images (Icons / etc)", EditorStyles.boldLabel);
            imageHeight = EditorGUILayout.FloatField("Image Height", imageHeight);
            imageWidth = EditorGUILayout.FloatField("Image Width", imageWidth);
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Small UI Images (Icons / etc)", EditorStyles.boldLabel);
            smallImageHeight = EditorGUILayout.FloatField("Image Height", smallImageHeight);
            smallImageWidth = EditorGUILayout.FloatField("Image Width", smallImageWidth);
            GUILayout.Space(5);

            CUILayout.HorizontalLayout((System.Action)(() => {
                if (GUILayout.Button("Save Settings", innerButtonOptions))
                {
                    SetWindowMinSize(windowMinSize);
                    SetWindowMaxSize(windowMaxSize);
                    SetButtonSize(buttonSize);
                    SetInnerButtonSize(innerButtonSize);
                    ImageHeight = imageHeight;
                    ImageWidth = imageWidth;
                    SmallImageHeight = smallImageHeight;
                    SmallImageWidth = smallImageWidth;
                    WindowName = windowName;
                    Initialize();
                }
            }));
        }));
    }
}
