using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SlnScan
{
    public class IgnorePattern
    {
        private string[] _patternParts;

        public IgnorePattern(string pattern)
        {
            _patternParts = pattern.Split('/');
        }

        public bool IsMatch(string path)
        {
            var parts = GetPathParts(path);

            return IsMatch(parts, _patternParts);
        }

        private bool IsMatch(IList<string> pathParts, IList<string> patternParts)
        {
            if (!pathParts.Any() || !patternParts.Any())
                return false;

            var regex = ToRegex(patternParts.First());

            var isMatchForCurrentLevel = regex.IsMatch(pathParts.First());
            var remainingPatternParts = patternParts.Skip(1).ToList();
            var remainingPathParts = pathParts.Skip(1).ToList();

            if (isMatchForCurrentLevel && remainingPatternParts.Any())
                return IsMatch(remainingPathParts, remainingPatternParts);
            if (isMatchForCurrentLevel)
                return true;

            return IsMatch(remainingPathParts, _patternParts);
        }

        private IList<string> GetPathParts(string path, IList<string> parts = null)
        {
            parts = parts ?? new List<string>();

            if (string.IsNullOrEmpty(path))
                return parts;

            var name = Path.GetFileName(path);

            parts.Insert(0, name);

            return GetPathParts(Path.GetDirectoryName(path), parts);
        }

        private Regex ToRegex(string pattern)
        {
            if (pattern.StartsWith("*."))
                pattern = ".*\\." + pattern.Substring(2);
            else if (pattern.StartsWith("*"))
                pattern = ".*" + pattern.Substring(1);

            pattern = "^" + pattern + "$";

            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }
}