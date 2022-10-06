using CrossPlatformConsole.FileGenerator;
using System.Diagnostics;
using System.Management;
using System.Runtime.Versioning;

namespace CrossPlatformConsole.Architecture.Windows
{
    [SupportedOSPlatform("windows")]
    public class Scanner
    {
        private TextWriter DEBUG_STREAM_WRITER = new StreamWriter(@"output\WINDOWS_SCANNER_DEBUG.OUTPUT", true);

        // This code is from Microsoft Documentation
        // and is not final, it's just a PoC 
        // to see Cross Platform working

        internal void GetSystemInfo()
        {
            Console.WriteLine($"    Machine Name       :    {Environment.MachineName}");
            Console.WriteLine($"    Operating System   :    {GetOperatingSystem()}");
            Console.WriteLine($"    Operating Version  :    {Environment.OSVersion.Version}");
            Console.WriteLine($@"   Username           :    {Environment.UserDomainName}\{Environment.UserName}");
            Console.WriteLine($@"   Model              :    {""}");
            Console.WriteLine($"Sys. Vendor         :    {""}");
            Console.WriteLine("Writing Computer Information to log...");

            DEBUG_STREAM_WRITER.WriteLine($"    Machine Name       :    {Environment.MachineName}");
            DEBUG_STREAM_WRITER.WriteLine($"    Operating System   :    {GetOperatingSystem()}");
            DEBUG_STREAM_WRITER.WriteLine($"    Operating Version  :    {Environment.OSVersion.Version}");
            DEBUG_STREAM_WRITER.WriteLine($@"   Username           :    {Environment.UserDomainName}\{Environment.UserName}");
            DEBUG_STREAM_WRITER.WriteLine($@"   Model              :    {""}");
            DEBUG_STREAM_WRITER.WriteLine($"Sys. Vendor         :    {""}");
        }

        internal void GetSoftware()
        {
            // Start with drives if you have to search the entire computer.
            string[] drives = Environment.GetLogicalDrives();

            new WriteToXml();

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

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        internal void WalkDirectoryTree(DirectoryInfo root)
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
                DEBUG_STREAM_WRITER.WriteLine(e.Message);
            }

            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    if (fi.Extension == ".exe") // we only care about executables tbf
                    {
                        Console.WriteLine(fi.FullName);
                        DEBUG_STREAM_WRITER.WriteLine(fi.FullName);
                        GetExecutableVersion(fi.FullName);
                    }
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    if (!dirInfo.FullName.Contains(@"\Windows\")) // this needs to be changed to read multiple exclusion directories, just %WINDIR% currently
                    {
                        WalkDirectoryTree(dirInfo);
                    }
                }
            }

        }

        private string GetOperatingSystem()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();

            if(string.IsNullOrEmpty(name.ToString()))
            {
                return "Unknown";
            } else
            {
                return name.ToString();
            }
        }

        private void GetExecutableVersion(string path)
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(path);

            

            Console.WriteLine(versionInfo.FileVersion);
            Console.WriteLine(versionInfo.ProductVersion);
            Console.WriteLine(versionInfo.CompanyName);
            Console.WriteLine(versionInfo.Language);
            Console.WriteLine(versionInfo.ProductName);

            WriteToXml.instance.Write(versionInfo.ProductName, path, versionInfo.FileVersion, versionInfo.CompanyName);

            DEBUG_STREAM_WRITER.WriteLine(versionInfo.FileVersion);
            DEBUG_STREAM_WRITER.WriteLine(versionInfo.ProductVersion);
            DEBUG_STREAM_WRITER.WriteLine(versionInfo.CompanyName);
            DEBUG_STREAM_WRITER.WriteLine(versionInfo.Language);
            DEBUG_STREAM_WRITER.WriteLine(versionInfo.ProductName);
        }
    }
}
