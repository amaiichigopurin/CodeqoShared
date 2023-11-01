using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CodeqoEditor.Git
{
    public abstract class GitWindow : PaddedEditorWindow
    {
        protected abstract string GIT_URL { get; }
        protected abstract string WORKING_DIR { get; }

        CodeqoGit _git;
        Vector2 _scrollPosition;
        List<GitOutput> _gitOutputs;

        string _repoName;
        string _windowName;
        string _commandLine;

        int _gitOutputUpdated = 0;
        bool _initialized = false;

        private Dictionary<GitOutputStatus, Color> gitOutputColors = new Dictionary<GitOutputStatus, Color>
        {
            { GitOutputStatus.Error, Color.red },
            { GitOutputStatus.Success, Color.blue },
            { GitOutputStatus.Warning, Color.magenta }
        };
        
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

            _gitOutputs = new List<GitOutput>();
            _repoName = GIT_URL.Substring(GIT_URL.LastIndexOf('/') + 1);
            _windowName = _repoName;

            _git = new CodeqoGit(WORKING_DIR, GIT_URL);

            _git.OnGitOutput += (output) =>
            {
                _gitOutputs.Add(output);
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


            if (_git.PullAvailable)
            {
                EditorGUILayout.HelpBox("New version available. Please download the latest version.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox("You are up to date with the latest version.", MessageType.Info);
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
            GUIStyle gitOutputStyle = new GUIStyle(GUI.skin.label)
            {
                wordWrap = true
            };

            CUILayout.VerticalLayout(CUI.Box(5), () =>
            {
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

                foreach (var gitOutput in _gitOutputs)
                {
<<<<<<< HEAD
                    DrawGitOutput(gitOutput, gitOutputStyle);
=======
                    for (int i = 0; i < _gitOutputs.Count; i++)
                    {              
                        Color colorOrigin = Color.black;
                        
                        switch (_gitOutputs[i].status)
                        {
                            case GitOutputStatus.Error:                      
                                colorOrigin = gitOutputStyle.normal.textColor;
                                gitOutputStyle.normal.textColor = Color.red;
                                GUILayout.Label(_gitOutputs[i].value, gitOutputStyle);
                                gitOutputStyle.normal.textColor = colorOrigin;
                                break;
                            case GitOutputStatus.Success:                       
                                colorOrigin = gitOutputStyle.normal.textColor;
                                gitOutputStyle.normal.textColor = Color.blue;
                                GUILayout.Label(_gitOutputs[i].value, gitOutputStyle);
                                gitOutputStyle.normal.textColor = colorOrigin;
                                break;
                            case GitOutputStatus.Warning:                     
                                colorOrigin = gitOutputStyle.normal.textColor;
                                gitOutputStyle.normal.textColor = Color.magenta;
                                GUILayout.Label(_gitOutputs[i].value, gitOutputStyle);
                                gitOutputStyle.normal.textColor = colorOrigin;
                                break;
                            default:
                                GUILayout.Label(_gitOutputs[i].value, gitOutputStyle);
                                break;
                        }     
                    }                    
>>>>>>> ae0b2997d5568880480ce3f1fdf16e9647e27880
                }

                GUILayout.EndScrollView();

                GUILayout.FlexibleSpace();
                DrawCommandLineInput();

            }, GUILayout.MinHeight(120), GUILayout.MaxHeight(2000), GUILayout.ExpandHeight(true));
        }

        void DrawGitOutput(GitOutput gitOutput, GUIStyle gitOutputStyle)
        {
            if (gitOutputColors.TryGetValue(gitOutput.status, out Color color))
            {
                GUIStyle coloredStyle = new GUIStyle(gitOutputStyle)
                {
                    normal = { textColor = color }
                };
                GUILayout.Label(gitOutput.value, coloredStyle);
            }
            else
            {
                GUILayout.Label(gitOutput.value, gitOutputStyle);
            }
        }

        void DrawCommandLineInput()
        {
            GUILayout.BeginHorizontal();
            {
                _commandLine = GUILayout.TextField(_commandLine);

                if (GUILayout.Button(EditorIcon.Enter, GUILayout.Height(18f), GUILayout.Width(30f)))
                {
                    EnterGitCommand();
                }
            }
            GUILayout.EndHorizontal();
        }

        void DrawButtons()
        {
            if (GUILayout.Button("Download (Git Pull)"))
            {
                Pull();
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

                    VersionTypePopup.ShowWindow(popupMessage, popupDescription, GitVersion.Patch, (versionType) =>
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
            _commandLine = _commandLine.Trim();
            if (string.IsNullOrEmpty(_commandLine))
            {
                _gitOutputs.Add(new GitOutput("Empty Command"));
                return;
            }
            await _git.RunGitCommandAsync(_commandLine);
            _commandLine = "";
        }

        async void Pull()
        {
            await _git.PullAsync();
        }

        async void Push(GitVersion versionType)
        {
            await _git.PushAsync(versionType);
        }

        async void Status()
        {
            await _git.StatusAsync();
        }
    }
}
