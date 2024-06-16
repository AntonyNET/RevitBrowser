using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using zRevitFamilyBrowser.Windows;

namespace zRevitFamilyBrowser.Commands
{
    [Transaction(TransactionMode.Manual)]
    class ShowSettings : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SettingsControl settings = new SettingsControl();

            Window window = new Window
            {
                Height = 210,
                Width = 600,
                Title = "Familien Browser Settings",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = settings,
                Background = System.Windows.Media.Brushes.WhiteSmoke,
                WindowStyle = WindowStyle.ToolWindow,
                Name = "Settings",
                ResizeMode = ResizeMode.NoResize
            };
            window.Show();
            return Result.Succeeded;
        }
    }
}
