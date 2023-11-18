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
        bool _debugMode = false;

        private Dictionary<GitOutputStatus, Color> gitOutputColors = new Dictionary<GitOutputStatus, Color>
        {
            { GitOutputStatus.Success, Color.blue },
            { GitOutputStatus.Warning, ExColor.firebrick },
            { GitOutputStatus.Hint, Color.magenta },
            { GitOutputStatus.Error, ExColor.charcoal },
            { GitOutputStatus.RealError, ExColor.orange },
            { GitOutputStatus.Fatal, Color.red },
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

            _git = new CodeqoGit(WORKING_DIR, GIT_URL, _repoName);

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

            DrawVersionInfo();

            //if (_git.PullAvailable)
            //{
            //    EditorGUILayout.HelpBox("New version available. Please download the latest version.", MessageType.Warning);
            //}
            //else
            //{
            //    EditorGUILayout.HelpBox("You are up to date with the latest version.", MessageType.Info);
            //}            

            DrawGitPanel();
            DrawButtons();

            GUILayout.Space(10);

            DrawDebugMenu();
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

        void DrawVersionInfo()
        {
            CUILayout.VerticalLayout(CUI.box, () =>
            {
                GUILayout.Label($"Local: {_git.LocalVersion.CreateTagInfo()}");
                GUILayout.Label($"Remote: {_git.RemoteVersion.CreateTagInfo()}");
            });
        }

        void DrawDebugMenu()
        {
            CUILayout.VerticalLayout(CUI.box, () =>
            {
                _debugMode = GUILayout.Toggle(_debugMode, "Debug Menu");

                if (_debugMode)
                {
                    if (GUILayout.Button("Commit"))
                    {
                        Commit();
                    }

                    if (GUILayout.Button("Normalize Line Endings"))
                    {
                        if (CUIDialogue.Warning("Are you sure you want to continue?"))
                        {
                            NomalizeLineEndings();
                        }
                    }

                    if (GUILayout.Button("Configure core.autocrlf Globally [true]"))
                    {
                        if (CUIDialogue.Warning("Are you sure you want to continue?"))
                        {
                            ConfigureAutoCRLF(true);
                        }
                    }

                    if (GUILayout.Button("Configure core.autocrlf Globally [false]"))
                    {
                        if (CUIDialogue.Warning("Are you sure you want to continue?"))
                        {
                            ConfigureAutoCRLF(false);
                        }
                    }

                    if (GUILayout.Button("Force Push"))
                    {
                        if (CUIDialogue.Warning("Are you sure you want to upload to the git repository?"))
                        {
                            string popupMessage = "Are you sure you want to force push?";
                            string popupDescription = "Version type is used to determine the version number. \n" +
                                                      "Patch: 1.0.0 -> 1.0.1 \n" +
                                                      "Minor: 1.0.0 -> 1.1.0 \n" +
                                                      "Major: 1.0.0 -> 2.0.0 \n";

                            VersionTypePopup.ShowWindow((popupMessage, popupDescription), VersionIncrement.Patch, ForcePush);
                        }
                    }

                    if (GUILayout.Button("Push Version Tag"))
                    {
                        PushVersionTag();
                    }

                    if (GUILayout.Button("Pull Version Tag"))
                    {
                        PullVersionTag();
                    }
                }
            });
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
                    DrawGitOutput(gitOutput, gitOutputStyle);
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
                if (CUIDialogue.Warning("Are you sure you want to upload to the git repository?"))
                {
                    string popupMessage = "Please select the version type.";
                    string popupDescription = "Version type is used to determine the version number. \n" +
                                              "Patch: 1.0.0 -> 1.0.1 \n" +
                                              "Minor: 1.0.0 -> 1.1.0 \n" +
                                              "Major: 1.0.0 -> 2.0.0 \n";

                    VersionTypePopup.ShowWindow((popupMessage, popupDescription), VersionIncrement.Patch, Push);
                }
            }

            if (GUILayout.Button("Git Status"))
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

        async void Push(VersionIncrement versionType)
        {
            await _git.PushAsync(versionType);
        }

        async void ForcePush(VersionIncrement versionType)
        {
            await _git.PushAsync(versionType, true);
        }

        async void Status()
        {
            await _git.StatusAsync();
        }

        async void PushVersionTag()
        {
            await _git.PushVersionTagAsync();
        }

        async void PullVersionTag()
        {
            await _git.PullVersionTagAsync();
        }
        async void Commit()
        {
            await _git.CommitAsync();
        }

        async void NomalizeLineEndings()
        {
            await _git.NormalizeLineEndingsAsync();
        }

        async void ConfigureAutoCRLF(bool value)
        {
            await _git.ConfigureGlobalCoreAutoCRLFAsync(value);
        }
    }
}
