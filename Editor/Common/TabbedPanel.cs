using UnityEngine;
using UnityEditor;
using System;

namespace CodeqoEditor
{
    public class TabbedPanelTab
    {
        public GUIContent Content { get; private set; }
        public Action PanelContent { get; private set; }

        public TabbedPanelTab(string label, Action content)
        {
            Content = new GUIContent(label);
            PanelContent = content;
        }
        
        public TabbedPanelTab(Texture2D icon, Action content)
        {
            Content = new GUIContent(icon);
            PanelContent = content;
        }

        public TabbedPanelTab(GUIContent con, Action content)
        {
            Content = con;
            PanelContent = content;
        }
    }

    public class TabbedPanel
    {
        #region Lazy Init
        GUIStyle _tabStyle;
        GUIStyle tabStyle => _tabStyle ?? (_tabStyle = CUI.skin.GetStyle("tabbedpaneltab"));

        GUIStyle _panelStyle;
        GUIStyle panelStyle => _panelStyle ?? (_panelStyle = CUI.skin.GetStyle("tabbedpanelpanel"));

        GUIContent[] _headers;
        GUIContent[] headers
        {
            get
            {
                if (_headers == null)
                {
                    _headers = new GUIContent[_tabs.Length];
                    for (int i = 0; i < _tabs.Length; i++)
                    {
                        _headers[i] = _tabs[i].Content;
                    }
                }
                return _headers;
            }
        }
        #endregion

        bool _isFixedHeight = false;
        float _fixedHeight = 0;
        int _selectedTabIndex = 0;
        TabbedPanelTab[] _tabs;
        GUILayoutOption[] _options;

        public TabbedPanel(TabbedPanelTab[] tabs, params GUILayoutOption[] options)
        {
            if (tabs == null || tabs.Length == 0)
                throw new ArgumentException("Tabs cannot be null or empty.");

            _tabs = tabs;
            _options = options;
        }

        public void SetFixedHeight(float height)
        {
            _isFixedHeight = true;
            _fixedHeight = height;
        }    

        public void Draw()
        {
            GUILayout.BeginVertical();
            { 
                _selectedTabIndex = GUILayout.Toolbar(_selectedTabIndex, headers, tabStyle);

                GUILayout.BeginVertical(panelStyle, _options);
                {
                    if (_isFixedHeight)
                    {
                        GUILayoutOption heightOption = GUILayout.Height(_fixedHeight);
                        GUILayout.BeginVertical(heightOption);
                    }

                    _tabs[_selectedTabIndex].PanelContent?.Invoke();

                    if (_isFixedHeight)
                    {
                        GUILayout.EndVertical();
                    }                           
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }

    }
}
