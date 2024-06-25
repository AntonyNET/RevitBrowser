using System.Windows.Controls;
using Autodesk.Revit.UI;

namespace zRevitFamilyBrowser.Windows
{
    public partial class DockPanel : UserControl, IDockablePaneProvider
    {
        public DockPanel()
        {
            InitializeComponent();
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;
            data.InitialState.DockPosition = DockPosition.Left;
        }
    }
}