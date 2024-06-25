using System.Windows.Controls;
using Autodesk.Revit.UI;
using zRevitFamilyBrowser.Events;

namespace zRevitFamilyBrowser.Windows
{
    public partial class DockPanel : UserControl, IDockablePaneProvider
    {
        private ExternalEvent _externalEvent;
        private readonly SingleInstallEvent _singleInstallEvent;

        public DockPanel()
        {
            InitializeComponent();

            _singleInstallEvent = new SingleInstallEvent();
            _externalEvent = ExternalEvent.Create(_singleInstallEvent);
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;
            data.InitialState.DockPosition = DockPosition.Left;
        }
    }
}