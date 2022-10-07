using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlasmaSerializationWPF
{
    internal class Util
    {
        internal static readonly Guid LocalLowId = new("A520A1A4-1780-4FF6-BD18-167343C5AF16");
        internal static string GetDevicesPath() => Path.Combine(GetBaseDir(), "Devices");
        internal static string GetWorldsPath() => Path.Combine(GetBaseDir(), "Worlds");
        internal static string GetBaseDir() => Path.Combine(GetLocalLowPath(), SecondLevelPath);
        internal static readonly string SecondLevelPath = Path.Combine("DryLicorice", "Plasma", "User Data");
        internal static string GetLocalLowPath()
        {
            IntPtr pszPath = IntPtr.Zero;
            try
            {
                int hr = SHGetKnownFolderPath(LocalLowId, 0, IntPtr.Zero, out pszPath);
                if (hr >= 0)
                    return Marshal.PtrToStringAuto(pszPath);
                throw Marshal.GetExceptionForHR(hr);
            }
            finally
            {
                if (pszPath != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(pszPath);
            }
        }

        [DllImport("shell32.dll")]
        static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr pszPath);
    }
    internal enum FileType
    {
        Json,
        Device,
        World,
        DeviceM,
        WorldM
    }
}
