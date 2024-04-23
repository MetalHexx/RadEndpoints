namespace RadEndpoints.Cli.Helpers
{
    /// <summary>
    /// Some helper functions for file system directory operations
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// Starting from the environment's running location, scans the parent directories and 
        /// returns the first directory containingthe file
        /// </summary>
        /// <param name="fileName">The file to look for</param>
        /// <returns>The path of the file without the filename</returns>
        public static string? FindFileLocationInParentDirectory(this string fileName)
        {
            string? currentDirectory = Environment.CurrentDirectory;

            while (currentDirectory != null)
            {
                string filePath = Path.Combine(currentDirectory, fileName);
                if (File.Exists(filePath))
                {
                    return currentDirectory;
                }

                currentDirectory = Directory.GetParent(currentDirectory)?.FullName; 
            }
            return string.Empty; 
        }

        /// <summary>
        /// Checks to see if a directory exists and creates it if not.
        /// </summary>
        /// <param name="path">Path to check and create</param>
        public static void EnsureDirectoryExists(this string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Takes a list of strings and forms them together into a proper path.
        /// </summary>
        /// <param name="paths">List of strings representing segments of a path</param>
        /// <returns>A valid combined path string</returns>
        public static string CombinePaths(this string startingPath, params string[] paths)
        {
            var finalPath = string.Empty;
            foreach (var path in paths)
            {
                finalPath = Path.Combine(startingPath, finalPath, path);
            }
            return finalPath;
        }
    }
}
