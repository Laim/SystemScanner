using System.Diagnostics;
using System.Runtime.InteropServices;
using CrossPlatformConsole;

System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
TextWriter tsw = new StreamWriter(@"log.txt", true);

Console.WriteLine("Hello, World!");

if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    MicrosoftExample();
} else
{
    Example();
}


void Example()
{
    string[] excludeFiles = { "kbuild", "makefile", ".config", ".gitignore", "kconfig", "readme", "copyright"};

    Console.WriteLine("This is the example method for getting all directories on the system");

    string? rootPath = null;

    if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        rootPath = @"/usr/";
    }

    if (rootPath != null)
    {
        TextWriter tsw = new StreamWriter(@"log.txt", true);
        tsw.WriteLine(rootPath);

        foreach (var i in Directory.GetDirectories(rootPath, "*.*", SearchOption.AllDirectories))
        {
            DirectoryInfo dir = new(i);

            try
            {
                Console.WriteLine($"Path: {dir.FullName}");
                tsw.WriteLine($"Path: {dir.FullName}");

                foreach (FileInfo f in dir.GetFilesByExtensions(false, ".au", ".bmp", ".bmu", ".cfg", ".class", ".conf", ".csm", ".css", ".dic", ".enc", ".gif",
                    ".h", ".htm", ".html", ".jpg", ".js", ".log", ".mo", ".mof", ".packlist", ".pcf", ".pc", ".pf", ".pl", ".png", ".po", ".properties",
                    ".rdf", ".sdl", ".so", ".sql", ".ttf", ".txe", ".txt", ".utf8", ".xml", ".xpt", ".zip", ".ko", ".nffw", ".fw", ".pyc", ".oga"))
                {

                    if (!excludeFiles.Contains(f.Name.ToLower()))
                    {

                        Console.WriteLine($"    > File: {f.FullName}");
                        Console.WriteLine($"        > Attributes    : {f.Attributes}");
                        Console.WriteLine($"        > CreationTime  : {f.CreationTime}");
                        Console.WriteLine($"        > File Version  : {FileVersionInfo.GetVersionInfo(f.FullName).FileVersion}");

                        tsw.WriteLine($"    > File: {f.FullName}");
                        tsw.WriteLine($"        > Name: {FileVersionInfo.GetVersionInfo(f.FullName).ProductName}");
                        tsw.WriteLine($"        > Attributes    : {f.Attributes}");
                        tsw.WriteLine($"        > CreationTime  : {f.CreationTime}");

                        if (!string.IsNullOrEmpty(FileVersionInfo.GetVersionInfo(f.FullName).ProductVersion))
                        {
                            tsw.WriteLine($"    > File Version  : {FileVersionInfo.GetVersionInfo(f.FullName).ProductVersion}");
                        }
                    }
                }

            } catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"oops");
                continue;
            }
            catch (Exception ex) {
                Console.WriteLine($"Massive fucky wucky");
            }
        }

        tsw.Close();
    }
}

void MicrosoftExample()
{
    // Start with drives if you have to search the entire computer.
    string[] drives = Environment.GetLogicalDrives();

    foreach (string dr in drives)
    {
        DriveInfo di = new DriveInfo(dr);

        // Here we skip the drive if it is not ready to be read. This
        // is not necessarily the appropriate action in all scenarios.
        if (!di.IsReady)
        {
            Console.WriteLine("The drive {0} could not be read", di.Name);
            continue;
        }
        DirectoryInfo rootDir = di.RootDirectory;
        WalkDirectoryTree(rootDir);
    }

    // Write out all the files that could not be processed.
    Console.WriteLine("Files with restricted access:");
    foreach (string s in log)
    {
        Console.WriteLine(s);
    }
    tsw.Close();
    // Keep the console window open in debug mode.
    Console.WriteLine("Press any key");
    Console.ReadKey();
}

void WalkDirectoryTree(DirectoryInfo root)
{
    FileInfo[] files = null;
    DirectoryInfo[] subDirs = null;

    // First, process all the files directly under this folder
    try
    {
        files = root.GetFiles("*.*");
    }
    // This is thrown if even one of the files requires permissions greater
    // than the application provides.
    catch (UnauthorizedAccessException e)
    {
        // This code just writes out the message and continues to recurse.
        // You may decide to do something different here. For example, you
        // can try to elevate your privileges and access the file again.
        log.Add(e.Message);
    }

    catch (DirectoryNotFoundException e)
    {
        Console.WriteLine(e.Message);
    }

    if (files != null)
    {
        foreach (FileInfo fi in files)
        {
            if(fi.Extension == ".exe")
            {
                // In this example, we only access the existing FileInfo object. If we
                // want to open, delete or modify the file, then
                // a try-catch block is required here to handle the case
                // where the file has been deleted since the call to TraverseTree().
                Console.WriteLine(fi.FullName);
                tsw.WriteLine(fi.FullName);
            }
        }

        // Now find all the subdirectories under this directory.
        subDirs = root.GetDirectories();

        foreach (DirectoryInfo dirInfo in subDirs)
        {
            if(!dirInfo.FullName.Contains(@"C:\Windows\")) {
                // Resursive call for each subdirectory.
                WalkDirectoryTree(dirInfo);
            }
        }
    }

}