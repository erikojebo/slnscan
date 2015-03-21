using System.IO;
using System.Text.RegularExpressions;

namespace SlnScan
{
    public class IgnorePattern
    {
        private readonly string _pattern;

        public IgnorePattern(string pattern)
        {
            _pattern = pattern;

            if (_pattern.StartsWith("*."))
                _pattern = ".*\\." + _pattern.Substring(2);
            else if (_pattern.StartsWith("*"))
                _pattern = ".*" + _pattern.Substring(1);

            _pattern = "^" + _pattern + "$";
        }

        public bool IsMatch(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var name = Path.GetFileName(path);

            if (Regex.IsMatch(name, _pattern, RegexOptions.IgnoreCase))
                return true;

            return IsMatch(Path.GetDirectoryName(path));
        }
    }
}