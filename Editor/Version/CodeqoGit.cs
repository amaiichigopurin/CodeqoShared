using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace CodeqoEditor.Git
{
    public enum GitOutputStatus
    {
        Info,
        Hint,
        Success,
        Warning,
        Error,
        RealError,
        Fatal,
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

        //private const string RAW_GIT_URL = "https://raw.githubusercontent.com";
        private const string GIT_BRANCH = "master";
        private const string GIT_HINT = "hint: ";
        private const string GIT_WARNING = "warning: ";
        private const string GIT_ERROR = "error: ";
        private const string GIT_FATAL = "fatal: ";

        public event Action<GitOutput> OnGitOutput;

        private readonly string _workingDirectory;
        private readonly string _gitUrl; // should look like https://github.com/amaiichigopurin/CodeqoShared.git
        private readonly string _projectName;

        private GitVersion _localVersion;
        private GitVersion _remoteVersion;
        public GitVersion LocalVersion => _localVersion;
        public GitVersion RemoteVersion => _remoteVersion;

        public CodeqoGit(string workingDirectory, string gitUrl, string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new ArgumentException("Project name cannot be null or empty.", nameof(projectName));
            }

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
            _projectName = projectName;
        }

        public async Task InitializeAsync()
        {
            _localVersion = GitVersion.CreateCurrentVersion(_projectName);

            try
            {
                await InitializeGitRepositoryAsync();
                await PullVersionTagAsync();
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

        public async Task PushAsync(VersionIncrement versionInc, bool force = false)
        {
            string pushCommand = $"push origin {GIT_BRANCH}";
            if (force) pushCommand += " --force";       
            await RunGitCommandAsync("add .");
            await RunGitCommandAsync(pushCommand);
            await PushVersionTagAsync(versionInc);
        }

        /// <summary>
        /// git tag -a "tag" -m "tagInfo"
        /// git push origin--tags
        /// </summary>
        /// <returns></returns>
        public async Task PushVersionTagAsync(VersionIncrement versionInc = VersionIncrement.Patch)
        {
            string tag = _remoteVersion.CreateUpdatedTag(versionInc);
            string tagInfo = _remoteVersion.CreateTagInfo();
            await RunGitCommandAsync($"tag -a {tag} -m \"{tagInfo}\"");
            await RunGitCommandAsync("push origin --tags");
        }

        public async Task PullVersionTagAsync()
        {
            Debug.Log("Getting version info...");
            // Fetch the tags from the remote repository
            await RunGitCommandAsync("fetch --tags");

            // Get the latest tag from the fetched tags
            string latestTag = await RunGitCommandAsync("describe --tags --abbrev=0", true);

            // Check if the latestTag is not null or empty after trimming
            if (!string.IsNullOrEmpty(latestTag))
            {
                _remoteVersion = new GitVersion(_projectName, latestTag);
                Debug.Log($"Latest version tag pulled: {_remoteVersion}");
            }
            else
            {
                Debug.LogError("No valid version tag found.");
                _remoteVersion = new GitVersion();
            }
        }

        public async Task CommitAsync()
        {
            await RunGitCommandAsync("add .");
            await RunGitCommandAsync("commit --no-edit");
        }
    

        public Task StatusAsync() => RunGitCommandAsync("status");

        public async Task<string> PushVersionAsync()
        {
            return await RunGitCommandAsync("remote get-url origin", returnOutput: true);
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
                        outputBuilder.AppendLine(args.Data);
                        HandleResult(args.Data, GitOutputStatus.Success);
                    }
                };

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        errorBuilder.AppendLine(args.Data);
                        HandleResult(args.Data, GitOutputStatus.Error);
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

                return returnOutput ? outputBuilder.ToString().Trim() : null;
            }
            catch (Exception ex)
            {
                string error = $"Git command failed: {ex.Message}";
                Debug.LogError(error);
                OnGitOutput?.Invoke(new GitOutput(error, GitOutputStatus.Error));
                return null;
            }
        }

        void HandleResult(string output, GitOutputStatus status)
        {
            if (output.StartsWith(GIT_HINT))
            {
                output = output.Replace(GIT_HINT, "");
                status = GitOutputStatus.Hint;
                Debug.Log(output);
            }
            else if (output.StartsWith(GIT_WARNING))
            {
                output = output.Replace(GIT_WARNING, "");
                status = GitOutputStatus.Warning;
                Debug.LogWarning(output);
            }
            else if (output.StartsWith(GIT_ERROR))
            {
                output = output.Replace(GIT_ERROR, "");
                status = GitOutputStatus.RealError;
                Debug.LogError(output);
            }
            else if (output.StartsWith(GIT_FATAL))
            {
                output = output.Replace(GIT_FATAL, "");
                status = GitOutputStatus.Fatal;
                Debug.LogError(output);
            }
            else
            {
                Debug.Log(output);
            }

            OnGitOutput?.Invoke(new GitOutput(output, status));
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
