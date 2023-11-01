using UnityEditor;
using UnityEngine;
using static CodeqoEditor.VersionInfo;

namespace CodeqoEditor
{
    public abstract class GitWindow : PaddedEditorWindow
    {
        protected abstract string GIT_URL { get; }
        protected abstract string WORKING_DIR { get; }

        CodeqoGit _git;
        string _gitOutput;
        string _gitErrorOutput;
        string _repoName;
        string _windowName;
        string _commandLine;

        int _gitOutputUpdated = 0;
        bool _initialized = false;


        async void OnEnable()
        {
            if (string.IsNullOrEmpty(GIT_URL))
            {
                Debug.LogError("Git URL is null or empty: " + GIT_URL);
                return;
            }

            if (string.IsNullOrEmpty(WORKING_DIR))
            {
                Debug.LogError("Working Dir is null or empty: " + WORKING_DIR);
                return;
            }
          
            _repoName = GIT_URL.Substring(GIT_URL.LastIndexOf('/') + 1);
            _windowName = _repoName + " Git Window";

            _git = new CodeqoGit(WORKING_DIR, GIT_URL);
            _git.OnGitOutput += (output) =>
            {
                _gitOutput = output;
                _gitOutputUpdated++;
            };
            _git.OnGitErrorOutput += (output) =>
            {
                _gitErrorOutput = output;
                _gitOutputUpdated++;
            };

            await _git.InitializeAsync();
            _initialized = true;
        }

        protected override void OnGUIUpdate()
        {
            DrawLabel();

            if (!_initialized)
            {
                GUILayout.Label("Initializing Git...");
                return;
            }

            DrawGitPanel();
            DrawButtons();

            GUILayout.Space(10);

            CUILayout.VerticalLayout(CUI.box, () =>
            {
                GUILayout.Label("Version Info", EditorStyles.boldLabel);
                GUILayout.Label($"Local Version: {_git.LocalVersion}");
                GUILayout.Label($"Remote Version: {_git.RemoteVersion}");
            });
        }

        void DrawLabel()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(_windowName, EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.Label($"Output {(_gitOutputUpdated)}");
            }
            GUILayout.EndHorizontal();
        }

        void DrawGitPanel()
        {
            GUIStyle gitOutputStyle = GUI.skin.label;
            gitOutputStyle.wordWrap = true;

            CUILayout.VerticalLayout(CUI.Box(5), () =>
            {
                if (!string.IsNullOrEmpty(_gitOutput))
                {
                    GUILayout.Label(_gitOutput, gitOutputStyle);
                }

                if (!string.IsNullOrEmpty(_gitErrorOutput))
                {
                    CUILayout.ColorLabelField(_gitErrorOutput, Color.red, gitOutputStyle);
                }

                GUILayout.FlexibleSpace();

                DrawCommandLineInput();

            }, GUILayout.MinHeight(120), GUILayout.MaxHeight(2000), GUILayout.ExpandHeight(true));
        }

        void DrawCommandLineInput()
        {
            GUILayout.BeginHorizontal();
            {
                _commandLine = GUILayout.TextField(_commandLine);

                if (GUILayout.Button("Enter", GUILayout.Width(30f)))
                {
                    EnterGitCommand();
                }
            }
            GUILayout.EndHorizontal();
        }

        void DrawButtons()
        {
            if (_git.PullAvailable)
            {
                EditorGUILayout.HelpBox("New version available. Please download the latest version.", MessageType.Warning);

                if (GUILayout.Button("Download (Git Pull)"))
                {
                    Pull();
                }
            }

            if (GUILayout.Button("Upload (Git Push)"))
            {
                if (CUIUtility.Warning("Are you sure you want to upload to the git repository?"))
                {
                    string popupMessage = "Please select the version type.";
                    string popupDescription = "Version type is used to determine the version number. \n" +
                                              "Patch: 1.0.0 -> 1.0.1 \n" +
                                              "Minor: 1.0.0 -> 1.1.0 \n" +
                                              "Major: 1.0.0 -> 2.0.0 \n";

                    VersionTypePopup.ShowWindow(popupMessage, popupDescription, VersionType.Patch, (versionType) =>
                    {
                        Push(versionType);
                    });
                }
            }

            if (GUILayout.Button("Update Git Status"))
            {
                Status();
            }
        }

        async void EnterGitCommand()
        {
            await _git.RunGitCommandAsync(_commandLine);
            _commandLine = "";
        }

        async void Pull()
        {
            await _git.PullAsync();
        }

        async void Push(VersionType versionType)
        {
            await _git.PushAsync(versionType);
        }

        async void Status()
        {
            await _git.StatusAsync();
        }
    }
}
