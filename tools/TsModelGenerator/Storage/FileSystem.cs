namespace Pims.Tools.TsModelGenerator.Storage
{
    /// <summary>
    /// Represents the file system.
    /// </summary>
    internal static class FileSystem
    {
        public static void ClearDirectory(string directory)
        {

            var directoryInfo = new DirectoryInfo(directory);
            if (!directoryInfo.Exists)
            { return; }

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
        }
    }
}