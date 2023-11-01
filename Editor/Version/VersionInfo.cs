using System;
using System.IO;
using Debug = UnityEngine.Debug;

/// <summary>
/// Version.txt looks like below
/// Version: 1.4.3 (major, minor, patch)
/// Build: 456
/// Release Date: 2023-11-01
/// </summary>

namespace CodeqoEditor
{
    public struct VersionInfo : IEquatable<VersionInfo>, IComparable<VersionInfo>
    {
        public enum VersionType
        {
            Major,
            Minor,
            Patch
        }

        public int Major;
        public int Minor;
        public int Patch;
        public int Build;
        public string ReleaseDate;
        public bool IsValid;

        public VersionInfo(bool isValid)
        {
            Major = 0;
            Minor = 0;
            Patch = 0;
            Build = 0;
            ReleaseDate = "";
            IsValid = isValid;
        }

        public VersionInfo(string text)
        {
            string[] lines = text.Split('\n');
            string version = lines[0].Split(':')[1].Trim();
            string build = lines[1].Split(':')[1].Trim();
            string releaseDate = lines[2].Split(':')[1].Trim();
            string[] versionParts = version.Split('.');
            Major = int.Parse(versionParts[0]);
            Minor = int.Parse(versionParts[1]);
            Patch = int.Parse(versionParts[2]);
            Build = int.Parse(build);
            ReleaseDate = releaseDate;
            IsValid = true;
        }
        
        public VersionInfo(string[] lines)
        {
            string version = lines[0].Split(':')[1].Trim();
            string build = lines[1].Split(':')[1].Trim();
            string releaseDate = lines[2].Split(':')[1].Trim();
            string[] versionParts = version.Split('.');
            Major = int.Parse(versionParts[0]);
            Minor = int.Parse(versionParts[1]);
            Patch = int.Parse(versionParts[2]);
            Build = int.Parse(build);
            ReleaseDate = releaseDate;
            IsValid = true;
        }

        public static VersionInfo CreateFromFilePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"Version file does not exist at {filePath}");
                return new VersionInfo(false);
            }
            Debug.Log($"Reading version file from {filePath}");
            string[] lines = File.ReadAllLines(filePath);
            return new VersionInfo(lines);
        }    

        public void IncrementBuild(string writePath, VersionType versionType)
        {
            switch (versionType)
            {
                case VersionType.Major:
                    Major++;
                    break;
                case VersionType.Minor:
                    Minor++;
                    break;
                case VersionType.Patch:
                    Patch++;
                    break;
            }
            string version = $"{Major}.{Minor}.{Patch}";
            string build = (Build + 1).ToString();
            string releaseDate = DateTime.Now.ToString("yyyy-MM-dd");
            string newVersionFile = $"Version: {version}\nBuild: {build}\nRelease Date: {releaseDate}";
            File.WriteAllText(writePath, newVersionFile);
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch} (Build: {Build}) - {ReleaseDate}";
        }

        // compare with build number
        public int CompareTo(VersionInfo other)
        {
            if (other == null)
                return 1;
            if (Build > other.Build)
                return 1;
            if (Build < other.Build)
                return -1;
            return 0;
        }

        public bool Equals(VersionInfo other)
        {
            if (other == null)
                return false;
            return Build == other.Build;
        }

        public static bool operator >(VersionInfo version1, VersionInfo version2)
        {
            return version1.CompareTo(version2) > 0;
        }

        public static bool operator <(VersionInfo version1, VersionInfo version2)
        {
            return version1.CompareTo(version2) < 0;
        }

        public static bool operator >=(VersionInfo version1, VersionInfo version2)
        {
            return version1.CompareTo(version2) >= 0;
        }

        public static bool operator <=(VersionInfo version1, VersionInfo version2)
        {
            return version1.CompareTo(version2) <= 0;
        }

        public static bool operator ==(VersionInfo version1, VersionInfo version2)
        {
            if (ReferenceEquals(version1, version2))
                return true;
            if (ReferenceEquals(version1, null))
                return false;
            if (ReferenceEquals(version2, null))
                return false;
            return version1.Equals(version2);
        }

        public static bool operator !=(VersionInfo version1, VersionInfo version2)
        {
            return !(version1 == version2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is VersionInfo))
                return false;
            return Build == ((VersionInfo)obj).Build;
        }

        public override int GetHashCode()
        {
            return Build.GetHashCode();
        }
    }
}