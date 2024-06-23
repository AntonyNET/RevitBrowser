using Autodesk.Revit.UI;
using System.Reflection;
using zRevitFamilyBrowser.Properties;
using System.IO;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI.Events;
using zRevitFamilyBrowser.Commands;
using zRevitFamilyBrowser.Definitions;
using zRevitFamilyBrowser.Windows;
using zRevitFamilyBrowser.Extensions;
using Settings = zRevitFamilyBrowser.Properties.Settings;
using zRevitFamilyBrowser.Services;

namespace zRevitFamilyBrowser
{
    class App : IExternalApplication
    {
        // когда айэнум, когда аррей, когда []? 

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
            var tabName = "Family Browser";
            var assemblyPath = Assembly.GetExecutingAssembly().Location;


            application.CreateRibbonTab(tabName);

            var showPanelButton = new PushButtonData("ShowPanel", "Показать панель", assemblyPath, typeof(ShowDockablePanel).FullName)
            {
                LargeImage = Resources.IconShowPanel.GetImage() // как картинки попадают в ресурсы?
            };

            var showSettingsButton = new PushButtonData("Settings", "Настройки", assemblyPath, typeof(ShowSettings).FullName)
            {
                LargeImage = Resources.settings.GetImage()
            };

            var ribbonPanel = application.CreateRibbonPanel("Family Browser", "Family Browser");

            ribbonPanel.AddItem(showPanelButton);
            ribbonPanel.AddSeparator();
            ribbonPanel.AddItem(showSettingsButton);
        }

        /// <summary>
        /// Создание высящей панели инструментов
        /// https://help.autodesk.com/view/RVT/2019/ENU/?guid=Revit_API_Revit_API_Developers_Guide_Advanced_Topics_Dockable_Dialog_Panes_html
        /// </summary>
        private void CreateDockPanel(UIControlledApplication application)
        {
            var dockPanel = new DockPanel();
            var dockablePaneId = new DockablePaneId(PanelIds.FamilyBrowserPanelId);
            application.RegisterDockablePane(dockablePaneId, "Family Browser", (IDockablePaneProvider)dockPanel);
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
          
            return Result.Succeeded;
        }

        private void OnViewActivated(object sender, ViewActivatedEventArgs e)
        {
            FamilyService.CollectFamilyData(e.Document);
        }

        private void OnDocOpened(object sender, DocumentOpenedEventArgs e)
        {
            FamilyService.CollectFamilyData(e.Document);
        }
    }
}