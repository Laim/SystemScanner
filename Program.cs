using System.Runtime.InteropServices;

if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    CrossPlatformConsole.Architecture.Windows.Scanner WindowsScanner = new();

    WindowsScanner.GetSoftware();
    WindowsScanner.GetSystemInfo();
}

if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    CrossPlatformConsole.Architecture.Linux.Scanner LinuxScanner = new();

    LinuxScanner.GetSystemInfo();
    LinuxScanner.GetSoftware();
}