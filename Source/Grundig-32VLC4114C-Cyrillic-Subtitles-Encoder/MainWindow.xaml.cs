namespace Grundig_32VLC4114C_Cyrillic_Subtitles_Encoder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Microsoft.Win32;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Reviewed. Suppression is OK here.")]
    public partial class MainWindow : Window
    {
        private readonly HashSet<string> convertedFiles;
        private readonly List<FileDialogFilter> fileDialogFilters;
        private byte[] bytes = new byte[0];
        private Encoding encoding = Encoding.GetEncoding(1251);
        private string path = string.Empty;

        public MainWindow()
        {
            this.InitializeComponent();
            this.CenterWindowOnScreen();
            this.convertedFiles = new HashSet<string>();
            this.fileDialogFilters = new List<FileDialogFilter>
                                         {
                                             new FileDialogFilter("SubRip Text", ".srt"),
                                             new FileDialogFilter("DVD Subtitle", ".sub"),
                                             new FileDialogFilter("All files", ".*")
                                         };
            this.StatusLabel.Content = "No file selected.";
            this.InsertCyrillicComboItems();
        }

        private void CenterWindowOnScreen()
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            var windowWidth = this.Width;
            var windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void InsertAllComboItems()
        {
            this.InsertComboItems(Encodings.GetAllEncodings());
        }

        private void InsertComboItems(List<EncodingInfo> list)
        {
            this.ComboBox.SelectedItem = null;
            this.ComboBox.Items.Clear();
            list.ForEach(
                e =>
                    {
                        var item = new ComboBoxItem { Content = $"{e.DisplayName}({e.Name}) - {e.CodePage}" };
                        if (e.CodePage == 1251)
                        {
                            item.IsSelected = true;
                        }

                        this.ComboBox.Items.Add(item);
                    });
        }

        private void InsertCyrillicComboItems()
        {
            this.InsertComboItems(Encodings.GetCyrillicEncodings());
        }

        private void OnCheckBoxChange(object sender, RoutedEventArgs e)
        {
            var isChecked = ((CheckBox)sender).IsChecked;
            if (isChecked.HasValue && isChecked.Value)
            {
                this.InsertAllComboItems();
            }
            else
            {
                this.InsertCyrillicComboItems();
            }
        }

        private void OnConvertClick(object sender, RoutedEventArgs e)
        {
            if (!this.convertedFiles.Contains(this.path))
            {
                this.convertedFiles.Add(this.path);
                Converter.Convert(
                    this.path,
                    this.bytes,
                    this.encoding,
                    ex => MessageBox.Show(
                        $"Exception: {Environment.NewLine}{ex.Message}",
                        "Alert message",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error));
                this.StatusLabel.Background = Brushes.Green;
                if (this.OpenAfterConvert.IsChecked.HasValue && this.OpenAfterConvert.IsChecked.Value)
                {
                    var programFilesX86Path = Environment.GetEnvironmentVariable(
                        Environment.Is64BitProcess ? "ProgramFiles(x86)" : "ProgramFiles");
                    var notepadPlusPlusPath = $"{programFilesX86Path}\\Notepad++\\notepad++.exe";
                    if (File.Exists(notepadPlusPlusPath))
                    {
                        Process.Start(notepadPlusPlusPath, this.path);
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "Don't try to convert file which is already converted twice.",
                    "Alert message",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void OnOpenFileClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = string.Join("|", this.fileDialogFilters) };
            if (ofd.ShowDialog() == true)
            {
                this.path = ofd.FileName;
                var pathParts = ofd.FileName.Split(Path.DirectorySeparatorChar);
                this.StatusLabel.Content = pathParts[pathParts.Length - 1];
                this.bytes = File.ReadAllBytes(ofd.FileName);
                if (!this.ConvertButton.IsEnabled)
                {
                    this.ConvertButton.IsEnabled = true;
                }

                this.StatusLabel.Background = Brushes.LightSeaGreen;
            }
        }

        private void OnSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            var items = ((ComboBox)sender).Items;
            var content = ((ComboBoxItem)items[0]).Content as string;
            foreach (var item in items)
            {
                var currentItem = (ComboBoxItem)item;
                if (currentItem.IsSelected)
                {
                    content = currentItem.Content as string;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(content))
            {
                var codePage = int.Parse(content.Split(new[] { " - " }, StringSplitOptions.None)[1]);
                this.encoding = Encoding.GetEncoding(codePage);
            }
        }
    }
}