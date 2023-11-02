using CodeqoEditor;
using UnityEditor;
using UnityEngine;

public abstract class ExtendedEditorWindow<WindowClass> : CodeqoEditorWindow<WindowClass>
    where WindowClass : EditorWindow
{  
    protected Vector2 scrollPosition;

    #region Lazy Caching
    GUIStyle _topBorderStyle;
    GUIStyle TopBorderStyle
    {
        get
        {
            if (_topBorderStyle == null) _topBorderStyle = GetTopBorderStyle(); 
            return _topBorderStyle;
        }
    }
    protected virtual GUIStyle GetTopBorderStyle() => CUI.Border(BorderDirection.Top);

    GUIStyle _bottomBorderStyle;
    GUIStyle BottomBorderStyle
    {
        get
        {
            if (_bottomBorderStyle == null) _bottomBorderStyle = GetBottomBorderStyle();
            return _bottomBorderStyle;
        }
    }
    protected virtual GUIStyle GetBottomBorderStyle() => CUI.Border(BorderDirection.Bottom);
    #endregion
    
    protected virtual void OnGUI()
    {
        DrawTop();

        GUILayout.BeginVertical(TopBorderStyle);
        DrawToolBarRow();
        GUILayout.EndVertical();
        
        GUILayout.BeginVertical();
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            GUILayout.Space(5);

            CUILayout.HorizontalLayout(() => {
                OnGUIUpdate();
                GUILayout.FlexibleSpace();
            });

            GUILayout.Space(5);
            EditorGUILayout.EndScrollView();
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical(BottomBorderStyle);
        DrawBottom();
        GUILayout.EndVertical();
    }    

    protected virtual void DrawToolBarRow()
    {
        CUILayout.HorizontalLayout(() =>
        {
            DrawWindowLabel();
            GUILayout.FlexibleSpace();
            DrawExtraToolBarButtons();
            if (GUILayout.Button(EditorContent.Settings))
            {
                _isShowingSettings = !_isShowingSettings;
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

    #region Override Methods
    protected abstract void ResetWindow();
    protected virtual void DrawTop() { }
    protected abstract void DrawWindowLabel();
    protected virtual void DrawToolBar() { }
    protected virtual void DrawExtraToolBarRow() { }
    protected virtual void DrawExtraToolBarButtons() { }
    protected abstract void OnGUIUpdate();
    protected abstract void DrawBottom();
    #endregion
}
