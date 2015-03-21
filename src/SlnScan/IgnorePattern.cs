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

            return IsMatch(parts);
        }

        private bool IsMatch(IList<string> parts)
        {
            if (!parts.Any())
                return false;

            var regex = ToRegex(_patternParts.First());

            if (regex.IsMatch(parts.First()))
                return true;

            return IsMatch(parts.Skip(1).ToList());
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