using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SlnScan.Tests
{
    [TestFixture]
    public class IgnorePatternTests
    {
        [Test]
        public void Leading_asterisk_matches_prefixed_letters()
        {
            var pattern = new IgnorePattern("*Controller");

            Assert.IsTrue(pattern.IsMatch("CustomerController"));
        }
        
        [Test]
        public void Leading_asterisk_matches_suffix_without_any_leading_characters()
        {
            var pattern = new IgnorePattern("*Controller");

            Assert.IsTrue(pattern.IsMatch("Controller"));
        }

        [Test]
        public void Leading_asterisk_matches_prefix_with_non_letters()
        {
            var pattern = new IgnorePattern("*Controller");

            Assert.IsTrue(pattern.IsMatch("_Foo-bar.Controller"));
        }

        [Test]
        public void Leading_asterisk_followed_by_dot_matches_pure_letter_file_name_with_the_given_extension()
        {
            var pattern = new IgnorePattern("*.cs");

            Assert.IsTrue(pattern.IsMatch("Program.cs"));
        }

        [Test]
        public void Leading_asterisk_followed_by_dot_matches_pure_mixed_character_file_name_with_the_given_extension()
        {
            var pattern = new IgnorePattern("*.cs");

            Assert.IsTrue(pattern.IsMatch("_foo-bar.baz.cs"));
        }
        
        [Test]
        public void Leading_asterisk_followed_by_dot_does_not_match_name_without_extension_dot()
        {
            var pattern = new IgnorePattern("*.obj");

            Assert.IsFalse(pattern.IsMatch("obj"));
        }
    }
}
