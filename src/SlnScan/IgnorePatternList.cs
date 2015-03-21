using System;
using System.Collections.Generic;
using System.Linq;

namespace SlnScan
{
    public class IgnorePatternList
    {
        private readonly IEnumerable<IgnorePattern> _patterns;

        public IgnorePatternList()
        {
        }

        private IgnorePatternList(IEnumerable<IgnorePattern> patterns)
        {
            _patterns = patterns;
        }

        public static IgnorePatternList Parse(params string[] patternLines)
        {
            var patterns = patternLines
                .Where(x => !x.StartsWith("#"))
                .Select(x => new IgnorePattern(x));

            return new IgnorePatternList(patterns);
        }

        public bool IsMatch(string path)
        {
            return _patterns.Any(x => x.IsMatch(path));
        }
    }
}