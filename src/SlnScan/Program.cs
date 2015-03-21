using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gosu.Commons.Console;

namespace SlnScan
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("Usage: slnscan \"path/to/sln-root\" [-ignore \"some/ignore/file/path\"]");
                return 0;
            }

            var rootDirectory = args[0];

            if (!Directory.Exists(rootDirectory))
            {
                Console.WriteLine("Could not find directory '{0}'", rootDirectory);
                return 1;
            }

            var scanner = new CsprojFileScanner();
            var arguments = new ArgumentList(args);

            var ignoredNames = GetIgnoredNames(arguments);
            var excludedFiles = scanner.FindExcludedFiles(rootDirectory, ignoredNames).ToList();

            var outputPath = arguments.GetFlagValue("o", "output");

            if (outputPath.HasValue)
            {
                File.WriteAllLines(outputPath.Value, scanner.Output);
            }

            if (excludedFiles.Any())
                return 1;

            return 0;
        }

        private static List<string> GetIgnoredNames(ArgumentList arguments)
        {
            var ignoredNames = new List<string>();
            var ignoreFilePath = arguments.GetFlagValue("ignore", "i");

            if (ignoreFilePath.HasValue)
            {
                ignoredNames = File.ReadAllLines(ignoreFilePath.Value).ToList();
            }

            return ignoredNames;
        }
    }
}
