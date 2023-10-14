using Microsoft.Win32;
using OdinSerializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using DataFormat = OdinSerializer.DataFormat;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;

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
        public NewTextBlock Display;

        public MainWindow()
        {
            InitializeComponent();
            this.Display = new NewTextBlock(this.Dispatcher);
            this.TextViewer.ItemsSource = this.Display.Lines;
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
                    Task.Run(() =>
                    { 
                        this.Display.Text = Encoding.UTF8.GetString(json);
                    });
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
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                CopySelectedLines();
            }
        }
        private void CopySelectedLines()
        {
            if (this.TextViewer.SelectedItems != null && this.TextViewer.SelectedItems.Count > 0)
            {
                var sortedSelectedItems = new List<EfficientTextBlock>(this.TextViewer.SelectedItems.Cast<EfficientTextBlock>())
                                    .OrderBy(item => this.TextViewer.Items.IndexOf(item));

                string textToCopy = string.Join(Environment.NewLine, sortedSelectedItems.Select(item => item.Text));
                Clipboard.SetText(textToCopy);
            }
        }
    }

    public class NewTextBlock
    {
        Dispatcher handle;
        public NewTextBlock(Dispatcher dispatcher) => handle = dispatcher;
        public ObservableCollection<EfficientTextBlock> Lines { get; private set; } = new ObservableCollection<EfficientTextBlock>();
        private string _text;
        public string Text 
        {
            get => this._text;
            set
            {
                this._text = value;
                this.handle.Invoke(this.Lines.Clear);
                StringBuilder currentLine = new StringBuilder();
                int cLen = 1;
                foreach (char @char in this._text)
                {
                    if (@char == '\n' || cLen == this._text.Length)
                    {
                        if (cLen == this._text.Length)
                            currentLine.Append(@char);

                        var bl = new EfficientTextBlock { Text = currentLine.ToString().TrimEnd() };
                        this.handle.Invoke(() =>
                        {
                            this.Lines.Add(bl);
                        });
                        currentLine.Clear();
                    }
                    else
                    {
                        currentLine.Append(@char);
                    }
                    cLen++;
                }
            }
        }
        private bool _readOnly;
        public bool IsReadOnly 
        {
            get => this._readOnly;
            set
            {
                this._readOnly = value;

                foreach(var line in this.Lines)
                { 
                    line.IsReadOnly = value;
                }
            } 
        }
    }

    public class EfficientTextBlock : INotifyPropertyChanged
    {
        private string _textValue;

        public string Text
        {
            get { return _textValue; }
            set
            {
                _textValue = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private bool _readOnly;

        public bool IsReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
