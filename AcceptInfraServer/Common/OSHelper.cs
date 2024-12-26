using System.Runtime.InteropServices;
using AcceptServer.Domain.Enums;

namespace AcceptServer.Common;
public static class OSHelper
{
    static readonly OSType _osType;
    public static string OSTypeName => _osType.ToString();
    public static bool IsWindows => _osType is OSType.Windows;
    public static bool IsLinux => _osType is OSType.Linux;
    public static bool IsMacOS => _osType is OSType.OSX;
    public static bool IsUnknown => _osType is OSType.Unknown;

    static OSHelper()
    {
        _osType = GetSystemInfo();
    }

    static OSType GetSystemInfo()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return OSType.Windows;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return OSType.Linux;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return OSType.OSX;

        return OSType.Unknown;
    }
}
