using Microsoft.Win32;
using OdinSerializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using DataFormat = OdinSerializer.DataFormat;
using Path = System.IO.Path;

namespace Plasma_Serialization_WPF_Framework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool IsFileLoaded = false;
        private string FileName = string.Empty;
        private string FilePath = string.Empty;
        private FileType FileType;
        public MainWindow()
        {
            InitializeComponent();
            /*
            string gamePath = GetPlasmaPath();
            PlasmaAssembly = Assembly.LoadFrom($"{gamePath}\\Plasma_Data\\Managed\\Assembly-CSharp.dll");
            UnityAssembly = Assembly.LoadFrom($"{gamePath}\\Plasma_Data\\Managed\\UnityEngine.CoreModule.dll");
            SirenixAssembly = Assembly.LoadFrom($"{gamePath}\\Plasma_Data\\Managed\\Sirenix.Serialization.dll");
            SirenixConfigAssembly = Assembly.LoadFrom($"{gamePath}\\Plasma_Data\\Managed\\Sirenix.Serialization.Config.dll");
            */
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                InitialDirectory = Util.GetBaseDir(),
                CheckFileExists = true,
                Filter = "Plasma Files|*.json;*.blueprint;*.metadata;*.world|Blueprint|*.blueprint|JSON|*.json|Metadata|*.metadata|World|*.world"
            };
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                this.DisplayPath.Text = path;
                byte[] json = null;

                this.FileName = Path.GetFileName(path);
                this.FilePath = path;
                object file = null;
                DataFormat df = DataFormat.Binary;
                if (path.EndsWith("json"))
                    df = DataFormat.JSON;

                if (Directory.GetParent(path).Name.Equals("Devices"))
                {
                    if (path.EndsWith("metadata"))
                    {
                        file = SerializationUtility.DeserializeValue<SerializedDeviceMetaData>(File.ReadAllBytes(path), df);
                        this.FileType = FileType.DeviceM;
                    }
                    else if (path.EndsWith("blueprint"))
                    {
                        file = SerializationUtility.DeserializeValue<SerializedDeviceBlueprint>(File.ReadAllBytes(path), df);
                        this.FileType = FileType.Device;
                    }
                }
                else if (Directory.GetParent(path).Parent.Name.Equals("Worlds"))
                {
                    if (path.EndsWith("metadata"))
                    {
                        file = SerializationUtility.DeserializeValue<SerializedWorldMetaData>(File.ReadAllBytes(path), df);
                        this.FileType = FileType.WorldM;
                    }
                    else if (path.EndsWith("world"))
                    {
                        file = SerializationUtility.DeserializeValue<SerializedWorld>(File.ReadAllBytes(path), df);
                        this.FileType = FileType.World;
                    }
                }

                if (file != null)
                    json = SerializationUtility.SerializeValue(file, DataFormat.JSON, null);

                if (json != null)
                {
                    this.Display.Text = Encoding.UTF8.GetString(json);
                    this.SaveAsButton.IsEnabled = true;
                    this.SaveButton.IsEnabled = false;
                    this.IsFileLoaded = true;
                    this.Display.IsReadOnly = false;
                }
                else
                {
                    this.Display.Text = "Error loading file. JSON was null";
                }
            }
        }

        private void Display_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.Display.IsReadOnly && (this.SaveAsButton.IsEnabled == false || this.SaveButton.IsEnabled == false))
            {
                this.SaveAsButton.IsEnabled = true;
                this.SaveButton.IsEnabled = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?\nThis will overwrite the current file", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.SaveFile(this.FilePath);
            }
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog()
            {
                InitialDirectory = Directory.GetParent(this.FilePath)?.FullName,
                FileName = Path.ChangeExtension(this.FileName, ".json"),
                Filter = "JSON|*.json|Metadata|*.metadata|World|*.world|Blueprint|*.blueprint"
            };

            if (saveFile.ShowDialog() == true)
            {
                this.SaveFile(saveFile.FileName);
            }
        }
        private void SaveFile(string path, DataFormat df = DataFormat.JSON)
        {
            byte[] json = null;

            if (path.EndsWith("blueprint") || path.EndsWith("metadata") || path.EndsWith("world"))
                df = DataFormat.Binary;

            object file = null;


            switch (this.FileType)
            {
                case FileType.DeviceM:
                    file = SerializationUtility.DeserializeValue<SerializedDeviceMetaData>(Encoding.UTF8.GetBytes(Display.Text), DataFormat.JSON, null);
                    break;
                case FileType.Device:
                    file = SerializationUtility.DeserializeValue<SerializedDeviceBlueprint>(Encoding.UTF8.GetBytes(Display.Text), DataFormat.JSON, null);
                    break;
                case FileType.WorldM:
                    file = SerializationUtility.DeserializeValue<SerializedWorldMetaData>(Encoding.UTF8.GetBytes(Display.Text), DataFormat.JSON, null);
                    break;
                case FileType.World:
                    file = SerializationUtility.DeserializeValue<SerializedWorld>(Encoding.UTF8.GetBytes(Display.Text), DataFormat.JSON, null);
                    break;
            }

            if (file != null)
            {
                json = SerializationUtility.SerializeValue(file, df, null);
            }

            if (json != null)
            {
                File.WriteAllBytes(path, json);
                this.SaveAsButton.IsEnabled = true;
                this.SaveButton.IsEnabled = false;
                this.IsFileLoaded = true;
                this.Display.IsReadOnly = false;
            }
            else
            {
                this.Display.Text = "Error loading file. JSON was null";
            }

            this.SaveButton.IsEnabled = false;
        }
        static string GetPlasmaPath()
        {
            var paths = new List<string>(10)
            {
                (string)Registry.LocalMachine?.OpenSubKey("SOFTWARE\\WOW6432Node\\Valve\\Steam")?.GetValue("InstallPath")
            };
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
        /*
        public static Assembly PlasmaAssembly { get; private set; }
        public static Assembly UnityAssembly { get; private set; }
        public static Assembly SirenixAssembly { get; private set; }
        public static Assembly SirenixConfigAssembly { get; private set; }

        public static Type DeviceData => PlasmaAssembly.GetType("SerializedDeviceBlueprint");
        public static Type DeviceMeta => PlasmaAssembly.GetType("SerializedDeviceMetaData");

        public static Type WorldData => PlasmaAssembly.GetType("SerializedWorld");
        public static Type WorldMeta => PlasmaAssembly.GetType("SerializedWorldMetaData");

        public static Type sSerializationUtility => SirenixAssembly.GetType("Sirenix.Serialization.SerializationUtility");
        public static Type sDeserializationContext => SirenixAssembly.GetType("Sirenix.Serialization.DeserializationContext");
        public static Type sDataFormat => SirenixConfigAssembly.GetType("Sirenix.Serialization.DataFormat");
        */
    }
}
