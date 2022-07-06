using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CrossPlatformConsole.Architecture.Linux
{
    public class Scanner
    {

        private TextWriter DEBUG_STREAM_WRITER = new StreamWriter("LINUX_SCANNER_DEBUG.OUTPUT", true);

        public void GetSystemInfo_Linux()
        {
            ShellCommand lc = new ShellCommand();

            var os = Environment.OSVersion;

            Console.WriteLine($"    Machine Name       :    {Environment.MachineName}");
            Console.WriteLine($"    Operating System   :    {lc.RunShell(". /etc/os-release; echo \"$PRETTY_NAME\"")}");
            Console.WriteLine($"    Operating Version  :    {lc.RunShell(". /etc/os-release; echo \"$VERSION\"")}");
            //Console.WriteLine($"    Server Chassis     :    {lc.RunShell("hostnamectl | grep Chassis")}");
            Console.WriteLine($"    Distro Version     :    {lc.RunShell(". /etc/lsb-release; echo \"$DISTRIB_RELEASE\"")}");
            Console.WriteLine($"    Distro CODENAME    :    {lc.RunShell(". /etc/lsb-release; echo \"$DISTRIB_CODENAME\"")}");
            Console.WriteLine($@"   Username           :    {Environment.UserDomainName}\{Environment.UserName}");
            Console.WriteLine($@"   Model              :    {lc.RunShell("cat /sys/devices/virtual/dmi/id/product_name")}");
            Console.WriteLine($"Sys. Vendor         :    {lc.RunShell("cat /sys/devices/virtual/dmi/id/sys_vendor")}");
            Console.WriteLine("Writing Computer Information to log...");

            DEBUG_STREAM_WRITER.WriteLine("Computer Information");
            DEBUG_STREAM_WRITER.WriteLine($"Machine Name        :    {Environment.MachineName}");
            DEBUG_STREAM_WRITER.WriteLine($"Operating System    :    {lc.RunShell(". /etc/os-release; echo \"$PRETTY_NAME\"")}");
            //DEBUG_STREAM_WRITER.WriteLine($"Server Chassis     :    {lc.RunShell("hostnamectl | grep Chassis")}");
            DEBUG_STREAM_WRITER.WriteLine($"Operating Version   :    {lc.RunShell(". /etc/os-release; echo \"$VERSION\"")}");
            DEBUG_STREAM_WRITER.WriteLine($"Distro Version      :    {lc.RunShell(". /etc/lsb-release; echo \"$DISTRIB_RELEASE\"")}");
            DEBUG_STREAM_WRITER.WriteLine($"Distro CODENAME     :    {lc.RunShell(". /etc/lsb-release; echo \"$DISTRIB_CODENAME\"")}");
            DEBUG_STREAM_WRITER.WriteLine($@"Username           :    {Environment.UserDomainName}\{Environment.UserName}");
            DEBUG_STREAM_WRITER.WriteLine($"Model               :    {lc.RunShell("cat /sys/devices/virtual/dmi/id/product_name")}");
            DEBUG_STREAM_WRITER.WriteLine($"Sys. Vendor         :    {lc.RunShell("cat /sys/devices/virtual/dmi/id/sys_vendor")}");

        }

        public void GetSoftware_Linux()
        {
            string[] excludeFiles = { "kbuild", "makefile", ".config", ".gitignore", "kconfig", "readme", "copyright" };
            string[] excludePaths = { @"\zoneinfo\" };
            Console.WriteLine("This is the example method for getting all directories on the system");

            string? rootPath = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                rootPath = @"/usr/";
            }

            if (rootPath != null)
            {
                Console.WriteLine("Writing Software Information to log...");
                DEBUG_STREAM_WRITER.WriteLine($"RootPath: {rootPath}");

                foreach (var i in Directory.GetDirectories(rootPath, "*.*", SearchOption.AllDirectories))
                {
                    if (!excludePaths.Contains(i))
                    {
                        DirectoryInfo dir = new(i);

                        try
                        {
                            Console.WriteLine($"Path: {dir.FullName}");
                            DEBUG_STREAM_WRITER.WriteLine($"Path: {dir.FullName}");

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

                                    DEBUG_STREAM_WRITER.WriteLine($"    > File: {f.FullName}");
                                    DEBUG_STREAM_WRITER.WriteLine($"        > Name: {FileVersionInfo.GetVersionInfo(f.FullName).ProductName}");
                                    DEBUG_STREAM_WRITER.WriteLine($"        > Attributes    : {f.Attributes}");
                                    DEBUG_STREAM_WRITER.WriteLine($"        > CreationTime  : {f.CreationTime}");

                                    if (!string.IsNullOrEmpty(FileVersionInfo.GetVersionInfo(f.FullName).ProductVersion))
                                    {
                                        DEBUG_STREAM_WRITER.WriteLine($"    > File Version  : {FileVersionInfo.GetVersionInfo(f.FullName).ProductVersion}");
                                    }
                                }
                            }

                        }
                        catch (UnauthorizedAccessException e)
                        {
                            Console.WriteLine($"oops");
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Massive fucky wucky");
                        }
                    }
                }

                DEBUG_STREAM_WRITER.Close();
            }
        }
    }
}
