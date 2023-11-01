using CodeqoEditor;
using UnityEditor;
using UnityEngine;

public abstract class ExtendedEditorWindow<WindowClass> : CodeqoEditorWindow<WindowClass>
    where WindowClass : EditorWindow
{  

    protected Vector2 scrollPosition;   

    protected abstract void OnGUIUpdate();
    protected abstract void DrawWindowLabel();
    protected abstract void DrawToolBar();
    protected abstract void ResetWindow();
    protected virtual void DrawExtraToolBarRow() { }
    protected virtual void DrawExtraToolBarButtons() { }
    protected virtual void DrawToolBarRow()
    {
        CUILayout.HorizontalLayout(() =>
        {
            DrawWindowLabel();
            GUILayout.FlexibleSpace();
            DrawExtraToolBarButtons();
            if (GUILayout.Button("Settings"))
            {
                _isShowingSettings = !_isShowingSettings;
                return;
            }
            if (GUILayout.Button("Reset"))
            {
                Initialize();
                ResetWindow();
                AssetDatabase.Refresh();
                return;
            }
        });
        if (_isShowingSettings)
        {
            OpenSettings();
        }
        else
        {
            DrawExtraToolBarRow();
            DrawToolBar();
        }  
    }
    
    protected virtual void OnGUI()
    {
        DrawTop();

        CUILayout.VerticalLayout(CUI.Border(BorderDirection.Top), () => {
            DrawToolBarRow();
        });

        CUILayout.VerticalLayout(CUI.BG(), () => {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            GUILayout.Space(5);

            CUILayout.HorizontalLayout(() => {
                OnGUIUpdate();
                GUILayout.FlexibleSpace();
            });

            GUILayout.Space(5);
            EditorGUILayout.EndScrollView();
        });

        CUILayout.VerticalLayout(CUI.Border(BorderDirection.Bottom), () => {
            DrawBottom();
        });
    }
    
    protected virtual void DrawTop() { }
    protected abstract void DrawBottom();  
}
