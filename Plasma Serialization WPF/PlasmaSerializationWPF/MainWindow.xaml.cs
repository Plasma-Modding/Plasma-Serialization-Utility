using Microsoft.Win32;
using OdinSerializer;
using PlasmaSerializationWPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using DataFormat = OdinSerializer.DataFormat;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.Reflection.Metadata;

namespace PlasmaFileEditor
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
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
                IPlasmaType file = null;
                DataFormat df = DataFormat.Binary;
                if (path.EndsWith("json"))
                    df = DataFormat.JSON;

                if (Directory.GetParent(path).Name.Equals("Devices"))
                {
                    if (path.EndsWith("metadata"))
                    {
                        file = SerializationUtility.DeserializeValue<SerializedDeviceMetaData>(File.ReadAllBytes(path), df, null);
                        this.FileType = FileType.DeviceM;
                    }
                    else if (path.EndsWith("blueprint"))
                    {
                        file = SerializationUtility.DeserializeValue<SerializedDeviceBlueprint>(File.ReadAllBytes(path), df, null);
                        this.FileType = FileType.Device;
                    }
                }
                else if (Directory.GetParent(path).Name.Equals("Worlds"))
                {
                    if (path.EndsWith("metadata"))
                    {
                        file = SerializationUtility.DeserializeValue<SerializedWorldMetaData>(File.ReadAllBytes(path), df, null);
                        this.FileType = FileType.WorldM;
                    }
                    else if (path.EndsWith("world"))
                    {
                        file = SerializationUtility.DeserializeValue<SerializedWorld>(File.ReadAllBytes(path), df, null);
                        
                        this.FileType = FileType.World;
                    }
                }
                if(file is not null)
                    json = SerializationUtility.SerializeValue(file, DataFormat.JSON, null);

                if (json is not null)
                {
                    this.Display.Text = Encoding.UTF8.GetString(json).Replace("System.Private.CoreLib", "mscorlib");
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
            if(!this.Display.IsReadOnly && (this.SaveAsButton.IsEnabled == false || this.SaveButton.IsEnabled == false))
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
            SaveFileDialog saveFile = new()
            {
                InitialDirectory = Directory.GetParent(this.FilePath)?.FullName,
                FileName = Path.ChangeExtension(this.FileName, ".json"),
                Filter = "JSON|*.json|Metadata|*.metadata|World|*.world|Blueprint|*.blueprint"
            };

            if (saveFile.ShowDialog() == true)
            {
                this.SaveFile(saveFile.FileName);
                /*
                if (File.Exists(saveFile.FileName))
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?\nThis will overwrite the current file", "Delete Confirmation", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        this.SaveFile(this.FilePath);
                    }
                }
                else
                {
                    this.SaveFile(saveFile.FileName);
                }
                */
            }
        }
        private void SaveFile(string path, DataFormat df = DataFormat.JSON)
        {
            byte[] json = null;

            if (path.EndsWith("blueprint") || path.EndsWith("metadata") || path.EndsWith("world"))
                df = DataFormat.Binary;

            IPlasmaType file = null;

            
            switch(this.FileType)
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

            if (file is not null)
                json = SerializationUtility.SerializeValue(file, df, null);
            
            //object content = SerializationUtility.DeserializeValue<object>(Encoding.UTF8.GetBytes(Display.Text), DataFormat.JSON, null);
            //json = SerializationUtility.SerializeValue(content, df, null);
            
            if (json is not null)
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
    }
}
