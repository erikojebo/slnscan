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
        }

        public bool IsMatch(string path)
        {
            var pattern = "^" + _pattern + "$";

            return Regex.IsMatch(path, pattern, RegexOptions.IgnoreCase);
        }
    }
}