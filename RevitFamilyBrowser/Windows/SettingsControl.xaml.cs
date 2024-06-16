using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Ookii.Dialogs.Wpf;

namespace zRevitFamilyBrowser.Windows
{
    public partial class SettingsControl : UserControl
    {
        private string DefaultFamilyPath { get; set; }
        public SettingsControl()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.DefaultSettingsPath))
                this.TextBoxFamily.Text = Properties.Settings.Default.DefaultFamilyPth;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.DefaultSettingsPath))
                this.TextBoxSettings.Text = Properties.Settings.Default.DefaultSettingsPath;

        }

        private void SelectInitialFamilyFolder(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog ofd = new VistaFolderBrowserDialog();
            if (ofd.ShowDialog() == true)
            {
                TextBoxFamily.Text = ofd.SelectedPath;
                DefaultFamilyPath = ofd.SelectedPath;
            }
        }

        private void SelectSettingsPath(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog ofd = new VistaFolderBrowserDialog();
            if (ofd.ShowDialog() == true)
            {
                string fileName = @"\FamilyBrowser.ini";
                TextBoxSettings.Text = ofd.SelectedPath + fileName;
            }
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            string pathIni = TextBoxSettings.Text;
            var parentWindow = Window.GetWindow(this);
            bool valid = true;


            try
            {
                System.IO.Path.GetFullPath(pathIni);
            }
            catch (Exception)
            {
                valid = false;
                parentWindow.Close();
            }

            try
            {
                System.IO.Path.GetFullPath(DefaultFamilyPath);
            }
            catch (Exception)
            {
                DefaultFamilyPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            if (valid)
            {
                File.WriteAllText(pathIni, DefaultFamilyPath + @"\");
                Properties.Settings.Default.SettingPath = pathIni;
                Properties.Settings.Default.Save();
            }

            Properties.Settings.Default.DefaultFamilyPth = DefaultFamilyPath;
            Properties.Settings.Default.DefaultSettingsPath = pathIni;

            parentWindow?.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
    }
}
