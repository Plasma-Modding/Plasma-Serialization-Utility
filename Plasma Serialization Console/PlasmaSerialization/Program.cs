using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Plasma
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            bool hasArgs = args != null && args.Length > 0;
            string gamePath = null;

            if (!hasArgs) Console.WriteLine("[#] Booting up Plasma File Converter");

            if (File.Exists(ManualGamePath))
            {
                try
                {
                    gamePath = $"{File.ReadAllText(ManualGamePath).Trim()}";
                }
                catch (Exception e) // Could happen if it can't read files, I suppose
                {
                    Console.WriteLine($"[Fatal Error] Failed to grab Plasma location from {ManualGamePath}: {e.Message}");
                    if (!hasArgs)
                    {
                        Console.WriteLine("\n[#] The program will now close.");
                        Console.ReadLine();
                    }
                    return ExitCodeGamePathError;
                }

                Console.WriteLine($"[#] Grabbed Plasma install location from {ManualGamePath}");
            }
            else
            {
                var errors = new List<Exception>(2);

                try { gamePath = GetPlasmaPath(); }
                catch (Exception e) { errors.Add(e); }

                if (gamePath == null)
                {
                    Console.WriteLine($"[Error] Failed to locate Plasma installation folder.");
                    Console.WriteLine($" You can manually set the location by creating a file here called \"{ManualGamePath}\" " +
                        "and writing the location of your game folder in that file, then restarting this program.");
                    foreach (var e in errors) Console.WriteLine($"\n[#] Error message: {e.Message}");
                    if (!hasArgs)
                    {
                        Console.WriteLine("\n[#] The program will now close.");
                        Console.ReadLine();
                    }
                    return ExitCodeGamePathError;
                }

                if (!hasArgs) Console.WriteLine($"[#] Automatically detected Plasma installation");
            }

            try
            {
                PlasmaAssembly = Assembly.LoadFrom($"{gamePath}\\Plasma_Data\\Managed\\Assembly-CSharp.dll");
                UnityAssembly = Assembly.LoadFrom($"{gamePath}\\Plasma_Data\\Managed\\UnityEngine.CoreModule.dll");
                SirenixAssembly = Assembly.LoadFrom($"{gamePath}\\Plasma_Data\\Managed\\Sirenix.Serialization.dll");
                SirenixConfigAssembly = Assembly.LoadFrom($"{gamePath}\\Plasma_Data\\Managed\\Sirenix.Serialization.Config.dll");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Fatal Error] Failed to load Plasma libraries at \"{gamePath}\":\n {e}");
                if (!hasArgs)
                {
                    Console.WriteLine("\n[#] The program will now close.");
                    Console.ReadLine();
                }
                return ExitCodeGamePathError;
            }

            while (true)
            {
                FileConverter.ConvertAll();
                Console.WriteLine("\n[#] Press Enter to run the program again.");
                Console.ReadLine();
            }
        }


        static string GetPlasmaPath()
        {
            var paths = new List<string>(10);

            paths.Add((string)Registry.LocalMachine?.OpenSubKey("SOFTWARE\\WOW6432Node\\Valve\\Steam")?.GetValue("InstallPath"));
            if (paths[0] == null || !Directory.Exists(paths[0])) return null;

            string config = File.ReadAllText($"{paths[0]}\\config\\config.vdf");
            foreach (Match match in Regex.Matches(config, "\"BaseInstallFolder_[0-9]\"\\s+\"([^\"]+)\""))
            {
                paths.Add(match.Groups[1].Value.Replace("\\\\", "\\"));
            }
            foreach (string path in paths)
            {
                string assembliesPath = $"{path}\\steamapps\\common\\Plasma Demo";
                if (Directory.Exists(assembliesPath)) return assembliesPath;
            }

            return null;
        }

        const int ExitCodeSuccessful = 0;
        const int ExitCodeJsonError = 1;
        const int ExitCodeConversionError = 2;
        const int ExitCodeFileError = 3;
        const int ExitCodeGamePathError = 4;

        const string ManualGamePath = "gamepath.txt";

        public static Assembly PlasmaAssembly { get; private set; }
        public static Assembly UnityAssembly { get; private set; }
        public static Assembly SirenixAssembly { get; private set; }
        public static Assembly SirenixConfigAssembly { get; private set; }

        public static Type DeviceData => PlasmaAssembly.GetType("SerializedDeviceBlueprint");
        public static Type DeviceMeta => PlasmaAssembly.GetType("SerializedDeviceMetaData");

        public static Type WorldData => PlasmaAssembly.GetType("SerializedWorld");
        public static Type WorldMeta => PlasmaAssembly.GetType("SerializedWorldMetaData");

        public static Type SerializationUtility => SirenixAssembly.GetType("Sirenix.Serialization.SerializationUtility");
        public static Type DeserializationContext => SirenixAssembly.GetType("Sirenix.Serialization.DeserializationContext");
        public static Type DataFormat => SirenixConfigAssembly.GetType("Sirenix.Serialization.DataFormat");
    }
}
