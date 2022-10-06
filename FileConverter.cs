using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FileConverter
{
    public class FileConverter
    {
        public const string BlueprintExtension = ".blueprint";
        public const string BlueprintJsonExtension = ".blueprint.json";
        public const string LayoutBackupExtension = ".blueprint.backup";
        public const string WorldExtension = ".slot";
        public const string WorldJsonExtension = ".slot.json";
        public const string WorldBackupExtension = ".slot.backup";

        static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        static readonly List<char> LogChars = new() { 'F', 'E', '+', '@', '*', '>' };

        static string GetDevicesPath()
        {
            Guid localLowId = new("A520A1A4-1780-4FF6-BD18-167343C5AF16");
            return Path.Combine(GetKnownFolderPath(localLowId), "DryLicorice", "Plasma", "User Data", "Devices");
        }
        static string GetKnownFolderPath(Guid knownFolderId)
        {
            IntPtr pszPath = IntPtr.Zero;
            try
            {
                int hr = SHGetKnownFolderPath(knownFolderId, 0, IntPtr.Zero, out pszPath);
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
            var resultLog = new List<string>();
            int fileCount = 0, backups = 0;

            string[] files = null;
            files = Directory.GetFiles(GetDevicesPath());

            Console.WriteLine("[>] Working...");

            //foreach (string path in files)
            //{
            string path = files[1];
                var serializer = JsonSerializer.Create(JsonSerializerSettings);
                Console.WriteLine(path);
                Console.WriteLine(String.Join(", ", File.ReadAllBytes(path)[0..337]));
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var reader = new BinaryReader(fs))
                {
                    //var bytes = File.ReadAllBytes(path);
                    var format = Enum.ToObject(Program.DataFormat, 0);
                    var Deserializer = Program.SerializationUtility.GetMethod("DeserializeValue", new[] { typeof(Stream), Program.DataFormat, Program.DeserializationContext });
                    Deserializer = Deserializer.MakeGenericMethod(Program.DeviceMeta);
                    var slot = Deserializer.Invoke(null, BindingFlags.Static, null, new object[] { reader.BaseStream, format, null }, null);
                    var slotJson = JObject.FromObject(slot, serializer);
                    Console.WriteLine(slotJson.ToString());
                }
                fileCount++;
            //}

            resultLog = resultLog
                .Where(s => !string.IsNullOrWhiteSpace(s) && s.Length >= 2)
                .OrderBy(s => LogChars.Contains(s[1]) ? LogChars.IndexOf(s[1]) : LogChars.Last())
                .ToList();

            foreach (string msg in resultLog)
                Console.WriteLine(msg);

            if (resultLog.Count == 0)
            {
                if (fileCount > 0) Console.WriteLine("[>] All files checked, no changes to apply.");
                else if (backups == 0) Console.WriteLine("[>] There are no layout files to convert in this folder.");
                else Console.WriteLine("[>] The only layouts detected are backups and were ignored.");
            }
            else Console.WriteLine($"[>] Done.");
        }

        public static string PathTrim(string path)
        {
            return path?.Substring(path.LastIndexOfAny(new[] { '/', '\\' }) + 1);
        }
    }
}
