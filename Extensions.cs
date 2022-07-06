
namespace CrossPlatformConsole
{
    public static class Extensions
    {
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, bool includeExtensions = false, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();

            if (includeExtensions)
            {
                return files.Where(f => extensions.Contains(f.Extension));
            } else {
                return files.Where(f => !extensions.Contains(f.Extension));
            }
        }   
    }
}
