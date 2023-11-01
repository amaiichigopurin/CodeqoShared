using UnityEngine;
using UnityEditor;
using System;

namespace CodeqoEditor
{
    public class TabbedPanelTab
    {
        /// <summary>
        /// If icon is set, label will be ignored
        /// </summary>
        public Texture2D Icon { get; private set; }
        public string Label { get; private set; } = "Not Set";
        public Action Content { get; private set; }

        public TabbedPanelTab(string label, Action content)
        {
            Label = label;
            Content = content;
        }
        
        public TabbedPanelTab(Texture2D icon, Action content)
        {
            Icon = icon;
            Content = content;
        }
    }

    public class TabbedPanel
    {
        private int _selectedTabIndex = 0;
        private TabbedPanelTab[] _tabs;
        private GUILayoutOption[] _options;

        public TabbedPanel(TabbedPanelTab[] tabs, params GUILayoutOption[] options)
        {
            if (tabs == null || tabs.Length == 0)
                throw new ArgumentException("Tabs cannot be null or empty.");

            _tabs = tabs;
            _options = options;
        }

        public void Draw()
        {
            GUILayout.BeginVertical();
            {
                GUIStyle toolbarStyle = new GUIStyle(EditorStyles.toolbarButton);
                
                _selectedTabIndex = GUILayout.Toolbar(_selectedTabIndex, GetTabHeaders(), toolbarStyle, _options);

                GUILayout.BeginVertical(box);
                {                
                   _tabs[_selectedTabIndex].Content?.Invoke();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }

        GUIStyle box
        {
            get
            {
                GUIStyle box = GUI.skin.box;
                box.margin = new RectOffset(0, 0, 0, 0);
                box.border = new RectOffset(10, 10, 10, 10);
                box.padding = new RectOffset(6, 6, 6, 6);
                return box;
            }
        }

        private GUIContent[] GetTabHeaders()
        {
            GUIContent[] tabHeaders = new GUIContent[_tabs.Length];

            for (int i = 0; i < _tabs.Length; i++)
            {
                if (_tabs[i].Icon != null)
                    tabHeaders[i] = new GUIContent(_tabs[i].Icon);
                else
                    tabHeaders[i] = new GUIContent(_tabs[i].Label);
            }

            return tabHeaders;
        }
    }
}
