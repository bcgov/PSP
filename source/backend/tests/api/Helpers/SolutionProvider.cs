using System.IO;
using System.Linq;

public static class SolutionProvider
{
    // Find the root directory containing the sln file.
    public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
    {
        var directory = new DirectoryInfo(
            currentPath ?? Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        return directory;
    }
}
