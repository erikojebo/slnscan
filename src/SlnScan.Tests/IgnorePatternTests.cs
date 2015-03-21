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

        [Test]
        public void Two_directory_names_separated_by_slash_matches_the_exact_same_path()
        {
            var pattern = new IgnorePattern("lib/js");

            Assert.IsTrue(pattern.IsMatch("lib/js"));
        }

        [Test]
        public void Two_directory_names_separated_by_slash_does_not_match_other_file_in_parent_directory()
        {
            var pattern = new IgnorePattern("lib/js");

            Assert.IsFalse(pattern.IsMatch("lib/css"));
        }

        [Test]
        public void Two_directory_names_separated_by_slash_matches_longer_path_ending_in_exact_match()
        {
            var pattern = new IgnorePattern("lib/js");

            Assert.IsTrue(pattern.IsMatch("c:/root/lib/js"));
        }

        [Test]
        public void Two_directory_names_separated_by_slash_matches_longer_path_beginning_with_exact_match()
        {
            var pattern = new IgnorePattern("lib/js");

            Assert.IsTrue(pattern.IsMatch("lib/js/child"));
        }
        
        [Test]
        public void Directory_name_followed_by_file_glob_pattern_matches_file_in_parent_directory_matching_glob_pattern()
        {
            var pattern = new IgnorePattern("src/*.cs");

            Assert.IsTrue(pattern.IsMatch("c:/code/foo/src/Program.cs"));
        }
        
        [Test]
        public void Directory_name_with_wildcard_followed_by_file_glob_pattern_matches_matching_file_name_in_matching_directory_name()
        {
            var pattern = new IgnorePattern("*Images/*.png");

            Assert.IsTrue(pattern.IsMatch("c:/UploadedImages/Horse.png"));
        }
        
        [Test]
        public void Multi_level_pattern_matches_path_using_backslash_separators()
        {
            var pattern = new IgnorePattern("*Images/*.png");

            Assert.IsTrue(pattern.IsMatch("c:\\UploadedImages\\Horse.png"));
        }

        [Test]
        public void Multi_level_pattern_does_not_match_shorter_path()
        {
            var pattern = new IgnorePattern("*Images/*.png");

            Assert.IsFalse(pattern.IsMatch("c:\\UploadedImages"));
        }
    }
}
