using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SlnScan
{
    public class CsprojFileScanner
    {
        public IList<string> Output = new List<string>();

        public IEnumerable<string> FindExcludedFiles(string rootDirectory, IList<string> ignoredNames)
        {
            var csprojFilePaths = Directory.GetFiles(rootDirectory, "*.csproj", SearchOption.AllDirectories);

            return csprojFilePaths.SelectMany(x => FindExcludedFilesInProject(x, ignoredNames));
        }

        private IEnumerable<string> FindExcludedFilesInProject(string csprojFilePath, IList<string> ignoredNames)
        {
            var allFilesInProject = GetFilesInProject(csprojFilePath);
            var allFilesOnDisk = Directory.GetFiles(Path.GetDirectoryName(csprojFilePath), "*", SearchOption.AllDirectories);

            var unignoredFilesOnDisk = ExcludeIgnoredFiles(allFilesOnDisk, ignoredNames);

            var excludedFiles = unignoredFilesOnDisk
                .Where(file => !allFilesInProject.Any(proj => string.Equals(file, proj, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();

            if (excludedFiles.Any())
            {
                WriteLine("Excluded files in " + csprojFilePath);
                
                foreach (var excludedFile in excludedFiles)
                {
                    WriteLine(excludedFile);
                }

                WriteLine();
            }

            return excludedFiles;
        }

        private void WriteLine(string s = "")
        {
            Console.WriteLine(s);
            Output.Add(s);
        }

        private static IEnumerable<string> GetFilesInProject(string csprojFilePath)
        {
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

            var csprojXml = XDocument.Load(csprojFilePath);
            var csprojDirectory = Path.GetDirectoryName(csprojFilePath);

            var matchingElements = new List<XElement>();
            var elementNames = new[] { "Content", "Compile", "EmbeddedResource", "None", "Resource", "Page", "ApplicationDefinition" };

            foreach (var elementName in elementNames)
            {
                var elements = csprojXml.Descendants(msbuild + elementName);
                matchingElements.AddRange(elements);
            }

            var allIncludedFiles = matchingElements
                .Select(x => x.Attribute("Include").Value)
                .Select(x => Path.Combine(csprojDirectory, x))
                .ToList();

            return allIncludedFiles;
        }

        private IEnumerable<string> ExcludeIgnoredFiles(string[] allFilesOnDisk, IList<string> ignoredNames)
        {
            return allFilesOnDisk.Where(x => !IsIgnored(x, ignoredNames));
        }

        private bool IsIgnored(string path, IList<string> ignoredNames)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var name = Path.GetFileName(path);

            if (ignoredNames.Any(ignored => IsMatch(name, ignored)))
                return true;

            return IsIgnored(Path.GetDirectoryName(path), ignoredNames);
        }

        private bool IsMatch(string name, string ignored)
        {
            if (ignored.StartsWith("*."))
                ignored = ".*\\." + ignored.Substring(2);
            else if (ignored.StartsWith("*"))
                ignored = ".*" + ignored.Substring(1);

            var pattern = "^" + ignored + "$";

            return Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase);
        }
    }
}