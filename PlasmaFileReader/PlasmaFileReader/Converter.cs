using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text.Json;
using OdinSerializer;
using OdinSerializer.Utilities;
using System.Text;
using Rewired;
using PlasmaFileReader.Plasma.Classes;

namespace Plasma
{
    public class FileConverter
    {
        static Guid LocalLowId = new Guid("A520A1A4-1780-4FF6-BD18-167343C5AF16");
        static string GetDevicesPath() => Path.Combine(GetBaseDir(), "Devices");
        static string GetWorldsPath() => Path.Combine(GetBaseDir(), "Worlds");
        static string GetBaseDir() => Path.Combine(GetLocalLowPath(), SecondLevelPath);
        static string SecondLevelPath = Path.Combine("DryLicorice", "Plasma", "User Data");
        static string GetLocalLowPath()
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
        public static void ConvertAll()
        {
            List<string> files = new();
            files.AddRange(Directory.GetFiles(GetDevicesPath()));
            files.AddRange(Directory.GetFiles(GetWorldsPath()));
            Console.WriteLine("[>] Working...");

            foreach (string path in files)
            {
                if (!path.EndsWith(".pt.json"))
                {
                    Console.WriteLine("[#] " + path);
                    string newpath = path + ".pt.json";
                    if (!File.Exists(newpath))
                    {
                        Console.WriteLine("[>] Converting to " + path + ".pt.json");
                        if (Directory.GetParent(path).Name.Equals("Devices"))
                        {
                            if (path.EndsWith("metadata"))
                            {
                                SerializedDeviceMetaData file = SerializationUtility.DeserializeValue<SerializedDeviceMetaData>(File.ReadAllBytes(path), DataFormat.Binary, null);
                                byte[] json = SerializationUtility.SerializeValue<SerializedDeviceMetaData>(file, DataFormat.JSON, null);
                                File.WriteAllBytes(newpath, json);
                            }
                            else if (path.EndsWith("blueprint"))
                            {
                                SerializedDeviceBlueprint file = SerializationUtility.DeserializeValue<SerializedDeviceBlueprint>(File.ReadAllBytes(path), DataFormat.Binary, null);
                                byte[] json = SerializationUtility.SerializeValue<SerializedDeviceBlueprint>(file, DataFormat.JSON, null);
                                File.WriteAllBytes(newpath, json);
                            }
                        }
                        else if (Directory.GetParent(path).Name.Equals("Worlds"))
                        {
                            if (path.EndsWith("metadata"))
                            {
                                SerializedWorldMetaData file = SerializationUtility.DeserializeValue<SerializedWorldMetaData>(File.ReadAllBytes(path), DataFormat.Binary, null);
                                byte[] json = SerializationUtility.SerializeValue<SerializedWorldMetaData>(file, DataFormat.JSON, null);
                                File.WriteAllBytes(newpath, json);
                            }
                            else if (path.EndsWith("world"))
                            {
                                SerializedWorld file = SerializationUtility.DeserializeValue<SerializedWorld>(File.ReadAllBytes(path), DataFormat.Binary, null);
                                byte[] json = SerializationUtility.SerializeValue<SerializedWorld>(file, DataFormat.JSON, null);
                                File.WriteAllBytes(newpath, json);
                            }
                        }
                    }
                }
            }
            Console.WriteLine("[>] All files converted");
            Console.WriteLine($"[>] Done.");
        }
    }
}
