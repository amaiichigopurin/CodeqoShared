using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace CodeqoEditor.Git
{
    public enum GitOutputStatus
    {
        Info,
        Success,
        Error,
        Warning
    }

    public struct GitOutput
    {
        public string value;
        public GitOutputStatus status;

        public GitOutput(string value, GitOutputStatus status = 0)
        {
            this.value = value;
            this.status = status;
        }
    }

    public class CodeqoGit
    {
        #region Status Checks
        public bool PullAvailable => _remoteVersion > _localVersion && _remoteVersion.Build != 0 && _localVersion.Build != 0;
        public bool PushAvailable => _remoteVersion < _localVersion && _remoteVersion.Build != 0 && _localVersion.Build != 0;
        #endregion

        private const string RAW_GIT_URL = "https://raw.githubusercontent.com";
        private const string GIT_BRANCH = "master";
        private const string GIT_BRANCH_NAME = "main";
        public const string VERSION_FILENAME = "Version.txt";
        string LOCAL_VERSION_FILEPATH => Path.Combine(_workingDirectory, VERSION_FILENAME);
        string REMOTE_VERSION_FILEPATH
        {
            get
            {
                if (string.IsNullOrEmpty(_gitUrl)) return null;
                string[] split = _gitUrl.Split('/');
                string repoName = split[split.Length - 1].Replace(".git", "");
                string repoOwner = split[split.Length - 2];
                return $"{RAW_GIT_URL}/{repoOwner}/{repoName}/{GIT_BRANCH_NAME}/{VERSION_FILENAME}";
            }
        }

        public event Action<GitOutput> OnGitOutput;

        private readonly string _workingDirectory;
        private readonly string _gitUrl; // should look like https://github.com/amaiichigopurin/CodeqoShared.git

        private VersionInfo _localVersion;
        private VersionInfo _remoteVersion;
        public VersionInfo LocalVersion => _localVersion;
        public VersionInfo RemoteVersion => _remoteVersion;

        public CodeqoGit(string workingDirectory, string gitUrl)
        {
            if (string.IsNullOrEmpty(workingDirectory))
            {
                throw new ArgumentException("Working directory cannot be null or empty.", nameof(workingDirectory));
            }

            if (string.IsNullOrEmpty(gitUrl))
            {
                throw new ArgumentException("Git URL cannot be null or empty.", nameof(gitUrl));
            }

            _workingDirectory = workingDirectory;
            _gitUrl = gitUrl;
        }

        public async Task InitializeAsync()
        {
            _localVersion = VersionInfo.CreateFromFilePath(LOCAL_VERSION_FILEPATH);

            try
            {
                await InitializeGitRepositoryAsync();
                await GetVersionInfoAsync();
                await ValidateRemoteOrigin();
                await ValidateBranch();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        async Task ValidateRemoteOrigin()
        {
            string currentRemoteOrigin = await GetCurrentRemoteOrigin();
            currentRemoteOrigin = currentRemoteOrigin.Trim();
            Debug.Log($"Current remote origin: {currentRemoteOrigin}");

            if (currentRemoteOrigin != _gitUrl)
            {
                Debug.LogWarning($"Current remote origin ({currentRemoteOrigin}) does not match the expected remote origin ({_gitUrl}).");
                Debug.Log("Attempting to set origin...");
                await RunGitCommandAsync($"remote add origin {_gitUrl}");
            }
        }

        async Task ValidateBranch()
        {
            string currentBranch = await GetCurrentBranch();
            currentBranch = currentBranch.Trim();
            Debug.Log($"Current branch: {currentBranch}");

            if (currentBranch != GIT_BRANCH)
            {
                Debug.LogWarning($"Current branch ({currentBranch}) does not match the expected branch ({GIT_BRANCH}).");
                Debug.Log("Attempting to checkout branch...");
                await RunGitCommandAsync($"checkout {GIT_BRANCH}");
            }
        }

        public Task PullAsync() => RunGitCommandAsync("pull");

        public async Task PushAsync(GitVersion versionType)
        {
            _remoteVersion.IncrementBuild(LOCAL_VERSION_FILEPATH, versionType);
            await RunGitCommandAsync("add .");
            await RunGitCommandAsync($"commit -m \"Version {_remoteVersion.Build}\"");
            await RunGitCommandAsync("push");
        }

        public Task StatusAsync() => RunGitCommandAsync("status");

        public async Task GetVersionInfoAsync()
        {
            Debug.Log("Getting version info...");
            string versionFile = await DownloadVersionFile();
            Debug.Log($"Version file downloaded: {versionFile}");
            _remoteVersion = new VersionInfo(versionFile);
        }

        public async Task<string> GetCurrentRemoteOrigin()
        {
            return await RunGitCommandAsync("remote get-url origin", returnOutput: true);
        }

        public async Task<string> GetCurrentBranch()
        {
            return await RunGitCommandAsync("rev-parse --abbrev-ref HEAD", returnOutput: true);
        }

        public async Task<string> RunGitCommandAsync(string command, bool returnOutput = false)
        {
            Debug.Log($"Running Git command: {command}");
            OnGitOutput?.Invoke(new GitOutput(command));

            if (string.IsNullOrEmpty(_workingDirectory) || string.IsNullOrEmpty(_gitUrl))
            {
                Debug.LogError("CodeqoGit is not initialized properly.");
                return null;
            }

            var startInfo = new ProcessStartInfo("git", command)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = _workingDirectory
            };

            try
            {
                using var process = new Process { StartInfo = startInfo };
                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();

                process.OutputDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        Debug.Log(args.Data);
                        outputBuilder.AppendLine(args.Data);
                        OnGitOutput?.Invoke(new GitOutput(args.Data, GitOutputStatus.Success));
                    }
                };

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        Debug.LogError(args.Data);
                        errorBuilder.AppendLine(args.Data);
                        OnGitOutput?.Invoke(new GitOutput(args.Data, GitOutputStatus.Error));
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    string error = $"Git command failed with exit code {process.ExitCode}";
                    Debug.LogError(error);
                    OnGitOutput?.Invoke(new GitOutput(error, GitOutputStatus.Error));
                    return null;
                }

                return returnOutput ? outputBuilder.ToString() : null;
            }
            catch (Exception ex)
            {
                string error = $"Git command failed: {ex.Message}";
                Debug.LogError(error);
                OnGitOutput?.Invoke(new GitOutput(error, GitOutputStatus.Error));
                return null;
            }
        }

        private async Task InitializeGitRepositoryAsync()
        {
            var gitDirectory = Path.Combine(_workingDirectory, ".git");
            if (Directory.Exists(gitDirectory))
            {
                Debug.Log("Git repository already initialized.");
                return;
            }

            try
            {
                await RunGitCommandAsync("init");
                Debug.Log(Directory.Exists(gitDirectory) ? "Git repository initialized." : "Failed to initialize Git repository.");
            }
            catch (Exception ex)
            {
                string error = $"Failed to initialize Git repository:  {ex.Message}";
                Debug.LogError(error);
                OnGitOutput?.Invoke(new GitOutput(error, GitOutputStatus.Error));
            }
        }

        private async Task<string> DownloadVersionFile()
        {
            string path = REMOTE_VERSION_FILEPATH;
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Git URL is not set.");
                return null;
            }

            Debug.Log($"Downloading version file from: {path}");
            using UnityWebRequest webRequest = UnityWebRequest.Get(path);
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {webRequest.error}");
                return null;
            }
            else
            {
                return webRequest.downloadHandler.text;
            }
        }
    }


    public static class TaskExtensions
    {
        public static async Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<object>();

            void ProcessExited(object sender, EventArgs e) => tcs.TrySetResult(null);

            process.EnableRaisingEvents = true;
            process.Exited += ProcessExited;

            using (cancellationToken.Register(() =>
            {
                process.Exited -= ProcessExited;
                tcs.TrySetCanceled();
            }))
            {
                await tcs.Task;
            }
        }
    }
}
