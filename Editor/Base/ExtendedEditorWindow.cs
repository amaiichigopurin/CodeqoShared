using CodeqoEditor;
using UnityEditor;
using UnityEngine;

namespace Glitch9
{
    public abstract class ExtendedEditorWindow<WindowClass> : EasyEditorWindow<WindowClass>
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

        void OnGUI()
        {
            GUILayout.BeginVertical(TopBorderStyle);
            DrawTop();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                GUILayout.Space(5);
                GUILayout.BeginVertical();
                OnGUIUpdate();
                GUILayout.FlexibleSpace();

                GUILayout.Space(5);
                GUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical(BottomBorderStyle);
            DrawBottom();
            GUILayout.EndVertical();
        }

        void DrawTop()
        {
            CUILayout.HorizontalLayout(() =>
            {
                DrawTopLabel();
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
        protected abstract void DrawTopLabel();
        protected virtual void DrawToolBar() { }
        protected virtual void DrawExtraToolBarRow() { }
        protected virtual void DrawExtraToolBarButtons() { }
        protected abstract void OnGUIUpdate();
        protected abstract void DrawBottom();
        #endregion
    }
}