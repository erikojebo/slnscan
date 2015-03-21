using NUnit.Framework;

namespace SlnScan.Tests
{
    [TestFixture]
    public class IgnorePatternListTests
    {
        [Test]
        public void Commented_pattern_does_not_match_anything()
        {
            var list = IgnorePatternList.Parse("#*.cs");

            Assert.IsFalse(list.IsMatch("Program.cs"));
        }

        [Test]
        public void Line_following_commented_line_matches_input()
        {
            var list = IgnorePatternList.Parse("#*.cs", "*.js");

            Assert.IsTrue(list.IsMatch("jquery.min.js"));
        }
    }
}