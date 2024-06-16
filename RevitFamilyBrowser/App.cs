using Autodesk.Revit.UI;
using System.Reflection;
using zRevitFamilyBrowser.Properties;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI.Events;
using zRevitFamilyBrowser.Commands;
using zRevitFamilyBrowser.Definitions;
using zRevitFamilyBrowser.Events;
using zRevitFamilyBrowser.Models;
using zRevitFamilyBrowser.Windows;
using System.Drawing;
using zRevitFamilyBrowser.Extensions;
using Autodesk.Revit.DB;
using System.Drawing.Imaging;
using Settings = zRevitFamilyBrowser.Properties.Settings;

namespace zRevitFamilyBrowser
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            CreateRibbonPanel(application);
            CreateDockPanel(application);
            LoadRootFolderSettings();

            application.ControlledApplication.DocumentOpened += OnDocOpened;
            application.ViewActivated += OnViewActivated;

            return Result.Succeeded;
        }

        /// <summary>
        /// Создание таба в основном меню
        /// https://help.autodesk.com/view/RVT/2019/ENU/?guid=Revit_API_Revit_API_Developers_Guide_Introduction_Add_In_Integration_Ribbon_Panels_and_Controls_html
        /// </summary>
        private void CreateRibbonPanel(UIControlledApplication application)
        {
            var tabName = "Familien Browser";
            var assemblyPath = Assembly.GetExecutingAssembly().Location;

            application.CreateRibbonTab(tabName);

            var showPanelButton = new PushButtonData("ShowPanel", "Показать панель", assemblyPath, typeof(ShowDockablePanel).FullName)
            {
                LargeImage =  Resources.IconShowPanel.GetImage()
            };

            var openFolderButton = new PushButtonData("OpenFolder", "Открыть папку", assemblyPath, typeof(FolderSelect).FullName)
            {
                LargeImage = Resources.OpenFolder.GetImage()
            };

            var spaceButton = new PushButtonData("Space", "Grid Elements\nInstall", assemblyPath, typeof(Space).FullName)
            {
                LargeImage = Resources.Grid.GetImage(),
                ToolTip = "1. Select item form browser.\n2. Pick room in project\n3. Adjust item position and quantity."
            };

            var showSettingsButton = new PushButtonData("Settings", "Настройки", assemblyPath, typeof(ShowSettings).FullName)
            {
                LargeImage = Resources.settings.GetImage()
            };

            var ribbonPanel = application.CreateRibbonPanel("Familien Browser", "Familien Browser");

            ribbonPanel.AddItem(showPanelButton);
            ribbonPanel.AddItem(openFolderButton);
            ribbonPanel.AddItem(spaceButton);
            ribbonPanel.AddSeparator();
            ribbonPanel.AddItem(showSettingsButton);
        }

        /// <summary>
        /// Создание высящей панели инструментов
        /// https://help.autodesk.com/view/RVT/2019/ENU/?guid=Revit_API_Revit_API_Developers_Guide_Advanced_Topics_Dockable_Dialog_Panes_html
        /// </summary>
        private void CreateDockPanel(UIControlledApplication application)
        {
            var singleInstallEvent = new SingleInstallEvent();
            var externalEvent = ExternalEvent.Create(singleInstallEvent);

            var dockPanel = new DockPanel(externalEvent, singleInstallEvent);
            var dockablePaneId = new DockablePaneId(PanelIds.FamilyBrowserPanelId);
            application.RegisterDockablePane(dockablePaneId, "Familien Browser", (IDockablePaneProvider)dockPanel);
        }

        private void LoadRootFolderSettings()
        {
            if (File.Exists(Settings.Default.SettingPath) == false)
                return;
            
            Settings.Default.RootFolder = File.ReadAllText(Settings.Default.SettingPath);
            Settings.Default.Save();
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            a.ControlledApplication.DocumentOpened -= OnDocOpened;
            a.ViewActivated -= OnViewActivated;

            Properties.Settings.Default.FamilyPath = string.Empty;
            Properties.Settings.Default.FamilyName = string.Empty;
            Properties.Settings.Default.FamilySymbol = string.Empty;
            Properties.Settings.Default.Save();
          
            return Result.Succeeded;
        }

        private void OnViewActivated(object sender, ViewActivatedEventArgs e)
        {
            CreateImages(e.Document);
            Tools.CollectFamilyData(e.Document);
        }

        private void OnDocOpened(object sender, DocumentOpenedEventArgs e)
        {
            CreateImages(e.Document);
            Tools.CollectFamilyData(e.Document);
        }

        /// <summary>
        ///     Создание изображений для всех элементов во временной папке
        /// </summary>
        public void CreateImages(Document document)
        {
            var temporaryImageFolder = Path.GetTempPath() + "FamilyBrowser\\";

            if (Directory.Exists(temporaryImageFolder) == false)
                Directory.CreateDirectory(temporaryImageFolder);

            var familyInstances = new FilteredElementCollector(document).OfClass(typeof(FamilyInstance));
            var defaultImageSize = new System.Drawing.Size(200, 200);

            foreach (var familyInstance in familyInstances)
            {
                var elementId = familyInstance.GetTypeId();
                var element = document.GetElement(elementId) as ElementType;
                var filename = Path.Combine(temporaryImageFolder, $"{element.Name}.bmp");

                if (File.Exists(filename))
                    continue;

                var image = element.GetPreviewImage(defaultImageSize);

                using var file = new FileStream(filename, FileMode.Create, FileAccess.Write);

                image.Save(file, ImageFormat.Png);
            }
        }
    }
}