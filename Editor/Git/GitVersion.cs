using System;
using System.Text.RegularExpressions;

/// <summary>
/// Version.txt looks like below
/// Version: 1.4.3 (major, minor, patch)
/// Build: 456
/// Release Date: 2023-11-01
/// </summary>

namespace CodeqoEditor.Git
{
    public enum VersionIncrement
    {
        Major,
        Minor,
        Patch
    }

    public struct GitVersion : IEquatable<GitVersion>, IComparable<GitVersion>
    {
        public static GitVersion CreateCurrentVersion(string projectName)
        {
            string envPrefix = CreateEnvPrefix(projectName);
            string versionTag = Environment.GetEnvironmentVariable(envPrefix) ?? null;
            if (versionTag == null) return new GitVersion();
            return new GitVersion(projectName, versionTag);
        }
        
        public static string CreateEnvPrefix(string projectName)
        {
            return string.Format(ENV_GITHUB_PROJECT_VERSION_PREFIX, projectName);
        }

        private const string VERSION_TAG_PATTERN = @"v(\d+)\.(\d+)\.(\d+)-build-(\d+)-(\d{4}-\d{2}-\d{2})";
        private const string VERSION_TAG_FORMAT = "v{0}.{1}.{2}-build-{3}-{4}";
        private const string VERSION_TAG_INFO_FORMAT = "Version {0}.{1}.{2} Build {3} Date {4}";
        private const string ENV_GITHUB_PROJECT_VERSION_PREFIX = "GITHUB_{0}";

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Patch { get; private set; }
        public int Build { get; private set; }
        public string ReleaseDate { get; private set; }
        public bool IsValid { get; private set; }

        private string _projectName;

        public GitVersion(string projectName, string tag)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new ArgumentException("Project name cannot be null or empty.", nameof(projectName));
            }

            _projectName = projectName.ToUpper();

            var match = Regex.Match(tag, VERSION_TAG_PATTERN);
            if (match.Success && match.Groups.Count == 6)
            {
                IsValid = true;
                Major = int.Parse(match.Groups[1].Value);
                Minor = int.Parse(match.Groups[2].Value);
                Patch = int.Parse(match.Groups[3].Value);
                Build = int.Parse(match.Groups[4].Value);
                ReleaseDate = match.Groups[5].Value;
                
                string envPrefix = CreateEnvPrefix(_projectName);
                Environment.SetEnvironmentVariable(envPrefix, tag);
            }
            else
            {
                throw new FormatException("The provided tag does not match the expected pattern.");
            }
        }
        
        public string CreateTag()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            return CreateTag(currentDate);
        }
        
        public string CreateTag(string dateString)
        {
            return string.Format(VERSION_TAG_FORMAT, Major, Minor, Patch, Build, dateString);
        }

        public string CreateTagInfo()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            return string.Format(VERSION_TAG_INFO_FORMAT, Major, Minor, Patch, Build, currentDate);
        }

        public string CreateUpdatedTag(VersionIncrement increment)
        {
            switch (increment)
            {
                case VersionIncrement.Major:
                    Major++;
                    break;
                case VersionIncrement.Minor:
                    Minor++;
                    break;
                case VersionIncrement.Patch:
                    Patch++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(increment), "Unsupported version increment specified.");
            }

            int sumVersion = Major + Minor + Patch;
            
            if (Build < sumVersion)
            {
                Build = sumVersion;
            }
            else
            {
                Build++;
            }

            string newTag = CreateTag();            
            string envPrefix = CreateEnvPrefix(_projectName);
            Environment.SetEnvironmentVariable(envPrefix, newTag);

            return newTag;
        }

        // compare with build number
        public int CompareTo(GitVersion other)
        {
            if (other == null)
                return 1;
            if (Build > other.Build)
                return 1;
            if (Build < other.Build)
                return -1;
            return 0;
        }

        public bool Equals(GitVersion other)
        {
            if (other == null)
                return false;
            return Build == other.Build;
        }

        public static bool operator >(GitVersion version1, GitVersion version2)
        {
            return version1.CompareTo(version2) > 0;
        }

        public static bool operator <(GitVersion version1, GitVersion version2)
        {
            return version1.CompareTo(version2) < 0;
        }

        public static bool operator >=(GitVersion version1, GitVersion version2)
        {
            return version1.CompareTo(version2) >= 0;
        }

        public static bool operator <=(GitVersion version1, GitVersion version2)
        {
            return version1.CompareTo(version2) <= 0;
        }

        public static bool operator ==(GitVersion version1, GitVersion version2)
        {
            if (ReferenceEquals(version1, version2))
                return true;
            if (ReferenceEquals(version1, null))
                return false;
            if (ReferenceEquals(version2, null))
                return false;
            return version1.Equals(version2);
        }

        public static bool operator !=(GitVersion version1, GitVersion version2)
        {
            return !(version1 == version2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is GitVersion))
                return false;
            return Build == ((GitVersion)obj).Build;
        }

        public override int GetHashCode()
        {
            return Build.GetHashCode();
        }
    }
}